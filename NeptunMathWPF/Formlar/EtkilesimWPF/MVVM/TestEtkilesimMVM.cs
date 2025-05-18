using HonkSharp.Fluency;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani.Algorithma;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static NeptunMathWPF.SoruTerimleri;

using ifadeTuru = NeptunMathWPF.SoruTerimleri.ifadeTurleri;
namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM
{
    class TestEtkilesimMVM : EtkilesimMVM
    {

        DateTime BaslangicZaman;

        internal int SoruSayisi;
        public ZamanlayiciModel Zamanlayici { get; set; } = new ZamanlayiciModel(1);
        public ICommand TestBitir { get; set; }

        public bool SureDevam { get {
                return Zamanlayici._Sure > TimeSpan.Zero;
            }
        }

        bool TestKilitlendi;

        public TestEtkilesimMVM()
        {
            SoruSayisi = 10;
            BaslangicZaman = DateTime.Now;

            //Gemini API bağlımı kontrol etmek için.
            APIcheck();

            DebugAPIProblemEkle = new RelayCommand(o => ProblemEkle());
            TestBitir = new RelayCommand(o => SessionBitir());

            Genel.Handle(() =>
            {
                Sorular = new ObservableCollection<SoruCardModel>();
                CokluIfadeTurlerListColl = new ObservableCollection<ifadeTuru>();
                DebugComboBoxTurler = Enum.GetNames(typeof(ifadeTuru));

                //Problemler için içine soruTuru.problem de ekleyin
                if (Algorithma == null)
                    Algorithma = new AlgorithmaModel(new soruTuru[] { soruTuru.islem, soruTuru.fonksiyon, soruTuru.limit });

                //MVVM'de Komutları bu sınıfa yazım alttaki KomutlarInit()'in gövdesinde belirtmeniz gerek
                KomutlarInit();

                OnPropertyChanged(nameof(DebugComboBoxTurler));

                DebugIslemSoruEkle();

                seciliTur = seciliSoru.SoruTuruStyleTemplate();
                OnPropertyChanged();
            });
        }

        internal override void SeciliSoruCevapla(object o)
        {
            Genel.Handle(() =>
            {
                if (string.IsNullOrEmpty(seciliSoru.NesneSecenekler.secilideger))
                {
                    MessageBox.Show("Herhangi bir cevap değeri alınmadı, Cevap Seçtiğinizden emin olun", "Cevap Seçilmedi", MessageBoxButton.OK, MessageBoxImage.Stop);

                    return;
                }

                //Radioboxlar Nesnenin içine seciliDeğeri önceden göndermiştir
                bool dogru = secenekler.Cevapla();

                if (dogru)
                {
                    seciliSoru.EkYaziGuncelle("Doğru cevaplandı", 3);

                    if (algodurum && Algorithma.RepoDenetim(seciliSoru.Tur))
                        Algorithma.ModelSeviyeArtır(seciliSoru.Tur);
                }
                else
                {
                    seciliSoru.EkYaziGuncelle($"Yanlış cevaplandı | doğru cevap {secenekler.DogruSecenekGetir()}", 2);

                    if (algodurum && Algorithma.RepoDenetim(seciliSoru.Tur))
                        Algorithma.ModelSeviyeAzalt(seciliSoru.Tur);

                    // DB KAYIT
                    if (!string.IsNullOrEmpty(aktifKullanici.kullaniciAdi))
                    {
                        AddWrongQuestionToDB();
                    }
                }
                
                if (Sorular.Last() != seciliSoru)
                {
                    int i = Sorular.IndexOf(seciliSoru);

                    seciliSoru = Sorular.ElementAt(i + 1);

                    OnPropertyChanged(nameof(seciliSoru));
                    OnPropertyChanged(nameof(secenekler));
                }

                seciliTur = seciliSoru.SoruTuruStyleTemplate();
                OnPropertyChanged(nameof(seciliTur));

                if (SonSoruCheck())
                {
                    if(Sorular.Count < SoruSayisi)
                    {
                        if (algodurum)
                            AlgorithmaSoruEkle();
                    }
                    else
                    {
                        SessionSonuIsaretle();
                    }
                }
            });
        }

        internal void SessionSonuIsaretle()
        {
            seciliTur = "TestBitti";
        } 

        internal void SessionBitir()
        {

        }
    }
}
