using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class FunctionCompositionGenerator : FunctionQuestionGenerator
    {
        public override Question GenerateQuestion()
        {
            var f = GetRandomFunction();
            var g = GetRandomFunction();
            int x = random.Next(1, 5);

            double result = f.func(g.func(x));

            return new Question
            {
                QuestionText = $"f(x) = {f.function.Split('=')[1]} ve g(x) = {g.function.Split('=')[1]} ise (f∘g)({x}) kaçtır?",
                Answer = Math.Round(result, 2).ToString()
            };
        }

        protected override string GenerateAnswer(string question)
        {
            throw new NotImplementedException();
        }
    }
}
