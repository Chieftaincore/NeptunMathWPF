using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM;
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
    /// EtkilesimPencereWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class EtkilesimPencereWPF : Window
    {
       
        public EtkilesimPencereWPF()
        {

            InitializeComponent();

            var viewModel = new EtkilesimMVM();
            this.DataContext = viewModel;

            if (viewModel.Sorular?.Count == 0)
            {
                MessageBox.Show("No items in Sorular!");
            }
        }
    }
}
