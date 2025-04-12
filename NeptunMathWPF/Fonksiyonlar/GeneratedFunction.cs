using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    internal class GeneratedFunction
    {
        public List<FunctionRepository> repository { get;}
        public bool HasMultipleFunctions => repository.Count > 1; // Listede 1den fazla eleman varsa true olur.
        internal GeneratedFunction()
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
        }
    }
}
