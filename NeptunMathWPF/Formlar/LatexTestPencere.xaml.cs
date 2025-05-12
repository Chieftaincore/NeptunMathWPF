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
    /// LatexTestPencere.xaml etkileşim mantığı
    /// </summary>
    public partial class LatexTestPencere : Window
    {
        Window onceki;

        public LatexTestPencere()
        {
            InitializeComponent();
        } 
        
        public LatexTestPencere(Window _onceki)
        {
            InitializeComponent();

            onceki = _onceki;
        }

        private void tusCevir_Click(object sender, RoutedEventArgs e)
        {
            //Try Catch düzenlenecek
            try
            {
                string LatexYazi = LatexGirdi.Text;
                LatexCikti.Formula = LatexYazi;
            }
            catch
            {
                MessageBox.Show("Hata", "Latex Hatası");
            }
        }

       
    }
}
