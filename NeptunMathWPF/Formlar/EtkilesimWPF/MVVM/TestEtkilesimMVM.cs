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

        ZamanlayiciModel _zamanlayici;
        public ZamanlayiciModel Zamanlayici { get => _zamanlayici; 
            set 
            {
                if (_zamanlayici != value) 
                {
                    _zamanlayici = value;
                    Zamanlayici.ZamanBittiEvent += ZamanBittiKilitle;
                    OnPropertyChanged(nameof(Zamanlayici));
                }
            }
        }

        public ICommand TestBitir { get; set; }
        public string Baslik { get; set; }

        public bool SureDevam { get {
                return Zamanlayici._Sure > TimeSpan.Zero;
            }
        }

        public override string seciliTur { 
            
            get => base.seciliTur;

            set { 
                if(!TestKilitlendi)
                base.seciliTur = value; 
            }
        }

        internal bool TestKilitlendi = false;

        public TestEtkilesimMVM(int _Sorusayisi)
        {
            SoruSayisi = _Sorusayisi;
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

                    if (algodurum && !TestKilitlendi)
                    {
                        if (Sorular.Count < SoruSayisi && algodurum)
                        {
                            MessageBox.Show("Soru Atlandı", "Cevap Seçilmedi", MessageBoxButton.OK, MessageBoxImage.Information);

                            AlgorithmaSoruEkle();
                        }
                        else
                        {
                            MessageBox.Show("Son Soru","Atlanamaz",MessageBoxButton.OK,MessageBoxImage.Hand);
                        }
                    }
                        
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
                    if (Sorular.Count < SoruSayisi)
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

            OnPropertyChanged(nameof(seciliTur));
        } 

        internal void SessionBitir()
        {
            int _sorusayisi = Sorular.Count;

            int _dogru = 0;

            int _yanlis = 0;

            foreach (SoruCardModel ScM in Sorular)
            {
                if(ScM.NesneSecenekler.CevaplanmaDurumu == SeceneklerModel.CevapDurum.Dogru)
                {
                    _dogru++;
                }
                else
                {
                    if (ScM.NesneSecenekler.CevaplanmaDurumu == SeceneklerModel.CevapDurum.Yanlis)
                        _yanlis++;
                }
            }
           
            MessageBox.Show($"Test Bitti {Baslik} \rSoru Sayisi : {_sorusayisi} \rDogru Sayisi : {_dogru}\rYanlis Sayisi : {_yanlis}\r Başlangıç : {BaslangicZaman}" ,"test bitti");
        }

        private void ZamanBittiKilitle(object sender, EventArgs e)
        {
            SessionSonuIsaretle();

            TestKilitlendi = true;
        }
    }
}
