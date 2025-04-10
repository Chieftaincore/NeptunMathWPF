using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public abstract class FunctionQuestionGenerator
    {
        protected Random random = new Random();

        public abstract Question GenerateQuestion();
        protected abstract string GenerateAnswer(string question);

        protected (string function, Func<double, double> func) GetRandomFunction()
        {
            // Rastgele fonksiyon tipi seç (Lineer, Karesel, Köklü, Mutlak, Üstel, Rasyonel)
            int functionType = random.Next(0, 6);

            // Parametre sınırları
            int a = random.Next(1, 5);
            int b = random.Next(0, 6);
            int c = random.Next(1, 4);

            switch (functionType)
            {
                case 0: // Lineer: f(x) = ax + b
                    return ($"f(x) = {a}x {(b > 0 ? $"+ {b}" : "")}".Trim(),
                            x => a * x + b);

                case 1: // Karesel: f(x) = ax² ± bx ± c
                    bool isNegative1 = random.Next(2) == 0;
                    bool isNegative2 = random.Next(2) == 0;

                    return ($"f(x) = {a}x² {(isNegative1 ? $"- {b}x" : $"+ {b}x")} {(isNegative2 ? $"- {c}" : $"+ {c}")}",
                            x => a * x * x + (isNegative1 ? -b : b) * x + (isNegative2 ? -c : c));

                case 2: // Köklü: f(x) = √(ax + b)
                    b = random.Next(1, 5);
                    return ($"f(x) = √({a}x + {b})",
                            x => Math.Sqrt(a * x + b));

                case 3: // Mutlak: f(x) = |ax - b|
                    return ($"f(x) = |{a}x - {b}|",
                            x => Math.Abs(a * x - b));

                case 4: // Üstel: f(x) = a^(x +- b)
                    string operation = random.Next(2) == 0 ? "+" : "-";
                    return ($"f(x) = {a}^(x {operation} {b})",
                            x => Math.Pow(a, operation == "+" ? x + b : x - b));

                case 5: // Rasyonel: f(x) = (ax + b)/(x ± c)
                    int denominatorOffset = random.Next(1, 4);
                    string denomSign = random.Next(2) == 0 ? "+" : "-";
                    return ($"f(x) = ({a}x + {b})/(x {denomSign} {denominatorOffset})",
                            x => (a * x + b) / (x + (denomSign == "+" ? denominatorOffset : -denominatorOffset)));

                default:
                    return ("f(x) = x", x => x);
            }
        }
    }
}
