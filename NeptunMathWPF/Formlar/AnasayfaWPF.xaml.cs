using AnaSayfaForm;
using System.Windows;

namespace AnasayfaWPF
{
    public partial class AnaSayfa : Window
    {
        public AnaSayfa()
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

        private void EtkilesimAc(object sender, RoutedEventArgs e)
        {

        }

        private void OturumuKapat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AyarlarFormu_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}