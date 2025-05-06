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
                Answer = ToRational(((double)(y - b) / a)),
                WrongAnswers = GenerateAnswer(((double)(y - b) / a).ToString(),a,b)
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
            var candidates = new HashSet<string>
            {
                ToRational((correct + b - b)),
                ToRational((correct * a)),
                "0",
                ToRational((correct +(double) b / a)),
                ToRational((correct -(double) b / a))
            };

            var wrongs = candidates
                .Where(w => w != answer)
                .Take(4)
                .ToList();

            int offset = 1;
            while (wrongs.Count < 4)
            {
                var extra1 = ToRational((correct + offset));
                var extra2 = ToRational((correct - offset));

                if (extra1 != answer && !wrongs.Contains(extra1))
                    wrongs.Add(extra1);

                if (wrongs.Count >= 4) break;

                if (extra2 != answer && !wrongs.Contains(extra2))
                    wrongs.Add(extra2);

                offset++;
            }

            return wrongs;
        }
    }
}
