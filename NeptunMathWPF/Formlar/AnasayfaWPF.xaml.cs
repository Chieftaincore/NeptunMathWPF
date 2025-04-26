using AnaSayfaForm;
using System.Windows;
using LoginApp; // LoginApp namespace'ini ekledik

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
            SoruFormu soruFormu = new SoruFormu();
            soruFormu.Show();
            MessageBox.Show("Sorulara sayfasına gidiliyor");
        }

        private void SoruUreticiButton_Click(object sender, RoutedEventArgs e)
        {
            SoruSec soruSec = new SoruSec();
            soruSec.Show();
            MessageBox.Show("Soru üretici sayfasına gidiliyor");
        }

        private void EtkilesimAc(object sender, RoutedEventArgs e)
        {
            // Etkileşim açma işlemleri
        }

        private void OturumuKapat_Click(object sender, RoutedEventArgs e)
        {
            // Giriş formunu aç
            LoginForm loginForm = new LoginForm();
            loginForm.Show();

            // Ana sayfayı kapat
            this.Close();
        }

        private void AyarlarFormu_Click(object sender, RoutedEventArgs e)
        {
            // Ayarlar formunu aç
            AyarlarFormu ayarlarFormu = new AyarlarFormu();
            ayarlarFormu.ShowDialog(); // ShowDialog, ayarlar formu kapanana kadar ana sayfanın etkileşimini engeller
        }
    }
}