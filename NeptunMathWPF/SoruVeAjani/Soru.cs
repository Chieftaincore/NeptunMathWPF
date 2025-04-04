using AngouriMath;
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
        Entity[] secenekler;

        //Constructor baslatici sınıf'ile türe göre islem generatör, sql çekiş veya yapay zekaya yönlendirilecektir
        public Soru(string islem, string sonuc, Entity[] secenekler) {
            string mesaj = $"islem :: {islem} \n sonuc :: {sonuc} \n secenekler :: ";
            for(int i=0;i < secenekler.Length; i++)
            {
                mesaj += $"({secenekler[i]})";
            }
            MessageBox.Show(mesaj);

            islemMetin = islem;
            this.sonuc = sonuc;
            this.secenekler = secenekler;
        }

        internal void SetOlusturmaLogu(string Metin)
        {
            OlusturmaLogu = Metin;
        }
        internal void SetLaTexMetin(string LaTex)
        {
            LatexMetin = LaTex;
        }

        public string[] GetDigerSecenekler()
        {
            List<String> Cikti = new List<string>();

            for(int i = 0; i < secenekler.Length; i++)
            {
                Cikti.Add(secenekler[i].Stringize());
            }

            return Cikti.ToArray();
        }
        public string GetSonucSecenek()
        {
            return sonuc;
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
