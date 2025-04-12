using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class InverseFunctionGenerator : FunctionQuestionGenerator
    {
        internal override List<FunctionRepository> GenerateQuestion()
        {
            int a = random.Next(1, 5);
            int b = random.Next(1, 5);
            int y = random.Next(1, 10);

            string question = $"f(x) = {a}x + {b} \n f⁻¹({y})";
            Question qst = new Question
            {
                QuestionText = $"f(x) = {a}x + {b} fonksiyonunun tersi olan f⁻¹(y) fonksiyonunda f⁻¹({y}) değeri nedir?",
                Answer = ((double)(y - b) / a).ToString()
            };

            return new List<FunctionRepository>
            {
                new FunctionRepository
                {
                    a = a,
                    b = b,
                    x=y,
                    question = question,
                    functionType = FunctionType.Inverse,
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
