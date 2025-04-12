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
                Answer = Math.Round(result, 2).ToString()
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
                new FunctionRepository{
                                        a = g.parameters.ElementAt(0),
                    b = g.parameters.ElementAt(1),
                    c = f.parameters.ElementAt(2),
                    x = x,
                    question = g.function,
                    functionType = g.functionType,
                    questionObject = qst
                }
            };
        }

        protected override string GenerateAnswer(string question)
        {
            throw new NotImplementedException();
        }
    }
}
