using AngouriMath.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Fonksiyonlar
{
    public abstract class FunctionQuestionGenerator
    {
        protected static readonly Random random = new Random();

        internal abstract List<FunctionRepository> GenerateQuestion();
        protected abstract List<string> GenerateAnswer(string answer, int a, int b);

        internal (string function, Func<double, double> func, List<int> parameters, FunctionType functionType, int denominator, string denomsign) GetRandomFunction(int rnd = -1)
        {
            // Rastgele fonksiyon tipi seç (Lineer, Karesel, Köklü, Mutlak, Üstel, Rasyonel)
            if(rnd==-1)
            rnd = random.Next(0, 6);

            FunctionType functionType = (FunctionType)rnd; // Fonksiyon tipini rastgele seçer

            // Parametre sınırları
            int a = random.Next(1, 5);
            int b = random.Next(0, 6);
            int c = random.Next(1, 4);
            int denominator = 0;
            string denomsign = "";
            
            string returnQuestion;
            Func<double, double> returnFunc;

            switch (functionType)
            {
                case FunctionType.Linear: // Lineer: f(x) = ax + b  >0<
                    returnQuestion = $"f(x) = {a}x {(b > 0 ? $"+ {b}" : "")}".Trim();
                    returnFunc = x => a * x + b;
                    break;

                case FunctionType.Quadratic: // Karesel: f(x) = ax² ± bx ± c  >1<
                    bool isNegative1 = random.Next(2) == 0;
                    bool isNegative2 = random.Next(2) == 0;

                    returnQuestion = $"f(x) = {a}x^2 {(isNegative1 ? $"- {b}x" : $"+ {b}x")} {(isNegative2 ? $"- {c}" : $"+ {c}")}";
                    returnFunc = x => (a * x * x) + ((isNegative1 ? -b : b) * x) + (isNegative2 ? -c : c);
                    break;

                case FunctionType.Root: // Köklü: f(x) = √(ax + b)  >2<
                    b = random.Next(1, 5);
                    returnQuestion = $"f(x) = √({a}x + {b})";
                    returnFunc = x => a * x + b;
                    break;

                case FunctionType.Absolute: // Mutlak: f(x) = |ax - b|  >3<
                    returnQuestion = $"f(x) = |{a}x - {b}|";
                    returnFunc = x => Math.Abs(a * x - b);
                    break;

                case FunctionType.Exponential: // Üstel: f(x) = a^(x ± b)  >4<
                    string operation = random.Next(2) == 0 ? "+" : "-";
                    b = random.Next(3);
                    returnQuestion = $"f(x) = {a}^(x {operation} {b})";
                    returnFunc = x => Math.Pow(a, operation == "+" ? x + b : x - b);
                    break;

                case FunctionType.Rational: // Rasyonel: f(x) = (ax + b)/(x ± c)  >5<
                    int denominatorOffset = random.Next(1, 4);
                    string denomSign = random.Next(2) == 0 ? "+" : "-";
                    returnQuestion = $"f(x) = ({a}x + {b})/(x {denomSign} {denominatorOffset})";
                    returnFunc = x => (a * x + b) / (x + (denomSign == "+" ? denominatorOffset : -denominatorOffset));
                    denominator = denominatorOffset;
                    denomsign = denomSign;
                    break;

                default:
                    returnQuestion = "f(x) = x";
                    returnFunc = x => x;
                    break;
            }

            List<int> parameters = new List<int> { a, b, c };

            return (returnQuestion, returnFunc, parameters, functionType,denominator,denomsign);

        }


        public static string GetClosestSquareRoot(double target)
        {
            int closestN = 1;
            double minDiff = double.MaxValue;

            
            for (int n = 1; n <= 1000; n++)
            {
                double sqrtValue = Math.Sqrt(n);
                double diff = Math.Abs(sqrtValue - target);

                if (diff < minDiff)
                {
                    minDiff = diff;
                    closestN = n;
                }
            }

            return SimplifySquareRoot(closestN);
        }


        public static string SimplifySquareRoot(int n)
        {
            int outside = 1;
            int inside = n;

            // n sayısını en büyük kare çarpanına ayır
            for (int i = 2; i * i <= inside; i++)
            {
                while (inside % (i * i) == 0)
                {
                    inside /= i * i;
                    outside *= i;
                }
            }

            if (inside == 1)
                return outside.ToString();
            else if (outside == 1)
                return $"√{inside}";
            else
                return $"{outside}√{inside}";
        }

        public static string GetRationalValue(int a, int b)
        {
            return $"{a}/{b}".Simplify().ToString();
        }


    }
}
