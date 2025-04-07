using System.Windows;

namespace AnaSayfaForm
{
    public partial class SoruSec : Window
    {
        public SoruSec()
        {
            InitializeComponent();
        }

        private void MatematikSorulariButton_Click(object sender, RoutedEventArgs e)
        {
            // Matematik soruları sayfasına yönlendirme kodları buraya gelecek
            MessageBox.Show("Matematik soruları sayfasına gidiliyor...");
        }

        private void TarihSorulariButton_Click(object sender, RoutedEventArgs e)
        {
            // Tarih soruları sayfasına yönlendirme kodları buraya gelecek
            MessageBox.Show("Tarih soruları sayfasına gidiliyor...");
        }

        private void CografyaSorulariButton_Click(object sender, RoutedEventArgs e)
        {
            // Coğrafya soruları sayfasına yönlendirme kodları buraya gelecek
            MessageBox.Show("Coğrafya soruları sayfasına gidiliyor...");
        }
    }
}