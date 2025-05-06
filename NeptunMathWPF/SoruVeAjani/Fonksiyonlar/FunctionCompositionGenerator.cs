using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class FunctionCompositionGenerator : FunctionQuestionGenerator
    {
        internal override List<FunctionRepository> GenerateQuestion()
        {
            var f = GetRandomFunction();
            var g = GetRandomFunction();
            int x = random.Next(1, 5);

            double result = f.func(g.func(x));

            Question qst = new Question
            {
                QuestionText = $"f(x) = {f.function.Split('=')[1]} ve g(x) = {g.function.Split('=')[1]} ise (f∘g)({x}) kaçtır?",
                Answer = ToRational(Math.Round(result, 2)),
                WrongAnswers = GenerateAnswer(Math.Round(result, 2).ToString(), f.parameters[0], f.parameters[1])
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
            ToRational((b * (a * x0))),
            ToRational((a + b)),
            ToRational((correct + 2)),
            ToRational((correct - 2)),
            ToRational((correct + random.Next(1, 4)))
        };

            // Ekstra sapma değerlerini üret (offset 1’den 6’ya kadar)
            var extras = Enumerable
                .Range(1, 6)
                .SelectMany(offset => new[]
                {
                ToRational((correct + offset)),
                ToRational((correct - offset))
                });

            // Birleştir, doğru cevabı çıkar, tekrarları temizle ve ilk 4’ü al
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
