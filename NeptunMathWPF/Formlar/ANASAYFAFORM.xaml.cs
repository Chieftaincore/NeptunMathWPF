using System.Windows;

namespace AnaSayfaForm
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SoruButton_Click(object sender, RoutedEventArgs e)
        {
            // SoruFormu penceresini aç
            SoruFormu soruFormu = new SoruFormu();
            soruFormu.Show();
            MessageBox.Show("Sorulara sayfasına gidiliyor");
        }

        private void SoruUreticiButton_Click(object sender, RoutedEventArgs e)
        {
            // Soru üretici sayfasına yönlendirme kodları buraya gelecek
            SoruSec soruSec = new SoruSec();
            soruSec.Show();
            MessageBox.Show("Soru üretici sayfasına gidiliyor");
        }
    }
}