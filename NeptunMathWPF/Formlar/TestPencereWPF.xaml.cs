using LoginApp;
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

namespace NeptunMathWPF
{
    /// <summary>
    /// TestPencereWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class TestPencereWPF : Window
    {
        //Mergelendi
        public TestPencereWPF()
        {
            InitializeComponent();
        }

        //HES kod
        //Yeni Latex pencere açar 
        private void LaTeXtestformuTikla(object sender, RoutedEventArgs e)
        {
            (new LatexTestPencere()).Show();
            //WPF'de Close() kaynakları siler Hide() ise gizler
            this.Close();
        }

        private void BasitSoruTus(object sender, RoutedEventArgs e)
        {
            new Soru(Soru.Turler.islem, 4);
        }

        private void FormlarAc_Click(object sender, RoutedEventArgs e)
        {
            new LoginForm().Show();
            this.Close();
        }
    }
}
