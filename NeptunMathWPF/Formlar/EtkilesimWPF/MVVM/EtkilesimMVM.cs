
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM
{
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
    using soruTuru = SoruTerimleri.soruTuru;
    class EtkilesimMVM : ObservableObject
    {
        //Debug Tools: Geliştirme de denemeler için
        public string[] DebugComboBoxTurler { get; set; }
        public string cmBxSecilen { get; set; }
        public ObservableCollection<ifadeTuru> CokluIfadeTurlerListColl { get; set; }
        public ICommand DebugCokluIfadeEkle { get; set; }
        public ICommand DebugCokluIfadeSil { get; set; }
        public ICommand DebugIslemEkleKomut { get; set; }
        public ICommand SeciliTurDegistir { get; set; }
        public ICommand SecimDegistir { get; set; }
        public ICommand DebugFonksiyonSoruOlustur { get; set; }

        //SoruListesini Belirliyor Görünen Soru Modelleri Koleksiyonu
        public ObservableCollection<SoruCardModel> Sorular  { get; set; }
        public SoruCardModel seciliSoru { get; set; }
        public SeceneklerModel secenekler { 

            get => seciliSoru.NesneSecenekler;
        }
        public string seciliSecenek {

            get => secenekler.secilideger;

            set { secenekler.secilideger = value; }
        }

        //sonraki sorunun ne geleceğini belirleyen algorithma için
        public Func<Soru> sonrakiSoruAlgorithmasi { get; set; }
        public ICommand SoruCevapla { get; set; }
        public string IsSelected { get; set; }

        internal KeyEventHandler key;

        private string _seciliTur;
        public string seciliTur{ get => _seciliTur ; set
            {
                if (_seciliTur != value)
                {
                    _seciliTur = value;
                    OnPropertyChanged(nameof(seciliTur));
                }
            }
        }

        public EtkilesimMVM()
        {
            Genel.Handle(() =>
            {
                Sorular = new ObservableCollection<SoruCardModel>();
                CokluIfadeTurlerListColl = new ObservableCollection<ifadeTuru>();

                DebugComboBoxTurler = Enum.GetNames(typeof(ifadeTuru));

                //MVVM'de Komutları bu sınıfa yazım altta belirtmeniz gerek
                DebugIslemEkleKomut = new RelayCommand(o => Ekle());
                SeciliTurDegistir = new RelayCommand(o => tusTurDegis());
                SoruCevapla = new RelayCommand(o => SeciliSoruCevapla(o));
                SecimDegistir = new RelayCommand(o => SeceneklerSecimDegistir(o));
                DebugCokluIfadeSil = new RelayCommand(o => DebugCokluIfadeCollSil(o));
                DebugFonksiyonSoruOlustur = new RelayCommand(o => FonksiyonSoruEkle());
                DebugCokluIfadeEkle = new RelayCommand(o => CokluIfadeListBoxEkle());

                OnPropertyChanged(nameof(DebugComboBoxTurler));
  
                seciliTur = "SoruModuNormal";
                Ekle();
                OnPropertyChanged();
            });
        }

        public void Ekle()
        {
            Genel.Handle(() => {

                List<ifadeTuru> ifadeTurleri = new List<ifadeTuru>();
                seciliTur = "SoruModuNormal";

                if (CokluIfadeTurlerListColl.Count < 2)
                {
                    ifadeTurleri = standartIfadeList();
                }
                else
                {
                    IEnumerable<ifadeTuru> ifadeListconv = CokluIfadeTurlerListColl;
                    ifadeTurleri = new List<ifadeTuru>(ifadeListconv);
                }

                List<Ifade> Liste = SoruAjani.CokluIfadeListesiOlustur(ifadeTurleri);
                Soru soru = SoruAjani.YerelSoruBirlestir(Liste, 5);

                seciliSoru = new SoruCardModel(soru)
                {
                    zaman = DateTime.Now,
                    kaynak = "Yerel"
                };

                //Aşağıdan Ekle
                Sorular.Add(seciliSoru);

                //Yukarıdan Ekle
                //Sorular.Insert(0,secilisoru);

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();
            });
        }

        public void FonksiyonSoruEkle()
        {
            Genel.Handle(() =>
            {
                seciliTur = "SoruModuMetin";

                Soru soru = SoruAjani.RastgeleFonksiyonSorusuOlustur(seceneksayisi: 4);

                seciliSoru = new SoruCardModel(soru)
                {
                    LaTeX = soru.GetMetin(),
                    zaman = DateTime.Now,
                    kaynak = "Yerel"
                };

                Sorular.Add(seciliSoru);

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();
            });
        }

        public void tusTurDegis()
        {
            if(seciliTur == "SoruModuNormal")
            {
                seciliTur = "Proompter";
            }
            else
            {
                seciliTur = "SoruModuNormal";
            }

            MessageBox.Show($"Panel Değişti {seciliTur}");
        }

        private static ifadeTuru turdondur(ifadeTuru[] turler)
        {
            Random rng = new Random();
            ifadeTuru tur = turler[rng.Next(0, turler.Length)];
            return tur;
        }

        private void SeciliSoruCevapla(object o)
        {
            //Radioboxlar Nesnenin içine seciliDeğeri önceden göndermiştir
            bool dogru = secenekler.Cevapla();

            if (dogru)
            {
                seciliSoru.EkYaziGuncelle("Doğru cevaplandı", 3);
            }
            else
            {
                seciliSoru.EkYaziGuncelle($"Yanlış cevaplandı | doğru cevap {secenekler.DogruSecenekGetir()}", 2);
            }
        }

        private void TusEkleClick(object sender, RoutedEventArgs e)
        {
            if (cmBxSecilen == null || String.IsNullOrEmpty(cmBxSecilen))
            {
                MessageBox.Show("Lütfen bir ifade türü seçin.");
                return;
            }

            string a = cmBxSecilen;

            if (a != null)
            {
                ifadeTuru tur;
                Enum.TryParse<ifadeTuru>(a, out tur);

                CokluIfadeTurlerListColl.Add(tur);
            }

            OnPropertyChanged();
        }

        private void SeceneklerSecimDegistir(object nesne)
        {
             secenekler.secilideger = (string)nesne;

             OnPropertyChanged(nameof(secenekler.secilideger));
        }

        private void CokluIfadeListBoxEkle()
        {
            if (!String.IsNullOrEmpty(cmBxSecilen))
            {
                ifadeTuru tur;
                Enum.TryParse<ifadeTuru>(cmBxSecilen, out tur);

                CokluIfadeTurlerListColl.Add(tur);
            }
        }

        private void DebugCokluIfadeCollSil(object obje)
        {
            Genel.Handle(() =>
            {
                if (obje != null)
                {
                    int index = (int)obje;

                    if (index + 1 < CokluIfadeTurlerListColl.Count)
                    {
                        CokluIfadeTurlerListColl.RemoveAt(index);
                    }
                }
            });
        }

        //Şuanki varsayılan soru için Ifadeler
        public List<ifadeTuru> standartIfadeList()
        {
            Random rng = new Random();

            List<ifadeTuru> ifadeTurleri = new List<SoruTerimleri.ifadeTurleri> {

                turdondur(new ifadeTuru[] { ifadeTuru.sayi, ifadeTuru.faktoriyel }),
                ifadeTuru.sayi,

                turdondur(new ifadeTuru[] { ifadeTuru.sayi, ifadeTuru.kesir, ifadeTuru.uslu}),
            };

            return ifadeTurleri;
        }

        //StackOverflow'dan aldım
        internal class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                if (execute == null) throw new ArgumentNullException("execute");

                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                _execute(parameter ?? "<N/A>");
            }
        }
    }

}
