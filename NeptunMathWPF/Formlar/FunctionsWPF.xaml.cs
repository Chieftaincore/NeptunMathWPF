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
using DataVis = System.Windows.Forms.DataVisualization;

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
            Chart1.Series[0].Points.Add(3, 0).AxisLabel = "asd";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lblWrongs.Content = "";
            GeneratedFunction function = new GeneratedFunction();

            foreach (var item in function.repository)
            {
                lblQuestion.Content = item.questionObject.QuestionText;
                lblAnswer.Content = item.questionObject.Answer;
                var a = item.questionObject.WrongAnswers;
                foreach (var answer in a)
                {
                    lblWrongs.Content += answer + "\n";
                }
            }

        }
    }
}
