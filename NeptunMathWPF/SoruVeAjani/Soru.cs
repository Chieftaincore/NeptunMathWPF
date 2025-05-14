using AngouriMath;
using AngouriMath.Extensions;
using NeptunMathWPF.Fonksiyonlar;
using NeptunMathWPF.SoruVeAjani;
using NeptunMathWPF.SoruVeAjani.Limit;
using NeptunMathWPF.SoruVeAjani.Problemler;
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

    using soruTuru = SoruTerimleri.soruTuru;
    public class Soru
    {
        public soruTuru SoruTuru { get; }
        public Enum AltTur { get; set; }
        public string IslemMetin { get; set; }
        public string LatexMetin { get; set; }

        string Sonuc;

        string OlusturmaLogu;

        int Seviye;

        List<string> secenekler = new List<string>();

        //Constructor baslatici sınıf'ile türe göre islem generatör, sql çekiş veya yapay zekaya yönlendirilecektir

        //İşlem Sorusu
        public Soru(string islem, string sonuc, Entity[] secenekler)
        {

            SoruTuru = soruTuru.islem;

            //Loglama Debug Mesajı
            string mesaj = $"islem :: {islem} \n sonuc :: {sonuc} \n secenekler :: ";

            for (int i = 0; i < secenekler.Length; i++)
            {
                mesaj += $"({secenekler[i]})";
            }
            //MessageBox.Show(mesaj);

            //secenekleri listede toplama
            foreach (Entity e in secenekler)
            {
                this.secenekler.Add(e.Stringize());
            }

            IslemMetin = islem;
            this.Sonuc = sonuc;
        }

        public Soru(LimitQuestion _limit, string[] _secenekler)
        {
            SoruTuru = soruTuru.limit;

            LatexMetin = _limit.QuestionLaTeX;
            IslemMetin = _limit.QuestionText;
            Sonuc = _limit.Answer.ToString();
            secenekler = _secenekler.ToList();
        }

        //Fonksiyon Sorusu
        public Soru(Question question, string[] secenekler)
        {
            SoruTuru = soruTuru.fonksiyon;

            IslemMetin = question.QuestionText;
            LatexMetin = question.LatexText;

            this.secenekler = secenekler.ToList();
            Sonuc = question.Answer;
        }

        //API/LLM'den gelen Problem Sorusu
        public Soru(ProblemRepository rep)
        {
            SoruTuru = soruTuru.problem;

            IslemMetin = rep.problem;
            LatexMetin = rep.problem;

            Sonuc = rep.correctAnswer;
            secenekler = rep.incorrectAnswers;
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
            return Sonuc;
        }
        public string GetOlusturmaLogu()
        {
            return OlusturmaLogu;
        }
        public string GetMetin()
        {
            return IslemMetin;
        }
        public string GetLaTex()
        {
            return LatexMetin;
        }
    }
}
