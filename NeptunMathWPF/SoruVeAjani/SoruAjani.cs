using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        //Yeterli kısma gelince SoruAjanı nesneyi tamamen oluşturacak hale gelmeli
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

                        ifadeSayiUret(ifadeTuru.sayi, toplayan: ifadeler, rng.Next(0, 50));
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
                    char rngC = KarakterDondur(new char[] { '*', '-', '+' });


                    if (rngC == '/')
                    {
                        //DT.Compute bölme işlemini Hatalı sonuç çıkarıyor.

                        //int birinci = int.Parse(ifadeler[i].getir());
                        //int tamsayicarp = birinci * rng.Next(2, 8);
                        //islemString += $"{tamsayicarp} / {ifadeler[i].getir()}";
                        //int a = tamsayicarp / int.Parse(ifadeler[i].getir());
                        //MessageBox.Show(a.ToString());
                        //MessageBox.Show($"{tamsayicarp} / {ifadeler[i].getir()}");
                        //i++;
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
            DataTable dt = new DataTable();
            object son;
     
            son = dt.Compute(islemString,"");
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

        internal static ifade ifadeSayiUret(ifadeTuru ifadeTur, List<ifade> toplayan, Action<int> INTaksiyonu = null)
        {
            ifade ifadeNesne = new ifade(INTaksiyonu.ToString(), INTaksiyonu.ToString());
            toplayan.Add(ifadeNesne);

            return ifadeNesne;
        }
        internal static ifade ifadeSayiUret(ifadeTuru ifadeTur, List<ifade> toplayan, int sayi)
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

    class ifade
    {
        string islemS;
        string LaTeXS;

        public ifade(string islem, string LaTeX)
        {
            islemS = islem;
            LaTeXS = LaTeX;
        }

        public string getir()
        {
            return islemS;
        }
    }
}
