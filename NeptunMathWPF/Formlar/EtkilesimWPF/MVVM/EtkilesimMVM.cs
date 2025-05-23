﻿using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using NeptunMathWPF.SoruVeAjani.Algorithma;
using System.Windows.Controls;

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
        #region Debug
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
        public ICommand DebugLimitSoruOlustur { get; set; }
        public ICommand DebugTurevSoruOlustur { get; set; }
        public ICommand DebugLatexsizPDF { get; set; } //Latex olmadan PDF oluşturma komutu
        public ICommand AlgorithmaCheck { get; set; }
        public ICommand DebugLatexsizPDFOlustur { get; set; }
        public ICommand DebugHavuzSoruEkle { get; set; }
        #endregion

        //SoruListesini Belirliyor Görünen Soru Modelleri Koleksiyonu
        bool algodurum = true;
        public AlgorithmaModel Algorithma { get; set; }
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
        public ICommand PDFciktiAl { get; set; }

        #region SoruiciEylemler 

        public ICommand SoruYerIsaretiKaydet { get; set; }
        public ICommand SoruHataBildir { get; set; }
        public ICommand tusSoruMetaVeri { get; set; }

        #endregion
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

                //Problemler için içine soruTuru.problem de ekleyin
                if(Algorithma == null)
                    Algorithma = new AlgorithmaModel(new soruTuru[] { soruTuru.islem, soruTuru.fonksiyon, soruTuru.limit});

                //MVVM'de Komutları bu sınıfa yazım alttaki KomutlarInit()'in gövdesinde belirtmeniz gerek
                KomutlarInit();

                OnPropertyChanged(nameof(DebugComboBoxTurler));

                DebugIslemSoruEkle();

                seciliTur = seciliSoru.SoruTuruStyleTemplate();
                OnPropertyChanged();
            });
        }

        //Bitmedi
        internal void IlkEkleme()
        {
            Genel.Handle(() =>
            {
                if(Algorithma != null)
                {
                    if (Algorithma.repo.Zorluklar.Count == 1 && Algorithma.repo.Zorluklar.ContainsKey(soruTuru.problem))
                    {

                    }

                    Algorithma.Sonraki();
                }
                else
                {
                    DebugIslemSoruEkle();
                }
            });
        }

        public void DebugIslemSoruEkle()
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
            catch (Exception e)
            {
                MessageBox.Show($"Problem Eklenirken Sorun Oluştu {e}", "Problem Sorunu",
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
                Soru soru = SoruAjani.RastgeleFonksiyonSorusuOlustur();

                seciliSoru = new SoruCardModel(soru)
                {
                    zaman = DateTime.Now,
                    kaynak = "Yerel"
                };

                Sorular.Add(seciliSoru);

                seciliTur = seciliSoru.SoruTuruStyleTemplate();

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();
            });
        }

        /// <summary>
        /// MVVM UI öğeleri içindir Kutuları Değiştirir
        /// </summary>
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

        #region MVVMAlgorithma

        internal bool SonSoruCheck()
        {
            if (Sorular.Last().kilitlendi)
                return true;

            return false;
        }

        public void AlgorithmaSoruEkle()
        {
            Genel.Handle(() =>
            {
                Soru _soru = algoSonrakiSoruGetir();

                seciliSoru = new SoruCardModel(_soru)
                {
                    LaTeX = _soru.LatexMetin,
                    zaman = DateTime.Now,
                    kaynak = "Yerel"
                };

                Sorular.Add(seciliSoru);

                seciliTur = seciliSoru.SoruTuruStyleTemplate();

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();

            });
        }

        internal Soru algoSonrakiSoruGetir()
        {
            return Algorithma.Sonraki();
        }

        #endregion

        private static ifadeTuru turdondur(ifadeTuru[] turler)
        {
            Random rng = new Random();
            ifadeTuru tur = turler[rng.Next(0, turler.Length)];
            return tur;
        }

        private void SeciliSoruCevapla(object o)
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

                if (SonSoruCheck() && algodurum)
                {
                    AlgorithmaSoruEkle();
                }
            });
        }

        #region VeriTabani
        private void AddWrongQuestionToDB()
        {
            Genel.Handle(() =>
            {
                Genel.ReloadEntity();

                string soruTur = seciliSoru.Tur.ToString();
                string soruAltTur = seciliSoru.soru.AltTur.ToString();
                var topic = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == soruTur);

                // null kontrol (eğer db'de veri yoksa veriyi ekle)
                if (topic == null)
                {
                    Genel.dbEntities.TOPICS.Add(new TOPICS
                    {
                        TOPIC = soruTur,
                    });
                    Genel.dbEntities.SaveChanges();
                    topic = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == soruTur);
                }
                var subtopic = Genel.dbEntities.SUBTOPICS.FirstOrDefault(x => x.SUBTOPIC == soruAltTur);
                if (subtopic == null)
                {
                    Genel.dbEntities.SUBTOPICS.Add(new SUBTOPICS
                    {
                        SUBTOPIC = soruAltTur,
                        TOPICS = topic
                    });
                    Genel.dbEntities.SaveChanges();
                    subtopic = Genel.dbEntities.SUBTOPICS.FirstOrDefault(x => x.SUBTOPIC == soruAltTur);
                }

                //------cevaplar-----
                var answer = secenekler.DogruSecenekGetir();
                var wrongAnswersList = secenekler.secenekler.Where(x => x != answer).ToList();
                string wrongAnswers = "";
                //-------------------

                foreach (var item in wrongAnswersList)
                {
                    wrongAnswers += item + "#";
                }
                Genel.dbEntities.WRONG_ANSWERED_QUESTIONS.Add(new WRONG_ANSWERED_QUESTIONS
                {
                    USERID = aktifKullanici.kullnId,
                    TOPICS = topic,
                    SUBTOPICS = subtopic,
                    QUESTION_TEXT = seciliSoru.soru.IslemMetin,
                    LATEX_TEXT = seciliSoru.LaTeX,
                    ANSWER = answer,
                    USERS_ANSWER = secenekler.secilideger,
                    WRONG_ANSWERS = wrongAnswers
                });
                Genel.dbEntities.SaveChanges();
            });
        }

        private void BookmarkQuestionToDB(SoruCardModel model)
        {
            Genel.Handle(() =>
            {
                Genel.ReloadEntity();

                string soruTur = model.Tur.ToString();
                string soruAltTur = model.soru.AltTur.ToString();
                var topic = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == soruTur);

                // null kontrol (eğer db'de veri yoksa veriyi ekle)
                if (topic == null)
                {
                    Genel.dbEntities.TOPICS.Add(new TOPICS
                    {
                        TOPIC = soruTur,
                    });
                    Genel.dbEntities.SaveChanges();
                    topic = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == soruTur);
                }
                var subtopic = Genel.dbEntities.SUBTOPICS.FirstOrDefault(x => x.SUBTOPIC == soruAltTur);
                if (subtopic == null)
                {
                    Genel.dbEntities.SUBTOPICS.Add(new SUBTOPICS
                    {
                        SUBTOPIC = soruAltTur,
                        TOPICS = topic
                    });
                    Genel.dbEntities.SaveChanges();
                    subtopic = Genel.dbEntities.SUBTOPICS.FirstOrDefault(x => x.SUBTOPIC == soruAltTur);
                }

                //------cevaplar-----
                var answer = model.NesneSecenekler.DogruSecenekGetir();
                var wrongAnswersList = model.NesneSecenekler.secenekler.Where(x => x != answer).ToList();
                string wrongAnswers = "";
                //-------------------

                foreach (var item in wrongAnswersList)
                {
                    wrongAnswers += item + "#";
                }
                Genel.dbEntities.BOOKMARKED_QUESTIONS.Add(new BOOKMARKED_QUESTIONS
                {
                    USERID = aktifKullanici.kullnId,
                    TOPICS = topic,
                    SUBTOPICS = subtopic,
                    QUESTION_TEXT = seciliSoru.soru.IslemMetin,
                    LATEX_TEXT = seciliSoru.LaTeX,
                    CORRECT_ANSWER = answer,
                    WRONG_ANSWERS = wrongAnswers
                });
                Genel.dbEntities.SaveChanges();
            });
        }

        private void FeedbackToDB(SoruCardModel scm)
        {
            if (!string.IsNullOrEmpty(aktifKullanici.kullaniciAdi))
            {

                Genel.Handle(() =>
                {
                    Genel.ReloadEntity();
                    Genel.dbEntities.FEEDBACK.Add(new FEEDBACK
                    {
                        USERID = aktifKullanici.kullnId,
                        SUBJECT = "Hatalı soru bildirimi",
                        MESSAGE = $"SORU: {scm.soru.IslemMetin} SECENEKLER: {scm.NesneSecenekler.secenekler[0]}" +
                        $"#{scm.NesneSecenekler.secenekler[1]}" +
                        $"#{scm.NesneSecenekler.secenekler[2]}" +
                        $"#{scm.NesneSecenekler.secenekler[3]}" +
                        $"#{(scm.NesneSecenekler.secenekler.Count >= 5 ? scm.NesneSecenekler.secenekler[4] : "")}" +
                        $" DOGRU CEVAP: {scm.NesneSecenekler.DogruSecenekGetir()}",
                        FEEDBACK_TYPE = "Hata",
                        STATUS = "Beklemede",
                        CREATE_DATE = DateTime.Now,
                    });
                    Genel.dbEntities.SaveChanges();
                });
            }
        }

        private void DebugLimitSoruEkle()
        {

            Genel.Handle(() =>
            {
                seciliTur = "SoruModuNormal";

                Soru soru = SoruAjani.RastgeleLimitSorusuOlustur();

                seciliSoru = new SoruCardModel(soru)
                {
                    LaTeX = soru.LatexMetin,
                    zaman = DateTime.Now,
                    kaynak = "Yerel"
                };

                Sorular.Add(seciliSoru);

                OnPropertyChanged(nameof(secenekler));
                OnPropertyChanged();

            });
        }

        #endregion

        /// <summary>
        /// Sorunun içindeki radiobuttonlar buna bağlıdır içindeki değerleri object' kısmına gönderirler ve
        /// Soru modelinin içindeki dosyalar değişir
        /// </summary>
        /// <param name="nesne"> Secenek Radiobuttonun içibdeki Content/Değer </param>
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

        //Belge düzenini toplamak için stabil pushdan sonra bunun içine Relayleri toplayacağım
        #region relayCommands
        /// <summary>
        /// MVVM'de komutlar Relaylenerek işlenmesi gerekir lütfen içine komutunuzu koyunuz
        /// </summary>
        internal void KomutlarInit()
        {

            //Soru İçi Eylemler
            SoruYerIsaretiKaydet = new RelayCommand(o => tusDBSoruIsaretle(o));
            tusSoruMetaVeri = new RelayCommand(o => tusSoruMetaBilgi(o));
            SoruHataBildir = new RelayCommand(o => tusDBSoruBildir(o));

            //MVVM önemli komutlar
            SoruSec = new RelayCommand(o => SoruCardSec(o));
            PDFciktiAl = new RelayCommand(o => PDFciktiPencere());
            SoruCevapla = new RelayCommand(o => SeciliSoruCevapla(o));

            //Radiobuttonlar bu Komut'a bağlı
            SecimDegistir = new RelayCommand(o => SeceneklerSecimDegistir(o));
            HesapMakinesiGosterGizle = new RelayCommand(o => hesap.GosterGizle());

            //DEBUG
            DebugIslemEkleKomut = new RelayCommand(o => DebugIslemSoruEkle());
            SeciliTurDegistir = new RelayCommand(o => tusTurDegis());
            DebugCokluIfadeSil = new RelayCommand(o => DebugCokluIfadeCollSil(o));
            DebugFonksiyonSoruOlustur = new RelayCommand(o => FonksiyonSoruEkle());
            DebugCokluIfadeEkle = new RelayCommand(o => CokluIfadeListBoxEkle());
            DebugLatexsizPDFOlustur = new RelayCommand(o => PDFlatexsizCiktiAl());
            DebugLimitSoruOlustur = new RelayCommand(o => DebugLimitSoruEkle());
            AlgorithmaCheck = new RelayCommand(o => AlgorithmaChecki(o));
        }

        /// <summary>
        /// Debug Algorithma kapatma/açma tuşu
        /// </summary>
        /// <param name="o"></param>
        internal void AlgorithmaChecki(object o)
        {
            bool durum = algodurum;
            algodurum = !durum;

            MessageBox.Show(algodurum.ToString());
        }

        internal void tusDBSoruGetir()
        {

        }

        internal void tusDBSoruIsaretle(object o)
        {
            SoruCardModel _model = (SoruCardModel)o;


            SoruMetaVeri(_model, "Yer işaretlenen");

            if (!string.IsNullOrEmpty(aktifKullanici.kullaniciAdi))
            {
                BookmarkQuestionToDB(_model);
            }
        }

        internal void tusDBSoruBildir(object o)
        {
            SoruCardModel _model = (SoruCardModel)o;

            FeedbackToDB(_model);
            SoruMetaVeri(_model, "Bildirilen");
        }

        internal void tusSoruMetaBilgi(object o)
        {
            SoruCardModel _model = (SoruCardModel)o;

            SoruMetaVeri(_model);
        }

        private void SoruMetaVeri(SoruCardModel _model, string EK = "")
        {
            if (_model.kilitlendi || aktifKullanici.yetki == "admin")
            {
                MessageBox.Show($"Eylemdeki {EK} Model:: {_model.kaynak}\r SORU |\r - Türü {_model.soru.SoruTuru}\r - Alt Konusu :: {_model.soru.AltTur} \r{_model.LaTeX}\r Cevaplar\r - doğru :: {_model.NesneSecenekler.DogruSecenekGetir()} \r - seçilen :: {_model.NesneSecenekler.secilideger} \rRenk : {_model.TabRenk} \r ", "SoruBilgisi", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("MetaVeriyi görmeden önce soruyu çözmeniz veya admin olmanız gerekli", "Metaveri?", MessageBoxButton.OK, MessageBoxImage.Question);
            }
        }

        public void PDFlatexsizCiktiAl()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "PDF Dosyasını Kaydet",
                FileName = "Sorular.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string pdfPath = saveFileDialog.FileName;

                // Türkçe karakter desteği için BaseFont ve Font ayarla
                string arialPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont baseFont = BaseFont.CreateFont(arialPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var boldFont = new Font(baseFont, 12, Font.BOLD);
                var normalFont = new Font(baseFont, 11, Font.NORMAL);

                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
                document.Open();

                int soruNo = 1;
                var sorular = Sorular;
                // Cevap anahtarı için liste
                List<string> cevapAnahtari = new List<string>();

                if (sorular != null)
                {
                    foreach (var soru in sorular)
                    {
                        document.Add(new Paragraph($"Soru {soruNo}: \n {soru.soru.GetMetin()}", boldFont));

                        // Seçenekleri ekle ve doğru cevabın harfini bul
                        string dogruCevap = "";
                        if (soru.NesneSecenekler != null)
                        {
                            var secenekler = soru.NesneSecenekler.secenekler;
                            string dogruSecenek = soru.NesneSecenekler.DogruSecenekGetir();
                            for (int i = 0; i < secenekler.Count; i++)
                            {
                                char secenekHarf = (char)('A' + i);
                                document.Add(new Paragraph($"{secenekHarf}) {secenekler[i]}", normalFont));
                                if (secenekler[i] == dogruSecenek)
                                {
                                    dogruCevap = secenekHarf.ToString();
                                }
                            }
                        }
                        cevapAnahtari.Add($"Soru {soruNo}: {dogruCevap}");

                        document.Add(new Paragraph("\n", normalFont));
                        soruNo++;
                    }
                }

                // Cevap anahtarı başlığı
                document.Add(new Paragraph("Cevap Anahtarı", boldFont));
                document.Add(new Paragraph(" ", normalFont));
                foreach (var cevap in cevapAnahtari)
                {
                    document.Add(new Paragraph(cevap, normalFont));
                }

                document.Close();
                MessageBox.Show($"Sorular PDF dosyasına aktarıldı: {pdfPath}");
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

        public void PDFciktiPencere()
        {
            (new PDFbelgelendiriciWPF(Sorular)).ShowDialog();
        }

        #endregion

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
