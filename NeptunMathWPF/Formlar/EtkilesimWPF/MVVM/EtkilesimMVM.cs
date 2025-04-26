
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

    class EtkilesimMVM : ObservableObject
    {
        //Debug Tools: 
        public string[] DebugComboBoxTurler { get; set; }
        public string cmBxSecilen { get; set; }
        public ObservableCollection<ifadeTuru> CokluIfadeTurlerListColl { get; set; }
        public ICommand DebugCokluIfadeEkle { get; set; }
        public ICommand DebugCokluIfadeSil { get; set; }
        public ICommand DenemeEkleKomut { get; set; }

        //ObservableCollectionlar
        public ObservableCollection<SoruCardModel> Sorular  { get; set; }

        private ObservableCollection<string> _secenekler;
        public ObservableCollection<string> secenekler
        {
            get => _secenekler; set
            {

                if (_secenekler != value)
                {
                    _secenekler = value;
                    OnPropertyChanged(nameof(secenekler));
                }
            }
        }
        public ICommand SeciliTurDegistir { get; set; }

        internal KeyEventHandler key;
        public SoruCardModel secilisoru { get; set; }

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
                DenemeEkleKomut = new RelayCommand(o => Ekle());
                SeciliTurDegistir = new RelayCommand(o => TurDegis());
                DebugCokluIfadeSil = new RelayCommand(o => DebugCokluIfadeCollSil(o));
                DebugCokluIfadeEkle = new RelayCommand(o => CokluIfadeListBoxEkle());

                OnPropertyChanged(nameof(DebugComboBoxTurler));

                seciliTur = "SoruModu";
                Ekle();
                OnPropertyChanged();
            });
        }

        public void Ekle(int a=1)
        {
            Genel.Handle(() => {

                List<ifadeTuru> ifadeTurleri = new List<ifadeTuru>();

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

                secilisoru = new SoruCardModel(soru)
                {
                    zaman = DateTime.Now,
                    kaynak = "Yerel"
                };

                secenekler = secilisoru.NesneSecenekler;

                //Aşağıdan Ekle
                Sorular.Add(secilisoru);

                //Yukarıdan Ekle
                //Sorular.Insert(0,secilisoru);

                seciliTur = "SoruModu";
                OnPropertyChanged();
            });
        }


        public void TurDegis()
        {
            if(seciliTur == "SoruModu")
            {
                seciliTur = "Proompter";
            }
            else
            {
                seciliTur = "SoruModu";
            }

            MessageBox.Show($"Panel Değişti {seciliTur}");
        }

        private static ifadeTuru turdondur(ifadeTuru[] turler)
        {
            Random rng = new Random();
            ifadeTuru tur = turler[rng.Next(0, turler.Length)];
            return tur;
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
            if (obje != null)
            {
                int index = (int)obje;
         

                if (index < CokluIfadeTurlerListColl.Count)
                {
                    CokluIfadeTurlerListColl.RemoveAt(index);
                }
            }
        }

        public List<ifadeTuru> standartIfadeList()
        {
            Random rng = new Random();

            List<ifadeTuru> ifadeTurleri = new List<SoruTerimleri.ifadeTurleri> {

                turdondur(new ifadeTuru[] { ifadeTuru.sayi, ifadeTuru.faktoriyel }),
                ifadeTuru.sayi,

                turdondur(new ifadeTuru[] { ifadeTuru.sayi, ifadeTuru.kesir }),
                
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
