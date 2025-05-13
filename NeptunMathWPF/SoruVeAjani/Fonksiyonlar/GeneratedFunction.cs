using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Fonksiyonlar
{
    internal class GeneratedFunction
    {
        public List<FunctionRepository> repository { get; }

        //Çevirme için gerekli generator type alma
        public FunctionQuestionGenerator gen { get; }

        //public bool HasMultipleFunctions => repository.Count > 1; // Listede 1den fazla eleman varsa true olur.
        internal GeneratedFunction()
        {
            bool whileCheck = true;
            while (whileCheck)
            {
                List<FunctionQuestionGenerator> generators = new List<FunctionQuestionGenerator>
                {

                    new FunctionValueGenerator(),
                    new FunctionCompositionGenerator(),
                    new InverseFunctionGenerator(),
                    new DomainRangeGenerator()

                };
                var generator = generators[new Random().Next(generators.Count)];
                repository = generator.GenerateQuestion();

                gen = generator;



                foreach (var item in repository)
                {
                    if (!item.questionObject.Answer.Contains("NaN") && item.questionObject.WrongAnswers.Count == 4 && item.questionObject.Answer!= "∞" && !string.IsNullOrEmpty(item.questionObject.Answer)) //Payda 0 gelirse veya yanlış cevapların sayısı 4'ten küçük gelirse tekrar soru üretsin
                    {
                        whileCheck = false;
                    }
                }

            }
        }
    }
}
