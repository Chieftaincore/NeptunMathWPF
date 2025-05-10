using AngouriMath;
using AngouriMath.Extensions;
using HonkSharp.Fluency;
using NeptunMathWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class FunctionCompositionGenerator : FunctionQuestionGenerator
    {
        internal override List<FunctionRepository> GenerateQuestion()
        {
            int x = random.Next(1, 5);
            string root = "";
            double sqr = 0;
            string answ = "";
            var f = GetRandomFunctionFor_f();
            var g = GetRandomFunctionFor_g(x);

            /*if (g.functionType != FunctionType.Root && f.functionType == FunctionType.Root)
            {
                while (true)
                {
                    g = GetRandomFunction();
                    if (g.functionType == FunctionType.Root)
                    {
                        break;
                    }
                }
            }

            if (g.functionType == FunctionType.Root && f.functionType == FunctionType.Root)
            {
                sqr = Math.Sqrt(g.func(x));
                root = $"{f.function.Split('=')[1].Replace("x", GetClosestSquareRoot(sqr))}".Insert(3, ".");
                MessageBox.Show(sqr.ToString() + " " + root);

            }
            else if (g.functionType == FunctionType.Root && f.functionType != FunctionType.Root)
            {
                sqr = Math.Sqrt(f.func(g.func(x)));
                root = GetClosestSquareRoot(sqr);
            }*/

            //rasyonel
            while (true)
            {
                try
                {

                    if ((g.functionType == FunctionType.Rational && f.functionType == FunctionType.Rational) || (g.functionType == FunctionType.Rational && f.functionType != FunctionType.Rational))
                    {
                        string ratio;
                        if (g.denomsign == "+")
                            ratio = GetRationalValue((g.parameters[0] * x) + g.parameters[1], x + g.denominator);
                        else
                            ratio = GetRationalValue((g.parameters[0] * x) + g.parameters[1], x - g.denominator);


                        answ = f.function.Split('=')[1].Substitute("x", ratio).Simplify().ToString();
                    }
                    else if (g.functionType != FunctionType.Rational && f.functionType == FunctionType.Rational)
                    {
                        string xVar = g.function.Split('=')[1].Substitute("x", x).ToString();
                        answ = f.function.Split('=')[1].Substitute("x", xVar).Simplify().ToString();
                    }
                    break;
                }
                catch (AngouriMath.Core.Exceptions.UnhandledParseException)
                {
                    f = GetRandomFunctionFor_f();
                    g = GetRandomFunctionFor_g(x);
                }
            }

            double result = f.func(g.func(x));
            Question qst = new Question
            {
                QuestionText = $"f(x) = {f.function.Split('=')[1]} ve g(x) = {g.function.Split('=')[1]} ise (f∘g)({x}) kaçtır?".Replace("- 0x", "").Replace("+ 0x", "").Replace(" + 0", "").Replace(" - 0", "").Replace("1x", "x"),
                Answer = (f.functionType == FunctionType.Rational || g.functionType == FunctionType.Rational) ? answ : result.ToString(),
                WrongAnswers = (f.functionType == FunctionType.Rational || g.functionType == FunctionType.Rational || f.functionType == FunctionType.Exponential || g.functionType == FunctionType.Exponential) ? GenerateAnswerRational(result, f.parameters[0], f.parameters[1], x, answ) : GenerateAnswer(Math.Round(result, 2).ToString(), f.parameters[0], f.parameters[1])
            };
            return new List<FunctionRepository>
            {
                new FunctionRepository
                {
                    a = f.parameters.ElementAt(0),
                    b = f.parameters.ElementAt(1),
                    c = f.parameters.ElementAt(2),
                    x = x,
                    question = f.function,
                    functionType = f.functionType,
                    questionObject = qst
                },
            };
        }

        protected override List<string> GenerateAnswer(string answer, int a, int b)
        {
            double correct = double.Parse(answer);
            int x0 = random.Next(1, 6);

            var temp = new List<string>
        {
            (b * (a * x0)).ToString(),
            (a + b).ToString(),
            (correct + random.Next(1,50)).ToString(),
            (correct - random.Next(1,50)).ToString(),
            (correct + random.Next(1, 4)).ToString()
        };

            // Ekstra sapma değerlerini üret (offset 1’den 6’ya kadar)
            var extras = Enumerable
                .Range(1, 6)
                .SelectMany(offset => new[]
                {
                ((correct + offset)).ToString(),
                ((correct - offset)).ToString()
                });

            // Birleştir, doğru cevabı çıkar, tekrarları temizle ve ilk 4’ü al

            List<string> tempList = temp.ToList();
            Genel.Shuffle(tempList);

            var wrongs = tempList
                .Concat(extras)
                .Where(w => w != answer)
                .Distinct()
                .Take(4)
                .ToList();

            return wrongs;
        }

        protected List<string> GenerateAnswerRoot(double sqr, int a, int b)
        {
            double correct = sqr;
            int x0 = random.Next(1, 6);

            var temp = new List<string>
        {
            SimplifySquareRoot(b *(a * x0)),
            SimplifySquareRoot(a + b),
            GetClosestSquareRoot(correct + random.Next(1,50)),
            GetClosestSquareRoot(correct - random.Next(1,50)),
            GetClosestSquareRoot(correct + random.Next(1, 4))
        };

            // Ekstra sapma değerlerini üret (offset 1’den 6’ya kadar)
            var extras = Enumerable
                .Range(1, 6)
                .SelectMany(offset => new[]
                {
                GetClosestSquareRoot(correct + offset),
                GetClosestSquareRoot(correct - offset)
                });

            // Birleştir, doğru cevabı çıkar, tekrarları temizle ve ilk 4’ü al

            List<string> tempList = temp.ToList();
            Genel.Shuffle(tempList);

            var wrongs = tempList
                .Concat(extras)
                .Where(w => w != GetClosestSquareRoot(sqr))
                .Distinct()
                .Take(4)
                .ToList();

            return wrongs;
        }

        protected List<string> GenerateAnswerRational(double answer, int a, int b, int x, string corAnsw)
        {
            double correct = answer;

            if (random.Next(2) == 1)
                GetRationalValue((a * x) + b, x + random.Next(7));
            else
                GetRationalValue((a * x) + b, x - random.Next(7));

            var candidates = new HashSet<string>
    {
        // Hatalı cevaplar
        GetRationalValue(a + random.Next(10), b + random.Next(10)),
        GetRationalValue(a - random.Next(10), b - random.Next(10)),
        a.ToString(),
        (a*random.Next(5) + random.Next(5)).ToString(),
        GetRationalValue(a + random.Next(10), b),
        GetRationalValue((int)correct * a,b),
        "0",
        GetRationalValue(a , b + random.Next(10)),
        GetRationalValue(a + random.Next(10) , b - random.Next(10)),
        GetRationalValue(a - random.Next(10) , b + random.Next(10)),
    };

            List<string> candidatesList = candidates.ToList();
            Genel.Shuffle(candidatesList);
            var wrongs = candidatesList
                .Where(w => w != corAnsw && w != "NaN")
                .Take(4)
                .ToList();

            int offset = random.Next(1, 50);
            while (wrongs.Count < 4)
            {
                string extra1 = (correct + offset).ToString();
                string extra2 = (correct - offset).ToString();

                if (extra1 != corAnsw && !wrongs.Contains(extra1) && extra1 != "NaN")
                    wrongs.Add(extra1);

                if (wrongs.Count >= 4) break;

                if (extra2 != corAnsw && !wrongs.Contains(extra2) && extra2 != "NaN")
                    wrongs.Add(extra2);

                offset++;
                if (offset > 100) break;
            }

            return wrongs;
        }

        private (string function, Func<double, double> func, List<int> parameters, FunctionType functionType, int denominator, string denomsign) GetRandomFunctionFor_f()
        {
            var returnFunction = GetRandomFunction();
            if (returnFunction.functionType == FunctionType.Root || returnFunction.functionType == FunctionType.Quadratic || returnFunction.functionType == FunctionType.Exponential)
            {
                while (true)
                {
                    returnFunction = GetRandomFunction();
                    if (returnFunction.functionType != FunctionType.Root && returnFunction.functionType != FunctionType.Quadratic && returnFunction.functionType != FunctionType.Exponential)
                    {
                        break;
                    }
                }
            }

            return returnFunction;
        }

        private (string function, Func<double, double> func, List<int> parameters, FunctionType functionType, int denominator, string denomsign) GetRandomFunctionFor_g(int x)
        {
            var returnFunction = GetRandomFunction();

            if (returnFunction.functionType == FunctionType.Root)
            {
                while (true)
                {
                    returnFunction = GetRandomFunction();
                    if (returnFunction.functionType != FunctionType.Root)
                    {
                        break;
                    }
                }
            }
            if (returnFunction.functionType == FunctionType.Exponential)
            {

                while (true)
                {
                    if (Math.Ceiling(returnFunction.func(x)) == returnFunction.func(x))
                        break;

                    returnFunction = GetRandomFunction(4);
                }


            }

            return returnFunction;
        }
    }
}
