using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class DomainRangeGenerator : FunctionQuestionGenerator
    {
        public override Question GenerateQuestion()
        {
            var function = GetRandomFunction();
            string questionText;
            string answer;

            if (function.function.Contains("√"))
            {
                questionText = $"{function.function} fonksiyonunun tanım kümesi nedir?";
                string funcstring = function.function;
                answer = "[-" + (funcstring.ElementAt(14).ToString() + "/" + funcstring.ElementAt(9).ToString()) + ", ∞)";
            }
            else if (function.function.Contains("/"))
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

            return new Question { QuestionText = questionText, Answer = answer };
        }

        protected override string GenerateAnswer(string question)
        {
            throw new NotImplementedException();
        }
    }
}
