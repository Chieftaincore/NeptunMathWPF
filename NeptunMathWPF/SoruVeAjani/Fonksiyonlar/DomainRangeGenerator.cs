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
            var (question, latex, func, parameters, type, denominator, denomsign) = GetRandomFunction();

            string domainAnswer;
            switch (type)
            {
                case FunctionType.Linear:
                case FunctionType.Quadratic:
                case FunctionType.Absolute:
                case FunctionType.Exponential:
                    domainAnswer = $"R"; // Tüm gerçel sayılar
                    break;

                case FunctionType.Root:
                    {
                        int a = parameters[0], b = parameters[1];
                        if (-b / (double)a != Math.Floor(-b / (double)a))
                        {
                            domainAnswer = a > 0
                                ? $"[ {-b}/{a}, \\infty )"
                                : $"( -\\infty, {-b}/{a} ]";
                        }
                        else
                        {
                            domainAnswer = a > 0
                                ? $"[ {-b / a}, \\infty )"
                                : $"( -\\infty, {-b / a} ]";
                        }
                    }
                    break;

                case FunctionType.Rational:
                    {
                        int c = denominator;
                        domainAnswer = denomsign == "-" ? $"R - \\text{{{{{c}}}}} " : $"R - \\text{{{{{-c}}}}} ";
                    }
                    break;

                default:
                    domainAnswer = $"R";
                    break;
            }

            var q = new Question
            {
                QuestionText = $"{question} fonksiyonunun tanım kümesi nedir?",
                LatexText = $"{latex} \\text{{ fonksiyonunun tanım kümesi nedir?}}",
                Answer = domainAnswer,
                WrongAnswers = GenerateAnswer(domainAnswer, parameters[0], parameters[1])
            };

            return new List<FunctionRepository>
            {
                new FunctionRepository
                {
                    question = question,
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
                $"R",
                $"R - \\text{{{{{b+1}}}}}",
                $"R - \\text{{{{{b+2}}}}}",
                $"R - \\text{{{{{b-1}}}}}",
                $"R - \\text{{{{ {b + 3} }}}}",
                $"R - \\text{{{{ {b - 2} }}}}"
            };

            List<string> candidatesList = candidates.ToList();
            Genel.Shuffle(candidatesList);

            var wrongs = candidatesList
                .Where(w => w != answer)
                .Take(4)
                .ToList();

            return wrongs;
        }
    }
}
