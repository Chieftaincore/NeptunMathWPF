using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class FunctionValueGenerator : FunctionQuestionGenerator
    {
        internal override List<FunctionRepository> GenerateQuestion()
        {
            var function = GetRandomFunction();
            int x = random.Next(1, 10);
            double result = function.func(x);


            Question qst = new Question
            {
                QuestionText = $"{function.function} fonksiyonu için f({x}) değeri nedir?",
                Answer = ToRational(Math.Round(result, 2)),
                WrongAnswers = GenerateAnswer(Math.Round(result, 2).ToString(), function.parameters[0], function.parameters[1])
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
        ToRational((a * b)),
        ToRational((a * 1 + b)),
        ToRational(Math.Ceiling(correct)),
        ToRational((correct + 1)),
        ToRational((correct - 1))
    };

            var extras = Enumerable
                .Range(1, 6)                                  
                .SelectMany(offset => new[]
                {
            ToRational((correct + offset)),           
            ToRational((correct - offset))            
                });

            var wrongs = temp
                .Concat(extras)
                .Where(w => w != answer)
                .Distinct()
                .Take(4)
                .ToList();

            return wrongs;
        }

    }
}
