using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class FunctionValueGenerator : FunctionQuestionGenerator
    {
        public override Question GenerateQuestion()
        {
            var function = GetRandomFunction();
            int x = random.Next(1, 10);
            double result = function.func(x);

            return new Question
            {
                QuestionText = $"{function.function} fonksiyonu için f({x}) değeri nedir?",
                Answer = Math.Round(result, 2).ToString()
            };
        }

        protected override string GenerateAnswer(string question)
        {
            throw new NotImplementedException();
        }
    }
}
