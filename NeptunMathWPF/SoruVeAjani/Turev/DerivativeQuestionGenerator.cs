using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeptunMathWPF.SoruVeAjani.Turev.Models;

namespace NeptunMathWPF.SoruVeAjani.Turev.Services
    //.Sevices çünkü bendeki böyle
    // Nedense soru tiplerini göremiyor.
{
    public class DerivativeQuestionGenerator
    {
        private static Random random = new Random();

        private static string WrapLatexQuestion(string questionContent)
        {
            return questionContent; // Basit bir wrapper, gerekiyorsa LaTeX ön işleme eklenebilir
        }

        private static string FormatCoefficient(int coef, bool isFirst = false)
        {
            if (coef == 0) return "";
            if (coef == 1) return isFirst ? "" : "+";
            if (coef == -1) return "-";
            return coef > 0 ? (isFirst ? $"{coef}" : $"+{coef}") : $"{coef}";
        }
        // Temel fonksiyonların türevini hesaplama
        public static DerivativeQuestion GenerateBasicDerivativeQuestion()
        {
            // Katsayıları belirle (-5 ile 8 aralığında)
            int a = random.Next(-5, 9); // x² katsayısı
            int b = random.Next(-5, 9); // x katsayısı
            int c = random.Next(-10, 11); // sabit terim

            // Sıfır katsayısı olmasını engelle
            if (a == 0) a = random.Next(1, 6);

            // Soruda hangisini soracağız?
            int questionVariant = random.Next(2);
            string questionText, latexString, explanation;
            double answer;

            if (questionVariant == 0)
            {
                // f(x) = ax² + bx + c fonksiyonunun x=0'da türevi
                answer = b; // x=0 için türev değeri = b

                questionText = $"f(x) = {a}x² + {b}x + {c} fonksiyonunun x = 0 noktasındaki türevi kaçtır?";

                // LaTeX formatı
                string aCoef = FormatCoefficient(a, true);
                string bCoef = FormatCoefficient(b);
                string cCoef = FormatCoefficient(c);

                latexString = $"f(x) = {aCoef}{(a == 1 || a == -1 ? "" : "")}{(a == 0 ? "" : "x^2")} {bCoef}{(b == 1 || b == -1 ? "" : "")}{(b == 0 ? "" : "x")} {cCoef} \\text{{ için }} f'(0) = \\, ?";

                explanation = $"f(x) = {a}x² {(b >= 0 ? "+" : "")}{b}x {(c >= 0 ? "+" : "")}{c} fonksiyonunun türevi:\n" +
                              $"f'(x) = {2 * a}x {(b >= 0 ? "+" : "")}{b}\n" +
                              $"x = 0 için türev değeri:\n" +
                              $"f'(0) = {2 * a}·0 {(b >= 0 ? "+" : "")}{b} = {b}";
            }
            else
            {
                // Türev fonksiyonunda x'in katsayısı
                answer = 2 * a; // türevde x katsayısı = 2a

                questionText = $"f(x) = {a}x² + {b}x + {c} fonksiyonunun türevinde x'in katsayısı kaçtır?";

                // LaTeX formatı
                string aCoef = FormatCoefficient(a, true);
                string bCoef = FormatCoefficient(b);
                string cCoef = FormatCoefficient(c);

                latexString = $"f(x) = {aCoef}{(a == 1 || a == -1 ? "" : "")}{(a == 0 ? "" : "x^2")} {bCoef}{(b == 1 || b == -1 ? "" : "")}{(b == 0 ? "" : "x")} {cCoef} \\text{{ için }} f'(x) = kx + n \\text{{ ise }} k = \\, ?";

                explanation = $"f(x) = {a}x² {(b >= 0 ? "+" : "")}{b}x {(c >= 0 ? "+" : "")}{c} fonksiyonunun türevi:\n" +
                              $"f'(x) = {2 * a}x {(b >= 0 ? "+" : "")}{b}\n" +
                              $"Türev fonksiyonunda x'in katsayısı {2 * a}'dir.";
            }

            return new DerivativeQuestion
            {
                QuestionText = questionText,
                QuestionLaTeX = WrapLatexQuestion(latexString),
                Answer = answer,
                ExplanationText = explanation
            };
        }
        // Belirli bir noktada türev değeri hesaplama
        public static DerivativeQuestion GeneratePointDerivativeQuestion()
        {
            // Katsayıları belirle (-5 ile 5 aralığında)
            int a = random.Next(-5, 6); // x³ katsayısı
            int b = random.Next(-5, 6); // x² katsayısı
            int c = random.Next(-5, 6); // x katsayısı

            // Sıfır olmaması için en az bir katsayı zorla
            if (a == 0 && b == 0) c = random.Next(1, 6);
            if (a == 0) b = random.Next(1, 6);

            // x noktasını belirle (-3 ile 3 aralığında)
            int x0 = random.Next(-3, 4);

            // Türev: f'(x) = 3ax² + 2bx + c
            // x0 noktasında: f'(x0) = 3a(x0)² + 2b(x0) + c
            double answer = 3 * a * x0 * x0 + 2 * b * x0 + c;

            string questionText = $"f(x) = {a}x³ + {b}x² + {c}x fonksiyonunun x = {x0} noktasındaki türevi kaçtır?";

            // LaTeX formatı
            string aCoef = FormatCoefficient(a, true);
            string bCoef = FormatCoefficient(b);
            string cCoef = FormatCoefficient(c);

            string latexString = $"f(x) = {aCoef}{(a == 1 || a == -1 ? "" : "")}{(a == 0 ? "" : "x^3")} {bCoef}{(b == 1 || b == -1 ? "" : "")}{(b == 0 ? "" : "x^2")} {cCoef}{(c == 1 || c == -1 ? "" : "")}{(c == 0 ? "" : "x")} \\text{{ için }} f'({x0}) = \\, ?";

            string explanation = $"f(x) = {a}x³ {(b >= 0 ? "+" : "")}{b}x² {(c >= 0 ? "+" : "")}{c}x fonksiyonunun türevi:\n" +
                           $"f'(x) = {3 * a}x² {(2 * b >= 0 ? "+" : "")}{2 * b}x {(c >= 0 ? "+" : "")}{c}\n" +
                           $"x = {x0} için türev değeri:\n" +
                           $"f'({x0}) = {3 * a}·({x0})² {(2 * b >= 0 ? "+" : "")}{2 * b}·({x0}) {(c >= 0 ? "+" : "")}{c}\n" +
                           $"f'({x0}) = {3 * a}·{x0 * x0} {(2 * b >= 0 ? "+" : "")}{2 * b}·{x0} {(c >= 0 ? "+" : "")}{c}\n" +
                           $"f'({x0}) = {3 * a * x0 * x0} {(2 * b * x0 >= 0 ? "+" : "")}{2 * b * x0} {(c >= 0 ? "+" : "")}{c} = {answer}";

            return new DerivativeQuestion
            {
                QuestionText = questionText,
                QuestionLaTeX = WrapLatexQuestion(latexString),
                Answer = answer,
                ExplanationText = explanation
            };
        }
        // Katsayı belirleme sorusu
        public static DerivativeQuestion GenerateFindCoefficientQuestion()
        {
            // Bilinen katsayılar
            int a = random.Next(1, 6); // x² katsayısı (0 olmayan)

            // Bilinmeyen katsayı (sorulacak)
            int b = random.Next(-5, 6); // x katsayısı
            while (b == 0) b = random.Next(-5, 6); // 0 olmasın

            // Sabit terim
            int c = random.Next(-10, 11);

            // Türev alınacak nokta
            int x0 = random.Next(-3, 4);

            // Bu noktadaki türev değeri: f'(x0) = 2a(x0) + b
            double derivativeAtX0 = 2 * a * x0 + b;

            // Sorunun cevabı b olacak
            double answer = b;

            string questionText = $"f(x) = {a}x² + bx + {c} fonksiyonunun x = {x0} noktasındaki türevi {derivativeAtX0} ise b kaçtır?";

            // LaTeX formatı
            string aCoef = FormatCoefficient(a, true);
            string cCoef = FormatCoefficient(c);

            string latexString = $"f(x) = {aCoef}{(a == 1 || a == -1 ? "" : "")}{(a == 0 ? "" : "x^2")} + bx {cCoef} \\text{{ fonksiyonunun }} f'({x0}) = {derivativeAtX0} \\text{{ ise }} b = \\, ?";

            string explanation = $"f(x) = {a}x² + bx {(c >= 0 ? "+" : "")}{c} fonksiyonunun türevi:\n" +
                           $"f'(x) = {2 * a}x + b\n" +
                           $"x = {x0} için türev değeri:\n" +
                           $"f'({x0}) = {2 * a}·({x0}) + b = {derivativeAtX0}\n" +
                           $"{2 * a}·{x0} + b = {derivativeAtX0}\n" +
                           $"{2 * a * x0} + b = {derivativeAtX0}\n" +
                           $"b = {derivativeAtX0} - {2 * a * x0} = {answer}";

            return new DerivativeQuestion
            {
                QuestionText = questionText,
                QuestionLaTeX = WrapLatexQuestion(latexString),
                Answer = answer,
                ExplanationText = explanation
            };
        }
        // Çarpım kuralı ile türev sorusu
        public static DerivativeQuestion GenerateProductRuleQuestion()
        {
            // İki fonksiyon tipi belirleyelim: f(x) = x + a ve g(x) = x + b
            int a = random.Next(-5, 6);
            int b = random.Next(-5, 6);
            while (a == b) b = random.Next(-5, 6); // a ve b farklı olsun

            // x = c noktasında türev değerini soracağız
            int c = random.Next(-3, 4);

            // f(x) = x + a, f'(x) = 1
            // g(x) = x + b, g'(x) = 1
            // h(x) = f(x) * g(x) = (x + a)(x + b) = x² + (a+b)x + ab
            // h'(x) = f'(x)g(x) + f(x)g'(x) = 1·(x+b) + (x+a)·1 = 2x + a + b
            // h'(c) = 2c + a + b

            double answer = 2 * c + a + b;

            string questionText = $"h(x) = (x + {a})(x + {b}) fonksiyonunun x = {c} noktasındaki türevi kaçtır?";

            // LaTeX formatı
            string aSign = a >= 0 ? "+" : "-";
            string bSign = b >= 0 ? "+" : "-";
            int absA = Math.Abs(a);
            int absB = Math.Abs(b);

            string latexString = $"h(x) = (x {aSign} {absA})(x {bSign} {absB}) \\text{{ için }} h'({c}) = \\, ?";

            string explanation = $"h(x) = (x {(a >= 0 ? "+" : "")}{a})(x {(b >= 0 ? "+" : "")}{b}) için çarpım kuralını uygulayalım.\n" +
                           $"f(x) = x {(a >= 0 ? "+" : "")}{a} ve g(x) = x {(b >= 0 ? "+" : "")}{b} olsun.\n" +
                           $"f'(x) = 1 ve g'(x) = 1\n" +
                           $"h'(x) = f'(x)·g(x) + f(x)·g'(x)\n" +
                           $"h'(x) = 1·(x {(b >= 0 ? "+" : "")}{b}) + (x {(a >= 0 ? "+" : "")}{a})·1\n" +
                           $"h'(x) = (x {(b >= 0 ? "+" : "")}{b}) + (x {(a >= 0 ? "+" : "")}{a})\n" +
                           $"h'(x) = 2x {(a + b >= 0 ? "+" : "")}{a + b}\n" +
                           $"x = {c} için türev değeri:\n" +
                           $"h'({c}) = 2·{c} {(a + b >= 0 ? "+" : "")}{a + b} = {2 * c} {(a + b >= 0 ? "+" : "")}{a + b} = {answer}";

            return new DerivativeQuestion
            {
                QuestionText = questionText,
                QuestionLaTeX = WrapLatexQuestion(latexString),
                Answer = answer,
                ExplanationText = explanation
            };
        }
    }
}
