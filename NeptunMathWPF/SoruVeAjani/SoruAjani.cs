using AngouriMath;
using NeptunMathWPF.Fonksiyonlar;
using NeptunMathWPF.Formlar;
using NeptunMathWPF.SoruVeAjani.Limit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NeptunMathWPF.SoruVeAjani
{
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
    //Şuan da genel kullanım bir sistem hazırlanmaya çalışıyorum Bu yüzden nesne üstüne sistem kuracağım
    //Kullandığım Terimler
    //Parametre Aralık
    //İfade = Sayı/Kesir gibi
    //Arabirim = işlem veya matematik fonksiyonu
    //Hedef hem karma hem de tekli tür Soru Yapabilecek bir sistem
    public static class SoruAjani
    {
        //Tamsayı Random Atma Aralığı
        //Yeni PARAMETRE olarak fikir Uygulanabilir
        public static Dictionary<string, int[]> Araliklar = new Dictionary<string, int[]>
        {
            {"TAMSAYINORMAL", new int[] {2,50} }, {"TAMSAYIBOLME", new int[] {2,5} }, {"TAMSAYIYANILMA", new int[] {-30,30} },
            {"FAKTORIYELNORMAL",new int[]{2,6}}, {"KESIRTAMCARPAN",new int[] {2,15} }, {"FAKTORIYELSAYI", new int[] { 2, 6 } },
            {"USLUNORMAL",new int[]{1,12}}, {"USTAMCARPAN",new int[] {0,5} }, {"USBOLMECARPAN", new int[]{2,5}}
        };

        internal static Random random = new Random();
       
        internal static Soru RastgeleLimitSorusuOlustur()
        {
            LimitQuestion lim;

            Array questionTypes = Enum.GetValues(typeof(LimitQuestionType));
            LimitQuestionType currentQuestionType = (LimitQuestionType)questionTypes.GetValue(random.Next(questionTypes.Length));

            switch (currentQuestionType)
            {
                case LimitQuestionType.CommonFactor:
                    lim = LimitQuestionGenerator.GenerateCommonFactorQuestion();
                    break;
                case LimitQuestionType.FindCoefficients:
                    lim = LimitQuestionGenerator.GenerateFindCoefficientsQuestion();
                    break;
                case LimitQuestionType.CoefficientExpression:
                    lim = LimitQuestionGenerator.GenerateCoefficientExpressionQuestion();
                    break;
                default:
                    lim = LimitQuestionGenerator.GenerateCommonFactorQuestion();
                    break;
            };
            
            // Doğru cevap ve şaşırtmacalar
            List<string> options = new List<string>();

            // 3 tane farklı yanlış şık oluştur
            while (options.Count < 4)
            {
                double _wrong;
                string wrongOption;

                if (currentQuestionType == LimitQuestionType.CommonFactor)
                {
                    // Tam sayı şıklar oluştur (doğru cevaptan belirli uzaklıkta)
                    _wrong = lim.Answer + random.Next(-10, 11);
                    wrongOption = _wrong.ToString();
                }
                else
                {
                    // Katsayı sorularında daha küçük aralıkta şaşırtma cevaplar
                    _wrong = lim.Answer + random.Next(-5, 6);
                    wrongOption = _wrong.ToString();
                }

                // Aynı şık olmamasını sağla
                if (!options.Contains(wrongOption) && Math.Abs(_wrong - lim.Answer) > 0.001)
                {
                    options.Add(wrongOption);
                }
            }
            // Şıkları karıştır
            options = options.OrderBy(x => random.Next()).ToList();

            return new Soru(lim, options.ToArray())
            {
                
            };
        }




        
        internal static Soru RastgeleFonksiyonSorusuOlustur(int seceneksayisi = 4)
        {
            GeneratedFunction generated = new GeneratedFunction();
            List<string> Secenekler = new List<string>();
            Question question;

            Func<string> dongu;

            //Çoklu repository için hatırlatma
            //if (generated.HasMultipleFunctions == false)
            FunctionRepository rep = generated.repository[0];
            question = rep.questionObject;

            MessageBox.Show(question.Answer);

            switch (generated.gen)
            {
                //case FunctionValueGenerator f:

                //    break;
                //case FunctionCompositionGenerator f:

                //    break;
                //case InverseFunctionGenerator f:

                //    break;
                case DomainRangeGenerator f:
                    dongu = delegate ()
                    {
                        //ileride tanım kümesi yapıcı ve sonuç filtreleyici eklemeliyim  eklenmeli
                        string[] ciktilar = { "Tüm reel Sayılar", "Tanımsız", "Tüm reel sayılar, x ≠ -c", "Tüm reel sayılar, x ≠ c", "[1,+∞]",
                            "[-∞,1]", "Karmaşık Sayılar","[11,∞]","[9,∞]","[Reel Sayılar - 0]"};

                        string cikti = ciktilar[random.Next(0, ciktilar.Length - 1)];

                        if (!Secenekler.Contains(cikti))
                        {
                            return cikti;
                        }
                        else
                        {
                            //hatalı olunca boş gönderip tekrar başlatacak
                            return string.Empty;
                        }
                    };
                    break;
                default:

                    if (question.Answer != "∞")
                    {
                        dongu = delegate ()
                        {
                            double sonuc;
                            string cikti;
                            if (double.TryParse(question.Answer, out sonuc))
                            {
                                double rand = sonuc + random.Next(-10, 20);
                                cikti = rand.ToString();
                            }
                            else
                            {
                                double rand = random.Next(10, 30);
                                cikti = rand.ToString();
                            }

                            if (!Secenekler.Contains(cikti.ToString()))
                            {
                                return cikti.ToString();
                            }
                            else
                            {
                                //hatalı olunca boş gönderip tekrar başlatacak
                                return string.Empty;
                            }
                        };
                    }
                    else
                    {
                        //Sonsuz gelirse
                        dongu = delegate ()
                        {
                            double sonuc = random.Next(10, 20);
                            string cikti = sonuc.ToString();

                            return cikti;
                        };
                    }

                    break;
            }

            for (int i = 0; i < seceneksayisi;)
            {
                string cikti = dongu();

                if (cikti != string.Empty && cikti != question.Answer)
                {
                    Secenekler.Add(cikti);
                    i++;
                }
            }
            return new Soru(question, Secenekler.ToArray());
        }
        internal async static Task<Soru> ProblemSorusuOlustur()
        {
            //rastgele EnumType alması için
            int le = Enum.GetValues(typeof(ProblemType)).Length;
            int secilen = (new Random()).Next(0, le);

            var ProblemSoru = await ProblemGenerator.GenerateProblem((ProblemType)secilen, ProblemDifficulty.Zor);

            return new Soru(ProblemSoru);
        }
        
        #region IfadeListeleri
        internal static List<Ifade> IfadeListesiOlustur(ifadeTuru ifadeTur, int ifadesayisi)
        {
            Random rng = new Random();
            List<Ifade> ifadeler = new List<Ifade>();

            for (int i = 0; i < ifadesayisi; i++)
            {
                switch (ifadeTur)
                {
                    case ifadeTuru.sayi:
                        ifadeler.Add(IfadeTamSayiUret(rng.Next(Araliklar["TAMSAYINORMAL"][0], Araliklar["TAMSAYINORMAL"][1])));
                        break;
                    case ifadeTuru.faktoriyel:
                        ifadeler.Add(new Faktoriyel(rng.Next(Araliklar["FAKTORIYELSAYI"][0], Araliklar["FAKTORIYELSAYI"][1])));
                        break;
                    case ifadeTuru.kesir:
                        ifadeler.Add(new Kesir(rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1]),
                                               rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1])));
                        break;
                    case ifadeTuru.uslu:
                        ifadeler.Add(new Uslu(rng.Next(Araliklar["USLUNORMAL"][0], Araliklar["USLUNORMAL"][1]),
                                               rng.Next(Araliklar["USTAMCARPAN"][0], Araliklar["USTAMCARPAN"][1])));
                        break;
                    default:
                        MessageBox.Show("BELİRSİZ IFADE Liste'ye eklenmedi");
                        break;
                }
            }
            return ifadeler;
        }

        internal static List<Ifade> CokluIfadeListesiOlustur(List<ifadeTuru> olusturulacak)
        {
            Random rng = new Random();
            List<Ifade> ifadeler = new List<Ifade>();

            Genel.Handle(() => {

                for (int i = 0; i < olusturulacak.Count; i++)
                {
                    switch (olusturulacak[i])
                    {
                        case ifadeTuru.sayi:
                            ifadeler.Add(IfadeTamSayiUret(rng.Next(Araliklar["TAMSAYINORMAL"][0], Araliklar["TAMSAYINORMAL"][1])));
                            break;
                        case ifadeTuru.faktoriyel:
                            ifadeler.Add(new Faktoriyel(rng.Next(Araliklar["FAKTORIYELSAYI"][0], Araliklar["FAKTORIYELSAYI"][1])));
                            break;
                        case ifadeTuru.kesir:

                            int pay = rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1]);

                            int R = rng.Next(0, 1);

                            if (R == 1)
                            {
                                int payda = pay * rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1]);

                                ifadeler.Add(new Kesir(pay, payda));
                            }
                            else
                            {
                                int payda;

                                do
                                {
                                    payda = rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1]);

                                } while (payda == pay || payda == 0);

                                ifadeler.Add(new Kesir(pay,payda));
                            }

                            break;
                        case ifadeTuru.uslu:
                            ifadeler.Add(new Uslu(rng.Next(Araliklar["USLUNORMAL"][0], Araliklar["USLUNORMAL"][1]),
                                                  rng.Next(Araliklar["USTAMCARPAN"][0], Araliklar["USTAMCARPAN"][1])));
                            break;
                        default:
                            break;
                    }
                }
            });

            return ifadeler;
        }
        #endregion

        #region YerelIslemSorusuOlusturucular

        //2.05.25 COMMİTİ: bir sürü sorun çözdüm ama hala bazı sorunlar var.
        public static Soru YerelSoruBirlestir(List<Ifade> ifadeler, int seceneksayisi = 4, Action<String> araeleman = null)
        {
            string islemString = string.Empty;
            string ajanLOG = string.Empty;

            string latex = string.Empty;

            Entity sonuc = 0;
            List<Entity> diger = new List<Entity>();

            Genel.Handle(() =>
            {
                Collection<ifadeTuru> turler = new Collection<ifadeTuru>();

                Random rng = new Random();

                for (int i = 0; i < ifadeler.Count; i++)
                {
                    turler.Add(ifadeler[i].TurGetir());

                    if (araeleman == null)
                    {
                        if (i == 0)
                        {
                            char dortislem = KarakterDondur(new char[] { '+', '-', '*', '/' });

                            //Tamsayı bölen oluşturmak için
                            //Bölüm tamsayı çıkması için başka bir tamsayı ile çarpılır ve bolen bölünen olarak eklenir
                            if (dortislem == '/')
                            {
                                bool alindi = false;
                                if (ifadeler[i].TurGetir() == ifadeTuru.sayi)
                                {
                                    int bolen = ifadeler[i].parseGetir();
                                    int bolunen = rng.Next(Araliklar["TAMSAYIBOLME"][0], Araliklar["TAMSAYIBOLME"][1]) * bolen;

                                    islemString += $"{bolunen}/{bolen}";

                                    latex += $"\\frac{{{bolunen}}}{{{bolen}}}";
                                    ajanLOG += $"Bölünen EK eklendi :: {ifadeler[i].getir()} | {i + 1}'e EK | [{ifadeler[i + 1].getir()} kaldırıldı] \n";
                                }
                                else
                                {
                                    if (ifadeler[i].TurGetir() == ifadeTuru.kesir)
                                    {
                                        //MessageBox.Show("DEBUG : silindi ");
                                        islemString += ifadeler[i].getir() + '/';
                                        latex += ifadeler[i].LaTeXString + '/';

                                        ajanLOG += $"Kesir Eklendi :: {ifadeler[i].getir()}";
                                        alindi = true;
                                    }

                                    if (ifadeler[i].TurGetir() == ifadeTuru.faktoriyel)
                                    {
                                        islemString += ifadeler[i].getir() + " / ";
                                        latex += ifadeler[i].LaTeXString + "/";

                                        ajanLOG += $"Faktoriyel Eklendi :: {ifadeler[i].getir()}";
                                        alindi = true;
                                    }

                                    if (ifadeler[i].TurGetir() == ifadeTuru.uslu)
                                    {
                                        //Üslü sayılarda bazen uçuk sayılar çıkabiliyor tekrar gözden geçirilmelidir, şu ana kadar büyük yanlış sonuçlar çıkmadı

                                        int temel = ((Uslu)ifadeler[i]).temel;
                                        int kuvvet = ((Uslu)ifadeler[i]).kuvvet;

                                        int mode = rng.Next(0,1);
                                      
                                        int itemel;
                                        int ikuvvet;

                                        switch (mode)
                                        {
                                            case 0:

                                                itemel = temel;
                                                ikuvvet = Math.Abs(kuvvet - rng.Next( 1, kuvvet));

                                                islemString += $"({temel}^{kuvvet})/({itemel}^{ikuvvet})";
                                                latex += $"\\frac{{{temel}^{kuvvet}}}{{{itemel}^{ikuvvet}}}";

                                                break;
                                            case 1:

                                                ikuvvet = kuvvet;
                                                itemel = temel + rng.Next(-10, 12);

                                                islemString += $"({temel}^{kuvvet})/({itemel}^{ikuvvet})";
                                                latex += $"\\frac{{{temel}^{kuvvet}}}{{{itemel}^{ikuvvet}}}";

                                                break;
                                            case 2:
                                                //iyi bir formul bulunca ekleyeceğim
                                                break;
                                        }

                                        int r = rng.Next(0, 10);
                                    }
                                }

                                if (i < ifadeler.Count - 1 && !alindi)
                                {
                                    char ekleme = KarakterDondur(new char[] { '-', '+' });
                                    islemString += ekleme;
                                    latex += ekleme;
                                }

                                continue;
                            }
                            else
                            {
                                islemString += ifadeler[i].getir();
                                latex += ifadeler[i].LaTeXString;

                                islemString += dortislem;

                                if (dortislem == '*')
                                {
                                    latex += "\\cdot";
                                }
                                else
                                {
                                    latex += dortislem;
                                }

                                ajanLOG += $"Eklendi :: {ifadeler[i].getir()} | ifade ::  {i + 1} / {ifadeler.Count} \n";
                            }
                        }
                        else
                        {
                            char dortislem;
                            if (i < 3)
                            {
                                dortislem = KarakterDondur(new char[] { '*', '-', '+' });
                            }
                            else
                            {
                                dortislem = KarakterDondur(new char[] { '-', '+' });
                            }

                            ajanLOG += $"Eklendi :: {ifadeler[i].getir()} :: {i + 1} / {ifadeler.Count} \n";
                            islemString += ifadeler[i].getir();
                            latex += ifadeler[i].LaTeXString;

                            if (i != ifadeler.Count - 1)
                            {
                                islemString += dortislem;

                                if (dortislem == '*')
                                {
                                    latex += "\\cdot";
                                }
                                else
                                {
                                    latex += dortislem;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Devam edilecek kısım
                        //Özel dıştan parametreler ile araelemanlar
                        araeleman = (islem) =>
                        {
                            islemString = islem;
                        };
                    }
                }
                //Hesaplama Kısmı AngourioMath Kütüphanesi kullanılıyor
                Entity entity = islemString;
                sonuc = entity.EvalNumerical();

                ajanLOG += $"{islemString}\n";
                ajanLOG += $"{sonuc}\n";

                for (int i = 0; i < seceneksayisi - 1;)
                {
                    Entity randEntity;

                    if (!turler.Contains(ifadeTuru.kesir) && !(sonuc.Stringize()).Contains('/'))
                    {
                       
                        randEntity = sonuc + rng.Next(Araliklar["TAMSAYIYANILMA"][0], Araliklar["TAMSAYIYANILMA"][1]);
                    }
                    else
                    {
                        sonuc = sonuc.Simplify();

                        if ((sonuc.Stringize()).Contains('/'))
                        {
                           
                            int rastg = random.Next(-2, 4);

                            if (rastg != 0)
                            {
                                randEntity = sonuc + ((sonuc / 3) * rastg);
                            }
                            else
                            {
                                randEntity = sonuc - (sonuc / 2);
                            }
                        }
                        else
                        {
                            int r;

                            r = random.Next(20, 126);
                            randEntity = (sonuc + r);
                        }
                    }

                    randEntity = randEntity.EvalNumerical();

                    if (!diger.Contains(randEntity) && randEntity != sonuc)
                    {
                        i++;
                        diger.Add(randEntity);
                    }
                }
            });
            //Nesnenin Olusutğu AN;
            Soru soru = new Soru(islem: islemString, sonuc.ToString(), diger.ToArray());

            //PARAMETRELER ve Olusturucu LOGU
            ajanLOG += "PARAMETRELER \n";
            for (int i = 0; i < Araliklar.Count; i++)
            {
                ajanLOG += $"{Araliklar.ElementAt(i).Key} :: min({Araliklar.ElementAt(i).Value[0]}), max({Araliklar.ElementAt(i).Value[1]}) \n";
            }

            soru.SetLaTexMetin(latex);
            ajanLOG += $"LaTex ÇIKTI :: {latex}";
            soru.SetOlusturmaLogu(ajanLOG);
            return soru;
        }

        //YEDEK GELİŞTİRİLİYOR Yeni Deneme daha iyi yollar için arama denemesi
        //Henüz tuşlara bağlanmadı
        public static Soru YeniSoruBirlestir(List<Ifade> ifadeler, int seceneksayisi = 4, List<AraIslem> islemler = null)
        {
            string ajanLOG = string.Empty;

            string islemString = string.Empty;
            string LaTeXString = string.Empty;

            Entity sonuc = 0;
            List<Entity> diger = new List<Entity>();

            Genel.Handle(() =>
            {
                Random rng = new Random();
                for (int i = 0; i < ifadeler.Count; i++)
                {
                    if (i < islemler.Count)
                    {
                        if (!islemler[i].OzelYapiGetir())
                        {
                            islemString += ifadeler[i].getir();
                            ajanLOG += $"{ifadeler[i].TurGetir()} Eklendi :: {ifadeler[i].getir()}";

                            islemString += islemler[i].IslemGetir();
                            ajanLOG += $"işlem :: {islemler[i].IslemGetir()} eklendi \n";
                        }
                        else
                        {
                            string Ozel = islemler[i].OzelYapiCalistir(ifadeler[i]);
                            islemString += Ozel;
                            ajanLOG += $"işlem :: {islemler[i].IslemGetir()} eklendi \n";
                        }

                        LaTeXString += islemler[i].LaTeXGetir();
                    }
                    else
                    {
                        if (i < 4)
                        {
                            char ekislem = KarakterDondur(new char[] { '+', '-', '*' });

                            islemString += ifadeler[i].getir();
                            islemString += ekislem;

                            if (ekislem == '*')
                            {
                                LaTeXString += " \\cdot ";
                            }
                            else
                            {
                                LaTeXString += ekislem;
                            }
                        }
                    }
                }
                for (int i = 0; i < seceneksayisi - 1;)
                {
                    Entity randEntity;
                    int.TryParse(sonuc.Stringize(), out int s);

                    if ((s % 1) == 0)
                    {
                        randEntity = sonuc + rng.Next(Araliklar["TAMSAYIYANILMA"][0], Araliklar["TAMSAYIYANILMA"][1]);
                    }
                    else
                    {
                        sonuc = sonuc.Simplify();

                        int rastg = random.Next(-2, 2);
                        randEntity = sonuc + ((sonuc / 3) * rastg);
                    }
                    randEntity = randEntity.EvalNumerical();

                    if (!diger.Contains(randEntity) && randEntity != sonuc)
                    {
                        i++;
                        diger.Add(randEntity);
                    }
                }
            });

            Entity entity = islemString;
            sonuc = entity.EvalNumerical();

            ajanLOG += $"{islemString}\n";
            ajanLOG += $"{sonuc}\n";

            Soru soru = new Soru(islem: islemString, sonuc.ToString(), diger.ToArray());
            soru.SetLaTexMetin(LaTeXString);

            return soru;
        }
        #endregion

        //Ifade oluştruma yöntemleri
        #region Ifadeler 
        public static Ifade IfadeKesirSayiUret(int pay)
        {
            int payda = pay * random.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1]);
            string LaTex = $"\\frac{{{pay}}}{{{payda}}}";
            string islem = $"({pay}/{payda})";

            return new Ifade(islem, LaTex, ifadeTuru.kesir);
        }

        public static Ifade IfadeTamSayiUret(int sayi)
        {
            Ifade ifadeNesne = new Ifade(sayi.ToString(), sayi.ToString(), ifadeTuru.sayi);

            return ifadeNesne;
        }

        #endregion 

        //Seçilelerden Rastgele char döndür
        public static char KarakterDondur(char[] charlar)
        {
            Random rng = new Random();

            return charlar[rng.Next(0, charlar.Length)];
        }
    }

    //Nesneler okuma koyaylığı için farklı bir Dosya'yw aktarılmışdır -Hüseyin
    //Nesneler SoruVeAjanı->Ifade.cs' içindedir
}


