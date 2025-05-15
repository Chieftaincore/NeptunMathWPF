using System.Windows;

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
