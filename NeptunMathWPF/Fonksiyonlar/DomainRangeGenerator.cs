using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class DomainRangeGenerator : FunctionQuestionGenerator
    {
        internal override List<FunctionRepository> GenerateQuestion()
        {
            var function = GetRandomFunction();
            string questionText;
            string answer;

            if (function.functionType==FunctionType.Root)
            {
                questionText = $"{function.function} fonksiyonunun tanım kümesi nedir?";
                answer = "[-" + (function.parameters.ElementAt(1).ToString() + "/" + function.parameters.ElementAt(0).ToString()) + ", ∞)";
            }
            else if (function.functionType==FunctionType.Rational)
            {
                questionText = $"{function.function} fonksiyonunun tanım kümesi nedir?";
                var parts = function.function.Split('/')[1];
                if (parts.Contains("x +"))
                {
                    answer = "Tüm reel sayılar, x ≠ -c";
                }
                else
                {
                    answer = "Tüm reel sayılar, x ≠ c";
                }
            }
            else
            {
                questionText = $"{function.function} fonksiyonunun görüntü kümesi nedir?";
                answer = "Tüm reel sayılar";
            }

            Question qst = new Question { QuestionText = questionText, Answer = answer };

            return new List<FunctionRepository>
            {
                new FunctionRepository
                {
                    a = function.parameters.ElementAt(0),
                    b = function.parameters.ElementAt(1),
                    c = function.parameters.ElementAt(2),
                    question = function.function,
                    functionType = function.functionType,
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
