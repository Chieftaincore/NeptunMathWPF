using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF
{
    //HES olusturdu
    //Sorular soruların iceriği
    //Soruların Nesne hali, önerisi olan yazsın
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
    public class Soru
    {
        string Metin;
        string LatexMetin;
        int Seviye;

        //ifadeler bir soruya eklenecek ifadeleri temsil eder
        List<ifadeTuru> ifadeler;

        //Constructor baslatici sınıf'ile türe göre islem generatör, sql çekiş veya yapay zekaya yönlendirilecektir
        public Soru(ifadeTuru tur, int soruseceneksayisi=3 ,int seviye=0)
        {
            //Aşağıdaki farklı tür soruları nasıl aynı nesneyle yapabileceğimizin örneğidir.
            Seviye = seviye;

            //SoruAjani.YerelSoruOlustur(this,ifadeTuru.sayi);
        }

        public Soru(string islem, int cozum, int[] secenekler) {
            string mesaj = $"islem :: {islem} \n sonuc :: {cozum} \n secenekler :: ";
            for(int i=0;i < secenekler.Length; i++)
            {
                mesaj += $"({secenekler[i]})";
            }
            MessageBox.Show(mesaj);
        }

        public int GetSeviye()
        {
            return Seviye;
        }
        public string GetMetin()
        {
            return Metin;
        }
    }
}
