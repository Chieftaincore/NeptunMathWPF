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
                Answer = Math.Round(result, 2).ToString()
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

        protected override string GenerateAnswer(string question)
        {
            throw new NotImplementedException();
        }
    }
}
