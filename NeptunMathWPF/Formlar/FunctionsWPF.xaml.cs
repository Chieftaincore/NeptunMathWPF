using NeptunMathWPF.Fonksiyonlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeptunMathWPF.Formlar
{
    /// <summary>
    /// Interaction logic for FunctionsWPF.xaml
    /// </summary>
    public partial class FunctionsWPF : Window
    {
        public FunctionsWPF()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<FunctionQuestionGenerator> generators = new List<FunctionQuestionGenerator>
        {
            new FunctionValueGenerator(),
            new FunctionCompositionGenerator(),
            new InverseFunctionGenerator(),
            new DomainRangeGenerator()
        };
            var generator = generators[new Random().Next(generators.Count)];
            var question = generator.GenerateQuestion();

            lblQuestion.Content = question.QuestionText;
            lblAnswer.Content = question.Answer;
        }
    }
}
