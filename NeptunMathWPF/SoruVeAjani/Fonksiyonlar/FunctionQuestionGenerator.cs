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

        internal abstract List<FunctionRepository> GenerateQuestion();
        protected abstract List<string> GenerateAnswer(string answer, int a, int b);

        internal (string function, Func<double, double> func, List<int> parameters, FunctionType functionType) GetRandomFunction()
        {
            // Rastgele fonksiyon tipi seç (Lineer, Karesel, Köklü, Mutlak, Üstel, Rasyonel)
            int rnd = random.Next(0, 6);
            FunctionType functionType = (FunctionType)rnd; // Fonksiyon tipini rastgele seçer

            // Parametre sınırları
            int a = random.Next(1, 5);
            int b = random.Next(0, 6);
            int c = random.Next(1, 4);

            string returnQuestion;
            Func<double, double> returnFunc;

            switch (functionType)
            {
                case FunctionType.Linear: // Lineer: f(x) = ax + b
                    returnQuestion = $"f(x) = {a}x {(b > 0 ? $"+ {b}" : "")}".Trim();
                    returnFunc = x => a * x + b;
                    break;

                case FunctionType.Quadratic: // Karesel: f(x) = ax² ± bx ± c
                    bool isNegative1 = random.Next(2) == 0;
                    bool isNegative2 = random.Next(2) == 0;

                    returnQuestion = $"f(x) = {a}x² {(isNegative1 ? $"- {b}x" : $"+ {b}x")} {(isNegative2 ? $"- {c}" : $"+ {c}")}";
                    returnFunc = x => a * x * x + (isNegative1 ? -b : b) * x + (isNegative2 ? -c : c);
                    break;

                case FunctionType.Root: // Köklü: f(x) = √(ax + b)
                    b = random.Next(1, 5);
                    returnQuestion = $"f(x) = √({a}x + {b})";
                    returnFunc = x => Math.Sqrt(a * x + b);
                    break;

                case FunctionType.Absolute: // Mutlak: f(x) = |ax - b|
                    returnQuestion = $"f(x) = |{a}x - {b}|";
                    returnFunc = x => Math.Abs(a * x - b);
                    break;

                case FunctionType.Exponential: // Üstel: f(x) = a^(x +- b)
                    string operation = random.Next(2) == 0 ? "+" : "-";
                    returnQuestion = $"f(x) = {a}^(x {operation} {b})";
                    returnFunc = x => Math.Pow(a, operation == "+" ? x + b : x - b);
                    break;

                case FunctionType.Rational: // Rasyonel: f(x) = (ax + b)/(x ± c)
                    int denominatorOffset = random.Next(1, 4);
                    string denomSign = random.Next(2) == 0 ? "+" : "-";
                    returnQuestion = $"f(x) = ({a}x + {b})/(x {denomSign} {denominatorOffset})";
                    returnFunc = x => (a * x + b) / (x + (denomSign == "+" ? denominatorOffset : -denominatorOffset));
                    break;

                default:
                    returnQuestion = "f(x) = x";
                    returnFunc = x => x;
                    break;
            }

            List<int> parameters = new List<int> { a, b, c };

            return (returnQuestion, returnFunc, parameters, functionType);

        }

        public static string ToRational(double value, double tolerance = 1.0E-6)
        {
            double fraction = value;
            int sign = Math.Sign(fraction);
            fraction = Math.Abs(fraction);

            if (fraction == 0) return ("0");

            int lower_n = 0, lower_d = 1;
            int upper_n = 1, upper_d = 0;

            while (true)
            {
                int mediant_n = lower_n + upper_n;
                int mediant_d = lower_d + upper_d;

                if (mediant_d * (fraction + tolerance) < mediant_n)
                {
                    upper_n = mediant_n;
                    upper_d = mediant_d;
                }
                else if (mediant_n < (fraction - tolerance) * mediant_d)
                {
                    lower_n = mediant_n;
                    lower_d = mediant_d;
                }
                else
                {
                    if (mediant_d==1)
                    {
                        return ($"{sign * mediant_n}");
                    }
                    else
                    {
                        return ($"{sign * mediant_n} / {mediant_d}");
                    }
                }
            }
        }

    }
}
