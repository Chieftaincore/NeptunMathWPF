using AngouriMath;
using AngouriMath.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static AngouriMath.MathS;
using static AngouriMath.MathS.Numbers;

namespace NeptunMathWPF.SoruVeAjani
{
    //ifadeler ve araişlemler için Nesneler
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
      
    public class Ifade
    {
        ifadeTuru Tur;
        string islemS;
        string LaTeXS;

        public Ifade()
        {

        }

        public Ifade(string islem, string LaTeX, ifadeTuru tur)
        {
            islemS = islem;
            LaTeXS = LaTeX;
            Tur = tur;
        }

        public Ifade(int Sayi)
        {
            islemS = Sayi.ToString();
            LaTeXS = Sayi.ToString();
            Tur = ifadeTuru.sayi;
        }

        public Ifade(int Sayi, ifadeTuru tur)
        {
            islemS = Sayi.ToString();
            LaTeXS = Sayi.ToString();
            Tur = tur;
        }
        public string LaTeXgetir()
        {
            return LaTeXS;
        }
        public ifadeTuru TurGetir()
        {
            return Tur;
        }
        public virtual string getir()
        {
            return islemS;
        }

        public int parseGetir()
        {
            return int.Parse(islemS);
        }

    }
    public class Kesir : Ifade
    {
        Entity pay;
        Entity payda;
        string islem;
        string LaTex;


        public Kesir(int pay, int payda)
        {
            string islemS = $"({pay}/{payda})";
            LaTex = $"frac({{{pay}}}, {{{payda}}})";
            islem = islemS;  
        }
    }

    public class Faktoriyel : Ifade
    {
        Entity faq;

        public Faktoriyel(int sayi) : base(sayi, tur: ifadeTuru.faktoriyel)
        {
            faq = Factorial(sayi);
        }

        public override string getir()
        {
            return faq.Stringize();
        }
    }

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

