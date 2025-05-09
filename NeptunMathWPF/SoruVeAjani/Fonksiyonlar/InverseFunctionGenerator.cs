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
        int y;
        internal override List<FunctionRepository> GenerateQuestion()
        {
            int a = random.Next(1, 5);
            int b = random.Next(1, 5);
            y = random.Next(1, 10);

            string question = $"f(x) = {a}x + {b} \n f⁻¹({y})";
            Question qst = new Question
            {
                QuestionText = $"f(x) = {a}x + {b} fonksiyonunun tersi olan f⁻¹(y) fonksiyonunda f⁻¹({y}) değeri nedir?",
                Answer = GetRationalValue(y - b, a),
                WrongAnswers = GenerateAnswer(((double)(y - b) / a).ToString(), a, b)
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

        protected override List<string> GenerateAnswer(string answer, int a, int b)
        {
            double correct = double.Parse(answer);
            string rationalAnswer = GetRationalValue(y - b, a);

            var candidates = new HashSet<string>
    {
        // Hatalı cevaplar
        GetRationalValue(a + random.Next(10), b + random.Next(10)),
        GetRationalValue(a - random.Next(10), b - random.Next(10)),
        a.ToString(),
        (a*random.Next(5) + random.Next(5)).ToString(),
        GetRationalValue(a + random.Next(10), b),
        GetRationalValue((int)correct * a,b),
        "0",
        GetRationalValue(a , b + random.Next(10)),
        GetRationalValue(a + random.Next(10) , b - random.Next(10)),
        GetRationalValue(a - random.Next(10) , b + random.Next(10)),
    };

            var wrongs = candidates
                .Where(w => w != rationalAnswer && w!="NaN")
                .Take(4)
                .ToList();

            int offset = random.Next(1, 50);
            while (wrongs.Count < 4)
            {
                string extra1 = (correct + offset).ToString();
                string extra2 = (correct - offset).ToString();
                
                if (extra1 != rationalAnswer && !wrongs.Contains(extra1) && extra1!="NaN")
                    wrongs.Add(extra1);

                if (wrongs.Count >= 4) break;

                if (extra2 != rationalAnswer && !wrongs.Contains(extra2) && extra2 != "NaN")
                    wrongs.Add(extra2);

                offset++;
                if (offset > 100) break;
            }

            return wrongs;
        }

    }
}
