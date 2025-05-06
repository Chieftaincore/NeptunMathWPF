using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AngouriMath.Entity;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class DomainRangeGenerator : FunctionQuestionGenerator
    {
        internal override List<FunctionRepository> GenerateQuestion()
        {
            var (expr, func, parameters, type) = GetRandomFunction();

            string domainAnswer;
            switch (type)
            {
                case FunctionType.Linear:
                case FunctionType.Quadratic:
                case FunctionType.Absolute:
                case FunctionType.Exponential:
                case FunctionType.Inverse:
                    domainAnswer = "ℝ"; // Tüm gerçel sayılar
                    break;

                case FunctionType.Root:
                    {
                        int a = parameters[0], b = parameters[1];

                        double boundary = -b / (double)a;
                        domainAnswer = a > 0
                            ? $"[ {Math.Round(boundary, 2)}, ∞ )"
                            : $"( -∞, {Math.Round(boundary, 2)} ]";
                    }
                    break;

                case FunctionType.Rational:
                    {
                        int denomOffset = parameters[2];
                        domainAnswer = $"ℝ \\ {{ {-denomOffset} }}";
                    }
                    break;

                default:
                    domainAnswer = "ℝ";
                    break;
            }

            var q = new Question
            {
                QuestionText = $"{expr} fonksiyonunun tanım kümesi nedir?",
                Answer = domainAnswer,
                WrongAnswers = GenerateAnswer(domainAnswer, parameters[0], parameters[1])
            };

            return new List<FunctionRepository>
            {
                new FunctionRepository
                {
                    question = expr,
                    functionType = type,
                    a = parameters[0],
                    b = parameters[1],
                    c = parameters.Count > 2 ? parameters[2] : (int?)null,
                    questionObject = q
                }
            };
        }

        protected override List<string> GenerateAnswer(string answer, int a, int b)
        {
            var candidates = new HashSet<string>
            {
                "ℝ",
                "ℝ \\ { 0 }",
                $"ℝ \\ {{ {b + 1} }}",
                $"ℝ \\ {{ {Math.Max(0, b - 1)} }}",
                $"ℝ \\ {{ {b + 2} }}",
                $"ℝ \\ {{ {b + 3} }}",
                $"ℝ \\ {{ {b - 1} }}"
            };

            var wrongs = candidates
                .Where(w => w != answer)
                .Take(4)
                .ToList();

            return wrongs;
        }
    }
}
