using AngouriMath;
using AngouriMath.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static AngouriMath.MathS;
using static AngouriMath.MathS.Numbers;

namespace NeptunMathWPF.SoruVeAjani
{
    //ifadeler ve araişlemler için Nesneler
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
      
    public class Ifade
    {
        ifadeTuru Tur;
        
        internal string IslemString { get; set; }
        public virtual string LaTeXString { get; set; }

        public Ifade(string islem, string LaTeX, ifadeTuru tur)
        {
            IslemString = islem;
            LaTeXString = LaTeX;
            Tur = tur;
        }

        public Ifade(int Sayi, string LaTeX, ifadeTuru tur)
        {
            IslemString = Sayi.ToString();
            LaTeXString = LaTeX;
            Tur = tur;
        }

        public ifadeTuru TurGetir()
        {
            return Tur;
        }
        public virtual string getir()
        {
            return IslemString;
        }
        public int parseGetir()
        {
            return int.Parse(IslemString);
        }

    }
    public class Kesir : Ifade
    {

        public Kesir(int pay, int payda)  : base($"({pay}/{payda})", $"\\frac{{{pay}}}{{{payda}}}", ifadeTuru.kesir)
        {
            
        }
    }

    public class Faktoriyel : Ifade
    {
        Entity faq;

        public Faktoriyel(int sayi) : base(sayi, $"{sayi}!" ,tur: ifadeTuru.faktoriyel)
        {
            faq = Factorial(sayi);
        }

        public override string getir()
        {
            return faq.Stringize();
        }
    }

    public class Uslu : Ifade
    {
        public int temel { get; set; }

        public int kuvvet { get; set; }

        public Uslu(int _temel, int _kuvvet) : base ( islem: $" ({_temel}^{_kuvvet}) ",  LaTeX: $" {_temel}^{_kuvvet} ", tur: ifadeTuru.uslu)
        {
            temel = _temel;
            kuvvet = _kuvvet;
        }
    }

    //Yeni Deneme SoruBirleştiriciyle ilgili ilgili
    public class AraIslem
    {
        private string Islem;
        private string LaTeX;

        //Araişlem özel yapılandırma istiyorsa
        //Fonksiyonun önceki Ifadeye erişimi var ifadeye göre özel döndürebilir
        Func<Ifade, string> OzelYontem = null;

        public AraIslem()
        {

        }

        public AraIslem(string islem, string latex)
        {
            Islem = islem;
            LaTeX = latex;
        }

        public string IslemGetir()
        {
            return Islem;
        }

        public string OzelYapiCalistir(Ifade ifade)
        {
            return OzelYontem(ifade);
        }
        
        public bool OzelYapiGetir()
        {
            if(OzelYontem != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string LaTeXGetir()
        {
            return LaTeX;
        }
    }

    public class KarakterIslem : AraIslem
    {
        public KarakterIslem(Random rng) 
        {
            //char[] karakterler[];
        }
    }
}

