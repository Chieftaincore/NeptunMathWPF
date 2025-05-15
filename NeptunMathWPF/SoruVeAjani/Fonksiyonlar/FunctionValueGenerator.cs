using NeptunMathWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XamlMath.Utils;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class FunctionValueGenerator : FunctionQuestionGenerator
    {
        int x = random.Next(1, 10);
        internal override List<FunctionRepository> GenerateQuestion()
        {
            Question qst = new Question();
            var function = GetRandomFunction();

            double result = function.functionType == FunctionType.Root ? Math.Sqrt(function.func(x)) : function.func(x);

            if (function.functionType == FunctionType.Exponential)
            {
                while (true)
                {
                    function = GetRandomFunction();
                    if (function.functionType != FunctionType.Exponential)
                        break;
                }
            }

            if (function.functionType != FunctionType.Rational)
            {
                qst = new Question
                {
                    QuestionText = $"{function.questiontext} fonksiyonu için f({x}) değeri nedir?".Replace(" + 0", "").Replace("1x", "x").Replace(" - 0", "").Replace("+ 0x", "").Replace("- 0x", ""),
                    LatexText = $"{function.latex} \\text{{ fonksiyonu için }} f({x}) \\text{{ değeri nedir?}}".Replace(" + 0", "").Replace("1x", "x").Replace(" - 0", "").Replace("+ 0x", "").Replace("- 0x", ""),
                    Answer = (function.functionType == FunctionType.Root) ? GetClosestSquareRoot(result).Replace("√", "\\sqrt") : Math.Round(result, 2).ToString(),
                    WrongAnswers = (function.functionType == FunctionType.Root) ? GenerateAnswerRoot(result.ToString(), function.parameters[0], function.parameters[1]) : GenerateAnswer(result.ToString(), function.parameters[0], function.parameters[1])
                };
            }
            else
            {
                qst = new Question
                {
                    QuestionText = $"{function.questiontext} fonksiyonu için f({x}) değeri nedir?".Replace(" + 0", "").Replace("1x", "x").Replace(" - 0", "").Replace("+ 0x", "").Replace("- 0x", ""),
                    LatexText = $"{function.latex} \\text{{ fonksiyonu için }} f({x}) \\text{{ değeri nedir?}}".Replace(" + 0", "").Replace("1x", "x").Replace(" - 0", "").Replace("+ 0x", "").Replace("- 0x", ""),
                    Answer = GetRationalValue((function.parameters[0] * x) + function.parameters[1], x + (function.denomsign == "+" ? function.denominator : -function.denominator)),
                    WrongAnswers = GenerateAnswerRational(((int)result).ToString(), function.parameters[0], function.parameters[1], function.denominator, function.denomsign)
                };
            }
            
                return new List<FunctionRepository>
            {
                new FunctionRepository
                {
                    a = function.parameters.ElementAt(0),
                    b = function.parameters.ElementAt(1),
                    c = function.parameters.ElementAt(2),
                    x = x,
                    question = function.questiontext,
                    functionType = function.functionType,
                    questionObject = qst
                }
            };
        }

        protected override List<string> GenerateAnswer(string answer, int a, int b)
        {
            double correct = double.Parse(answer);

            var temp = new List<string>
    {
        (a * b).ToString(),
        (a * 1 + b).ToString(),
        Math.Ceiling(correct).ToString(),
        (correct + random.Next(1, 50)).ToString(),
        (correct - random.Next(1, 50)).ToString()
    };

            var extras = Enumerable
                .Range(1, 6)
                .SelectMany(offset => new[]
                {
            (correct + offset).ToString(),
            (correct - offset).ToString()
                });

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

        protected List<string> GenerateAnswerRoot(string answer, int a, int b)
        {
            double correct = double.Parse(answer);

            var temp = new List<string>
    {
        SimplifySquareRoot(a * b).Replace("√","\\sqrt"),
        SimplifySquareRoot(a * 1 + b).Replace("√","\\sqrt"),
        SimplifySquareRoot((int)Math.Ceiling(correct)).Replace("√","\\sqrt"),
        GetClosestSquareRoot(correct + random.Next(1, 50)).Replace("√","\\sqrt"),
        GetClosestSquareRoot(correct - random.Next(1, 50)).ToString().Replace("√","\\sqrt")
    };

            var extras = Enumerable
                .Range(1, 6)
                .SelectMany(offset => new[]
                {
            GetClosestSquareRoot(correct + offset).ToString().Replace("√","\\sqrt"),
            GetClosestSquareRoot(correct - offset).ToString().Replace("√","\\sqrt")
                });

            List<string> tempList = temp.ToList();
            Genel.Shuffle(tempList);

            var wrongs = tempList
            .Concat(extras)
                .Where(w => w != GetClosestSquareRoot(correct).Replace("√", "\\sqrt"))
                .Distinct()
                .Take(4)
                .ToList();

            return wrongs;
        }

        protected List<string> GenerateAnswerRational(string answer, int a, int b, int denominator, string denomsign)
        {
            double correct = double.Parse(answer);
            string rationalAnswer = GetRationalValue((a*x) + b, x + (denomsign == "+" ? denominator : -denominator));

            var candidates = new HashSet<string>
    {
        // Hatalı cevaplar
        GetRationalValue(a + random.Next(10), x + random.Next(10)),
        GetRationalValue(a - random.Next(10) , x + random.Next(10)),
        GetRationalValue(-((a*x) + b), x + (denomsign == "+" ? denominator : -denominator)), // Doğru cevabın eksili hali
        GetRationalValue((a*x) + b, x + (denomsign != "+" ? denominator : -denominator)), // Doğru cevabın yanlış paydalı hali
        GetRationalValue(a + random.Next(10), b),
        GetRationalValue(a + random.Next(10) , x - random.Next(10)),
        GetRationalValue(a - random.Next(10), x - random.Next(10)),
        GetRationalValue((int)correct * a,b),
        "0",
        GetRationalValue(a , b + random.Next(10)),
    };

            List<string> candidatesList = candidates.ToList();
            Genel.Shuffle(candidatesList);

            var wrongs = candidatesList
                .Where(w => w != rationalAnswer && w != "NaN")
                .Take(4)
                .ToList();

            int offset = random.Next(1, 50);
            while (wrongs.Count < 4)
            {
                string extra1 = (correct + offset).ToString();
                string extra2 = (correct - offset).ToString();

                if (extra1 != rationalAnswer && !wrongs.Contains(extra1) && extra1 != "NaN")
                    wrongs.Add(extra1);

                if (wrongs.Count >= 4) break;

                if (extra2 != rationalAnswer && !wrongs.Contains(extra2) && extra2 != "NaN")
                    wrongs.Add(extra2);

                offset++;
                if (offset > 100) break;
            }

            return wrongs;
        }
    }
}
