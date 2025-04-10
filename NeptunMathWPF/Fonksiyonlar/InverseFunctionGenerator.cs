using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class InverseFunctionGenerator : FunctionQuestionGenerator
    {
        public override Question GenerateQuestion()
        {
            int a = random.Next(1, 5);
            int b = random.Next(1, 5);
            int y = random.Next(1, 10);

            return new Question
            {
                QuestionText = $"f(x) = {a}x + {b} fonksiyonunun tersi olan f⁻¹(y) fonksiyonunda f⁻¹({y}) değeri nedir?",
                Answer = ((double)(y - b) / a).ToString()
            };
        }

        protected override string GenerateAnswer(string question)
        {
            throw new NotImplementedException();
        }
    }
}
