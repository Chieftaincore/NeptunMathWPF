using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Fonksiyonlar
{
    public class Question
    {
        public string QuestionText { get; set; }
        public string LatexText { get; set; }
        public string Answer { get; set; }
        public List<string> WrongAnswers { get; set; }
    }
}
