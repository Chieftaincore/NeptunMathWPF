using AngouriMath;
using HonkSharp.Functional;
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
            {"FAKTORIYELNORMAL",new int[]{2,6}}, {"KESIRTAMCARPAN",new int[] {2,15}}
        };
        internal static Random random = new Random(); 

        //Yenileri Versiyon
        internal static List<ifade> IfadeListesiOlustur(ifadeTuru ifadeTur, int ifadesayisi)
        {
            Random rng = new Random();
            List<ifade> ifadeler = new List<ifade>();

            for (int i = 0; i < ifadesayisi; i++)
            {
                switch (ifadeTur)
                {
                    case ifadeTuru.sayi:
                        ifadeler.Add(IfadeTamSayiUret(rng.Next(Araliklar["TAMSAYINORMAL"][0], Araliklar["TAMSAYINORMAL"][1])));
                        break;
                    case ifadeTuru.faktoriyel:

                        break;
                    case ifadeTuru.kesir:
                        ifadeler.Add(IfadeKesirSayiUret(rng.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1])));
                        break;
                    case ifadeTuru.degisken:
                        break;
                    default:
                        break;
                }
            }
            return ifadeler;
        }

        internal static List<ifade> CokluIfadeListesiOlustur(List<ifadeTuru>olusturulacak)
        {
            Random rng = new Random();
            List<ifade> ifadeler = new List<ifade>();

            for (int i = 0; i < olusturulacak.Count; i++)
            {
                switch (olusturulacak[i])
                {
                    case ifadeTuru.sayi:
                        ifadeler.Add(IfadeTamSayiUret(rng.Next(Araliklar["TAMSAYINORMAL"][0], Araliklar["TAMSAYINORMAL"][1])));
                        break;
                    case ifadeTuru.faktoriyel:
                        break;
                    case ifadeTuru.kesir:

                        break;
                    case ifadeTuru.degisken:
                        break;
                    default:
                        break;
                }
            }

            return ifadeler;
        }

        //Temiz ve Daha iyi Oluşturucu || Daha iyisi YAKINDA™
        public static Soru YerelSoruBirlestir(List<ifade> ifadeler, int seceneksayisi = 4, Action<String> araeleman = null)
        {
            string ajanLOG = string.Empty;

            string islemString = string.Empty;
            int sonuc = 0;
            List<int> diger = new List<int>();

            Genel.Handle(() =>
            {
                Random rng = new Random();
                for (int i=0; i < ifadeler.Count; i++)
                {
                    if(araeleman == null)
                    {
                        if (i == 0)
                        {
                            char dortislem = KarakterDondur(new char[] { '+', '-' , '*', '/' });
                            
                            //Tamsayı bölen oluşturmak için
                            //Bölüm tamsayı çıkması için başka bir tamsayı ile çarpılır ve bolen bölünen olarak eklenir
                            if(dortislem == '/')
                            {
                                int bolen = ifadeler[i].parseGetir();
                                int bolunen = rng.Next(Araliklar["TAMSAYIBOLME"][0], Araliklar["TAMSAYIBOLME"][1]) * bolen;

                                islemString += $"{bolunen}/{bolen}";
                                ajanLOG += $"Bölünen EK eklendi :: {ifadeler[i].getir()} | {i + 1}'e EK | [{ifadeler[i+1].getir()} kaldırıldı] \n";
                                i++;
                                if (i < ifadeler.Count - 1) {
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
                Entity son = entity.EvalNumerical();

                ajanLOG += $"{islemString}\n";
                ajanLOG += $"{son.ToString()}\n";
                sonuc = int.Parse(son.ToString());
                
   

                for (int i = 0; i < seceneksayisi - 1;)
                {
                    int rand = sonuc + rng.Next(Araliklar["TAMSAYIYANILMA"][0], Araliklar["TAMSAYIYANILMA"][1]);
                    if (!diger.Contains(rand) && rand != sonuc)
                    {
                        i++;
                        diger.Add(rand);
                    }
                }
            });
            //Nesnenin Olusutğu AN;
            Soru soru=new Soru(islem: islemString, sonuc.ToString(), diger.ToArray());

            //PARAMETRELER ve Olusturucu LOGU
            ajanLOG += "PARAMETRELER \n";
            for (int i = 0; i < Araliklar.Count; i++)
            {
                ajanLOG += $"{Araliklar.ElementAt(i).Key} :: min({Araliklar.ElementAt(i).Value[0]}), max({Araliklar.ElementAt(i).Value[1]}) \n";
            }
            soru.SetOlusturmaLogu(ajanLOG);
            
            return soru;
        }

        public static ifade IfadeKesirSayiUret(int pay)
        {
            int payda = pay * random.Next(Araliklar["KESIRTAMCARPAN"][0], Araliklar["KESIRTAMCARPAN"][1]);
            string LaTex = $"frac({{{pay}}}, {{{payda}}})";
            MessageBox.Show(LaTex);
            string islem = $"({pay}/{payda})";
            ifade ifadeNesne = new ifade(islem, LaTex);
            return ifadeNesne;
        }

        public static ifade IfadeTamSayiUret(int sayi)
        {
            ifade ifadeNesne = new ifade(sayi.ToString(),sayi.ToString());
            return ifadeNesne;
        }
        
        //Seçilelerden Rastgele char döndür
        public static char KarakterDondur(char[] charlar)
        {
            Random rng = new Random();

            return charlar[rng.Next(0, charlar.Length)];
        }
    }

    public class ifade
    {
        string islemS;
        string LaTeXS;

        public ifade(string islem, string LaTeX)
        {
            islemS = islem;
            LaTeXS = LaTeX;
        }

        public ifade(int Sayi)
        {
            islemS = Sayi.ToString();
            LaTeXS = Sayi.ToString();
        }
        public string LaTeXgetir()
        {
            return LaTeXS;
        }

        public string getir()
        {
            return islemS;
        }

        public int parseGetir()
        {
            return int.Parse(islemS);
        }
    }
}
