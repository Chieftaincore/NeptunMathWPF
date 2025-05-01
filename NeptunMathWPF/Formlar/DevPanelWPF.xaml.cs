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
    /// Interaction logic for DevPanelWPF.xaml
    /// </summary>
    public partial class DevPanelWPF : Window
    {
        public DevPanelWPF()
        {
            InitializeComponent();
        }

        private void TopicsPanel_Click(object sender, RoutedEventArgs e)
        {
            content1.Content = new konuEkleUC();
        }

        private void QPoolPanel_Click(object sender, RoutedEventArgs e)
        {
            content1.Content = new soruHavuzuUC();
        }
    }
}
