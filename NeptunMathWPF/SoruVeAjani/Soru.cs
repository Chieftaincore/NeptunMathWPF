using AngouriMath;
using AngouriMath.Extensions;
using NeptunMathWPF.Fonksiyonlar;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;

namespace NeptunMathWPF
{

    using ifadeTuru = SoruTerimleri.ifadeTurleri;
    public class Soru
    {
        string islemMetin;
     
        string sonuc;
        string LatexMetin;
        string OlusturmaLogu;

        int Seviye;
        List<string> secenekler = new List<string>();

        //Constructor baslatici sınıf'ile türe göre islem generatör, sql çekiş veya yapay zekaya yönlendirilecektir
      
        //İşlem Sorusu
        public Soru(string islem, string sonuc, Entity[] secenekler) {
           
            string mesaj = $"islem :: {islem} \n sonuc :: {sonuc} \n secenekler :: ";
            for(int i=0;i < secenekler.Length; i++)
            {
                mesaj += $"({secenekler[i]})";
             
            }
            MessageBox.Show(mesaj);

            foreach (Entity e in secenekler)
            {
                this.secenekler.Add(e.Stringize());
            }
            islemMetin = islem;
            this.sonuc = sonuc;
        }

        //Fonksiyon Sorusu
        public Soru(Question question, string[] secenekler)
        {
            islemMetin = question.QuestionText;
            LatexMetin = question.QuestionText;

            this.secenekler = secenekler.ToList();
            sonuc = question.Answer;

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
            return secenekler.ToArray();
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
