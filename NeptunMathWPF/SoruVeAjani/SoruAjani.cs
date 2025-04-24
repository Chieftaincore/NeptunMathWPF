using AngouriMath;
using AngouriMath.Extensions;
using HonkSharp.Functional;
using NeptunMathWPF.Fonksiyonlar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using static AngouriMath.MathS;
using static AngouriMath.MathS.Numbers;

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
            {"USLUNORMAL",new int[]{1,12}}, {"USTAMCARPAN",new int[] {0,5} }
        };

        //Fonksiyon oluşturucu Generatörleri
        public static List<FunctionQuestionGenerator> generators = new List<FunctionQuestionGenerator>
        {
            new FunctionValueGenerator(),
            new FunctionCompositionGenerator(),
            new InverseFunctionGenerator(),
            new DomainRangeGenerator()
        };

        internal static Random random = new Random();

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
                        ifadeler.Add(IfadeFaktoriyelUret(rng.Next(Araliklar["FAKTORIYELSAYI"][0], Araliklar["FAKTORIYELSAYI"][1])));
                        break;
                    case ifadeTuru.kesir:
                        ifadeler.Add(IfadeKesirSayiUret(rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1])));
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

            for (int i = 0; i < olusturulacak.Count; i++)
            {
                switch (olusturulacak[i])
                {
                    case ifadeTuru.sayi:
                        ifadeler.Add(IfadeTamSayiUret(rng.Next(Araliklar["TAMSAYINORMAL"][0], Araliklar["TAMSAYINORMAL"][1])));
                        break;
                    case ifadeTuru.faktoriyel:
                        ifadeler.Add(IfadeFaktoriyelUret(rng.Next(Araliklar["FAKTORIYELSAYI"][0], Araliklar["FAKTORIYELSAYI"][1])));
                        break;
                    case ifadeTuru.kesir:
                        ifadeler.Add(IfadeKesirSayiUret(rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1])));
                        break;
                    default:
                        break;
                }
            }

            return ifadeler;
        }

        //Fonksiyon sorusunu çoklu kullanıma dönüştürme ve secenekler eklemek için
        //Birden fazla metota ayırabilirim -Hüseyin

        //Burayı şimdilik yorum satırına aldım, kodları yenilediğim için düzeltilmesi gerekiyor.
        /*
        internal static Soru RastgeleFonksiyonSorusuOlustur(int seceneksayisi=4)
        {
            FunctionQuestionGenerator Secili = generators[new Random().Next(generators.Count)];
            Question question = Secili.GenerateQuestion();

            List<string> Secenekler = new List<string>();
            Func<string> dongu;

            MessageBox.Show(question.Answer);

            switch (Secili)
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
                        //Uydurunca ben -Hüseyin
                        string[] ciktilar = { "Tüm Reel Sayılar", "Tanımsız", "Tüm reel sayılar, x ≠ -c", "Tüm reel sayılar, x ≠ c", "[1,+∞]",
                            "[-∞,1]", "Karmaşık Sayılar","[11,∞]","[9,∞]"};

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
                    break;
            }

            for (int i=0; i<seceneksayisi;)
            {
                string cikti = dongu();

                if(cikti != string.Empty)
                {
                    Secenekler.Add(cikti);
                    i++;
                }
            }
            return new Soru(question, Secenekler.ToArray());
        }
        */

        //Bunlarla Neyi Hedefliyorum 
        //Temiz ve Daha iyi Oluşturucu || Daha iyisi YAKINDA™

        //Aynı zamanda karma soru yapıcı yapmalıyız (aynı soruda Kesir + Faktoriyel + Tamsayı) gibi farklı tür ifadeler aynı soruda yer alabilmeli
        //Daha iyisi için yollar var yakında yapılabileceğini ümit ediyorum -Hüseyin

        public static Soru YerelSoruBirlestir(List<Ifade> ifadeler, int seceneksayisi = 4, Action<String> araeleman = null)
        {
            string ajanLOG = string.Empty;

            string islemString = string.Empty;
            Entity sonuc = 0;
            List<Entity> diger = new List<Entity>();

            Genel.Handle(() =>
            {
                ifadeTuru Tur = ifadeler[0].TurGetir();

                Random rng = new Random();
                for (int i = 0; i < ifadeler.Count; i++)
                {

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
                                    ajanLOG += $"Bölünen EK eklendi :: {ifadeler[i].getir()} | {i + 1}'e EK | [{ifadeler[i + 1].getir()} kaldırıldı] \n";
                                }
                                else
                                {
                                    if (ifadeler[i].TurGetir() == ifadeTuru.kesir)
                                    {
                                        MessageBox.Show("DEBUG : silindi ");
                                        islemString += ifadeler[i].getir() + '/';
                                        ajanLOG += $"Kesir Eklendi :: {ifadeler[i].getir()}";
                                        alindi = true;
                                    }

                                    if (ifadeler[i].TurGetir() == ifadeTuru.faktoriyel)
                                    {
                                        islemString += ifadeler[i].getir();
                                        ajanLOG += $"Faktoriyel Eklendi :: {ifadeler[i].getir()}";
                                    }
                                }

                                if (i < ifadeler.Count - 1 && !alindi)
                                {
                                    islemString += KarakterDondur(new char[] { '-', '+' });
                                }
                                continue;
                            }
                            else
                            {
                                islemString += ifadeler[i].getir();
                                islemString += dortislem;

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

                            if (i != ifadeler.Count - 1)
                            {
                                islemString += dortislem;
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

                    if (Tur == ifadeTuru.sayi)
                    {
                        randEntity = sonuc + rng.Next(Araliklar["TAMSAYIYANILMA"][0], Araliklar["TAMSAYIYANILMA"][1]);
                    }
                    else
                    {
                        sonuc = sonuc.Simplify();

                        if (Tur == ifadeTuru.kesir)
                        {
                            int rastg = random.Next(-2, 2);
                            randEntity = sonuc + ((sonuc / 3) * rastg);
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

            //NOT: Bazı LaTeX'leri doğru çeviremiyor 
            //Ornek = 12+22*6! Çarpmayı' yokediyor ya bir çözüm ya da daha iyi bir LaTeX convertor bulacağız
            //Sorun Çoklu Ifade' üçüncü çarpmaya gelince görülüyor
            string LaTeX = islemString.Latexise();
            soru.SetLaTexMetin(LaTeX);
            ajanLOG += $"LaTex ÇIKTI :: {LaTeX}";
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
                        if(i < 4)
                        {
                            char ekislem = KarakterDondur(new char[] { '+', '-', '*' });

                            islemString += ifadeler[i].getir();
                            islemString += ekislem;

                            if(ekislem == '*')
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

        public static Ifade IfadeKesirSayiUret(int pay)
        {
            int payda = pay * random.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1]);
            string LaTex = $"frac({{{pay}}}, {{{payda}}})";
            string islem = $"({pay}/{payda})";
            Ifade ifadeNesne = new Ifade(islem, LaTex, ifadeTuru.kesir);
            return ifadeNesne;
        }

        public static Ifade IfadeFaktoriyelUret(int sayi)
        {
            return new Faktoriyel(sayi);
        }

        public static Ifade IfadeTamSayiUret(int sayi)
        {
            Ifade ifadeNesne = new Ifade(sayi.ToString(), sayi.ToString(), ifadeTuru.sayi);

            return ifadeNesne;
        }

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

    
