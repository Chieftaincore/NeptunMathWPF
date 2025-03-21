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
        string islemMetin;
     
        string sonuc;
        string LatexMetin;
        string OlusturmaLogu;

        int Seviye;
        ifade[] secenekler;

        //ifadeler bir soruya eklenecek ifadeleri temsil eder
        List<ifadeTuru> ifadeler;

        //Constructor baslatici sınıf'ile türe göre islem generatör, sql çekiş veya yapay zekaya yönlendirilecektir
        public Soru(string islem, string sonuc, int[] secenekler) {
            string mesaj = $"islem :: {islem} \n sonuc :: {sonuc} \n secenekler :: ";
            for(int i=0;i < secenekler.Length; i++)
            {
                mesaj += $"({secenekler[i]})";
            }
            MessageBox.Show(mesaj);

            islemMetin = islem;
            this.sonuc = sonuc;
        }

        internal void SetOlusturmaLogu(string Metin)
        {
            OlusturmaLogu = Metin;
        }

        public string GetOlusturmaLogu()
        {
            return OlusturmaLogu;
        }

        public int GetSeviye()
        {
            return Seviye;
        }
        public string GetMetin()
        {
            return islemMetin;
        }
        public string GetLaTex()
        {
            return LatexMetin;
        }
    }
}
