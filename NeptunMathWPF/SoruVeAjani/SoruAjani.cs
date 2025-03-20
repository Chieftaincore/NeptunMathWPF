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
    //19.03.2025 Hüseyin
    //ifade türü kullanması için kısaltma
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
    
    //Soru terimlerine global erişim için Namespace konumu değiştirilebilir
    //Yeni Soru Üretici/Ajanı farklı yönlendirmeler yapabileceği için artık ajan olarak adlandırılacaktır
    //Bunlar lambdaya çevirilmesi lazım(try/catch'de kullanılan)
    public static class SoruAjani
    {
        //Deneysel Olusturucu sınırlı ve kötü. kaldırılacak
        public static Soru YerelSoruOlustur(ifadeTuru ifadeTur, int ifadesayisi = 4, int seviye = 0,int seceneksayisi = 4)
        {
            Random rng = new Random();
            List<ifade> ifadeler = new List<ifade>();

            string islemString = string.Empty;
            string LatexString = string.Empty;

            for (int i = 0; i < ifadesayisi; i++)
            {
                switch (ifadeTur)
                {
                    case ifadeTuru.sayi:
                        ifadeTamSayiUret(ifadeTuru.sayi, toplayan: ifadeler, rng.Next(0, 50));
                        break;
                    case ifadeTuru.kesir:

                        break;
                    case ifadeTuru.degisken:
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < ifadeler.Count; i++)
            {
                 if (i == 1)
                {
                    //Diziden rastgele karakter döndürür
                    char rngC = KarakterDondur(new char[] { '*', '-', '+', '/' });

                    //Bölme Tamsayı olması için farklı
                    if (rngC == '/')
                    {
                        int birinci = int.Parse(ifadeler[i].getir());
                        int tamsayicarp = birinci * rng.Next(2, 8);
                        islemString += $"{tamsayicarp} / {ifadeler[i].getir()}";
                        MessageBox.Show($"{tamsayicarp} / {ifadeler[i].getir()}");

                        islemString += KarakterDondur(new char[] { '*', '-', '+'});
                        i++;
                    }
                    else
                    {
                        islemString += ifadeler[i].getir();
                        islemString += rngC;
                    }
                }
                else
                {
                    islemString += ifadeler[i].getir();

                    if (i != ifadeler.Count - 1)
                    islemString += KarakterDondur(new char[] { '+', '-' });
                }
                
            }

            List<int> diger = new List<int>();

            Entity entity = islemString;
            Entity son = entity.EvalNumerical();

            MessageBox.Show($"{islemString}");
            MessageBox.Show(son.ToString());
            int sonuc = int.Parse(son.ToString());

            for (int i = 0; i < seceneksayisi - 1;)
            {
                int rand = sonuc + rng.Next(-30, 30);
                if (!diger.Contains(rand) && rand != sonuc)
                {
                    i++;
                    diger.Add(rand);
                }
            }
            return new Soru(islemString, sonuc, diger.ToArray());
        }

        
        //Yenileri ve Ayrılmış -----------------------------------------------------------------------------------------------
        internal static List<ifade> IfadeListesiOlustur(ifadeTuru ifadeTur, int ifadesayisi)
        {
            Random rng = new Random();
            List<ifade> ifadeler = new List<ifade>();

            for (int i = 0; i < ifadesayisi; i++)
            {
                switch (ifadeTur)
                {
                    case ifadeTuru.sayi:
                        ifadeTamSayiUret(ifadeTuru.sayi, toplayan: ifadeler, rng.Next(0, 50));
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

        
        //Temiz ve Daha iyi Oluşturucu
        public static Soru YerelSoruBirlestir(List<ifade> ifadeler, int seceneksayisi = 4, Action<String> araeleman=null)
        {
            string islemString = string.Empty;
            int sonuc = 0;
            bool oldu = false;
            List<int> diger = new List<int>();

            Genel.Handle(() =>
            {
                Random rng = new Random();
                for (int i=0; i < ifadeler.Count; i++)
                {
                    if(araeleman == null)
                    {
                        char dortislem;
                        if (i == 1)
                        {
                            dortislem = KarakterDondur(new char[] { '+', '-' , '*', '/' });
                            
                            //Tamsayı bölen oluşturmak için
                            if(dortislem == '/')
                            {
                                int bolen = ifadeler[i].parseGetir();
                                int bolunen = rng.Next(2,9) * bolen;

                                islemString += $"{bolunen}/{bolen}";
                            }
                        }
                        else
                        {
                            dortislem = KarakterDondur(new char[] { '*', '-', '+' });
                            islemString += ifadeler[i].getir(); 
                        }
                    }
                    else
                    {
                        //Devam edilecek kısım
                        //Özel dıştan parametreler ile araelemanlar
                        araeleman = (hiya) =>
                        {

                        };
                    }
                }

                Entity entity = islemString;
                Entity son = entity.EvalNumerical();

                MessageBox.Show($"{islemString}");
                MessageBox.Show(son.ToString());
                sonuc = int.Parse(son.ToString());


                for (int i = 0; i < seceneksayisi - 1;)
                {
                    int rand = sonuc + rng.Next(-30, 30);
                    if (!diger.Contains(rand) && rand != sonuc)
                    {
                        i++;
                        diger.Add(rand);
                    }
                }
                oldu = true;
            });

            if (oldu)
            {
                return new Soru(islem: islemString, sonuc, diger.ToArray());
            }
            else
            {
                throw new Exception();
            }
        }
        
        public static ifade ifadeTamSayiUret(ifadeTuru ifadeTur, List<ifade> toplayan, int sayi)
        {
            ifade ifadeNesne = new ifade(sayi.ToString(),sayi.ToString());
            toplayan.Add(ifadeNesne);

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
