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
    /// Interaction logic for dbTestWPF.xaml
    /// </summary>
    public partial class dbTestWPF : Window
    {
        public dbTestWPF()
        {
            InitializeComponent();

            NeptunDBEntities dbEntities = new NeptunDBEntities();
            dbTestDataGrid.ItemsSource = dbEntities.LoginKull.ToList();
        }
    }
}
