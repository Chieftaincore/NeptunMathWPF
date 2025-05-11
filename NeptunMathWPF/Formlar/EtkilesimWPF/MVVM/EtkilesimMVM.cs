
using Microsoft.Win32;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM
{
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
    using soruTuru = SoruTerimleri.soruTuru;

    /// <summary>
    /// EtkilesimMVVM' EtkilesimWPF penceresi için Main Model Nesnesidir
    /// 
    /// Sınıfın içinde Penecerenin çalışması için önemli değişkenler bulunuyor
    /// </summary>
    class EtkilesimMVM : ObservableObject
    {
        //Debug Tools: Geliştirme de denemeler için
        public string[] DebugComboBoxTurler { get; set; }
        public string cmBxSecilen { get; set; }
        public ObservableCollection<ifadeTuru> CokluIfadeTurlerListColl { get; set; }
        public ICommand DebugCokluIfadeEkle { get; set; }
        public ICommand DebugAPIProblemEkle { get; set; }
        public ICommand DebugCokluIfadeSil { get; set; }
        public ICommand DebugIslemEkleKomut { get; set; }
        public ICommand SeciliTurDegistir { get; set; }
        public ICommand SecimDegistir { get; set; }
        public ICommand HesapMakinesiGosterGizle { get; set; }
        public ICommand DebugFonksiyonSoruOlustur { get; set; }

        //SoruListesini Belirliyor Görünen Soru Modelleri Koleksiyonu

        public bool APIvar { get; set; }
        public ObservableCollection<SoruCardModel> Sorular { get; set; }
        public SoruCardModel seciliSoru { get; set; }

        public SeceneklerModel secenekler
        {

            get => seciliSoru.NesneSecenekler;
        }
        public string seciliSecenek
        {

            get => secenekler.secilideger;

            set { secenekler.secilideger = value; }
        }

        //sonraki sorunun ne geleceğini belirleyen algorithma için
        public Func<Soru> sonrakiSoruAlgorithmasi { get; set; }
        public HesapMakinesiModel hesap { get; set; } = new HesapMakinesiModel();
        public ICommand SoruSec { get; set; }
        public ICommand SoruCevapla { get; set; }
        public ICommand CiktiAL { get; set; }
        public string IsSelected { get; set; }

        internal KeyEventHandler key;

        private string _seciliTur;
        public string seciliTur
        {
            get => _seciliTur; set
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
            //Gemini API bağlımı kontrol etmek için.
            APIcheck();

            DebugAPIProblemEkle = new RelayCommand(o => ProblemEkle());

            Genel.Handle(() =>
            {
                Sorular = new ObservableCollection<SoruCardModel>();
                CokluIfadeTurlerListColl = new ObservableCollection<ifadeTuru>();
                DebugComboBoxTurler = Enum.GetNames(typeof(ifadeTuru));

                //MVVM'de Komutları bu sınıfa yazım alttaki KomutlarInit()'in gövdesinde belirtmeniz gerek
                KomutlarInit();

                OnPropertyChanged(nameof(DebugComboBoxTurler));

                Ekle();

                seciliTur = seciliSoru.SoruTuruStyleTemplate();
                OnPropertyChanged();
            });
        }

        public void Ekle()
        {
            Genel.Handle(() =>
            {

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

                seciliSoru = new SoruCardModel(soru)
                {
                    zaman = DateTime.Now,
                    kaynak = "Yerel"
                };

                seciliTur = seciliSoru.SoruTuruStyleTemplate();
                Sorular.Add(seciliSoru);

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();
            });
        }

        public async Task ProblemEkle()
        {
            try
            {
                Soru soru = await SoruAjani.ProblemSorusuOlustur();

                seciliSoru = new SoruCardModel(soru)
                {
                    LaTeX = soru.GetMetin(),
                    zaman = DateTime.Now,
                    kaynak = "Yapay Zeka API"

                };

                seciliTur = seciliSoru.SoruTuruStyleTemplate();

                Sorular.Add(seciliSoru);

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();

            }
            catch
            {
                MessageBox.Show("Problem Eklenirken Sorun Oluştu", "Problem Sorunu",
                    MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        public void SoruCardSec(object o)
        {

            Genel.Handle(() =>
            {

                seciliSoru = (SoruCardModel)o;

                seciliTur = seciliSoru.SoruTuruStyleTemplate();

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();
            });
        }

        public void FonksiyonSoruEkle()
        {
            Genel.Handle(() =>
            {
                seciliTur = "SoruModuMetin";

                Soru soru = SoruAjani.RastgeleFonksiyonSorusuOlustur();

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
            if (seciliTur == "SoruModuNormal")
            {
                seciliTur = "Proompter";
            }
            else
            {
                seciliTur = "SoruModuNormal";
            }

            MessageBox.Show($"Panel Değişti {seciliTur}");
        }

        /// <summary>
        /// GEMINI API dosyasının boş olup olmadığını kontrol eder, 
        /// </summary>
        internal void APIcheck()
        {
            Genel.Handle(() =>
            {
                if (File.Exists("GEMINI.config"))
                {
                    if (APIOperations.GetGeminiApiKey() != null && APIOperations.GetGeminiApiKey() != string.Empty)
                    {
                        APIvar = true;
                    }
                    else
                    {
                        MessageBox.Show("API key bulunamadı, YZ API işlemleri devre dışı,", "API Devre dışı"
                            , MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                    }
                }
                else
                {
                    APIvar = false;

                    MessageBox.Show("GEMINI.config dosyası bulunamadı YZ API işlemleri devre dışı, " +
                        "Lütfen Dosyayı Oluşturup içine API key atınız", "API Devre Dışı", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            });
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


                // DB KAYIT
                AddWrongQuestionToDB();

            }
        }

        private void AddWrongQuestionToDB()
        {
            string soruTur = seciliSoru.Tur.ToString();
            var topic = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == soruTur);

            var answer = secenekler.DogruSecenekGetir();
            var wrongAnswersList = secenekler.secenekler.Where(x=>x!=answer).ToList();
            string wrongAnswers = "";
            foreach(var item in wrongAnswersList)
            {
                wrongAnswers += item+"#";
            }

            //subtopic eklenecek
            Genel.Handle(() =>
            {
                Genel.ReloadEntity();
                Genel.dbEntities.WRONG_ANSWERED_QUESTIONS.Add(new WRONG_ANSWERED_QUESTIONS
                {
                    USERID = aktifKullanici.kullnId,
                    //TOPICS = topic,
                    TOPIC_ID = 5,
                    SUBTOPIC_ID = 5,
                    QUESTION_TEXT = seciliSoru.soru.IslemMetin,
                    LATEX_TEXT = seciliSoru.LaTeX,
                    ANSWER = answer,
                    USERS_ANSWER = secenekler.secilideger,
                    WRONG_ANSWERS = wrongAnswers
                });
                Genel.dbEntities.SaveChanges();
            });
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


        /// <summary>
        /// MVVM'de komutlar Relaylenerek işlenmesi gerekir lütfen içine komutunuzu koyunuz
        /// </summary>
        internal void KomutlarInit()
        {
            SoruSec = new RelayCommand(o => SoruCardSec(o));
            DebugIslemEkleKomut = new RelayCommand(o => Ekle());
            SeciliTurDegistir = new RelayCommand(o => tusTurDegis());
            SoruCevapla = new RelayCommand(o => SeciliSoruCevapla(o));
            SecimDegistir = new RelayCommand(o => SeceneklerSecimDegistir(o));
            DebugCokluIfadeSil = new RelayCommand(o => DebugCokluIfadeCollSil(o));
            DebugFonksiyonSoruOlustur = new RelayCommand(o => FonksiyonSoruEkle());
            DebugCokluIfadeEkle = new RelayCommand(o => CokluIfadeListBoxEkle());
            HesapMakinesiGosterGizle = new RelayCommand(o => hesap.GosterGizle());
            CiktiAL = new RelayCommand(o => PDFlatexCiktiAl());
        }

        public void PDFlatexCiktiAl()
        {
            if (MessageBox.Show("pdflatex tarafından dosya çıkarılması için bilgisayarınızda standart LatexLive kütüphanesi veya pdflatex komutları taşıyan bir TeX motoru bulunmalıdır", "Gereksinim Bildirisi", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                FileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Klasör | Directory",
                    Title = "Kaydetmek için klasör yeri ve ismi (Ek dosyalar eklenebilir)",
                    FileName = $"LatexDeneme{DateTime.Now:yyyyMMdd_HH}",

                };


                if (saveFileDialog.ShowDialog() == true)
                {
                    new PDFLatexYineleyici().LaTeXPDFolustur(Sorular, saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show("Seçim yapılmadı", "Geçersiz Dosya Seçimi", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
