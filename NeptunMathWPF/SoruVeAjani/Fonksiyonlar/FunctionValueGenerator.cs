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
        internal override List<FunctionRepository> GenerateQuestion()
        {
            var function = GetRandomFunction();
            int x = random.Next(1, 10);
            double result = function.functionType == FunctionType.Root ? Math.Sqrt(function.func(x)) : function.func(x);

            if (function.functionType==FunctionType.Exponential)
            {
                while(true)
                {
                    function = GetRandomFunction();
                    if (function.functionType != FunctionType.Exponential)
                        break;
                }
            }

            Question qst = new Question
            {
                QuestionText = $"{function.function} fonksiyonu için f({x}) değeri nedir?",
                Answer = (function.functionType == FunctionType.Root) ? GetClosestSquareRoot(result) : Math.Round(result, 2).ToString(),
                WrongAnswers = (function.functionType == FunctionType.Root) ? GenerateAnswerRoot(result.ToString(), function.parameters[0], function.parameters[1]) : GenerateAnswer(result.ToString(), function.parameters[0], function.parameters[1])
            };

            return new List<FunctionRepository>
            {
                new FunctionRepository
                {
                    a = function.parameters.ElementAt(0),
                    b = function.parameters.ElementAt(1),
                    c = function.parameters.ElementAt(2),
                    x = x,
                    question = function.function,
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

            var wrongs = temp
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
        SimplifySquareRoot(a * b),
        SimplifySquareRoot(a * 1 + b),
        SimplifySquareRoot((int)Math.Ceiling(correct)),
        GetClosestSquareRoot(correct + random.Next(1, 50)),
        GetClosestSquareRoot(correct - random.Next(1, 50)).ToString()
    };

            var extras = Enumerable
                .Range(1, 6)
                .SelectMany(offset => new[]
                {
            GetClosestSquareRoot(correct + offset).ToString(),
            GetClosestSquareRoot(correct - offset).ToString()
                });

            var wrongs = temp
            .Concat(extras)
                .Where(w => w != GetClosestSquareRoot((int)correct))
                .Distinct()
                .Take(4)
                .ToList();

            return wrongs;
        }
    }
}
