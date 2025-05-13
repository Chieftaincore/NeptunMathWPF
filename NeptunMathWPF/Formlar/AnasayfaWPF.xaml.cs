
using System.Windows;
using LoginApp;
using NeptunMathWPF;
using NeptunMathWPF.Formlar; // LoginApp namespace'ini ekledik

namespace AnasayfaWPF
{
    public partial class AnaSayfa : Window
    {
        public AnaSayfa()
        {
            InitializeComponent();
        }

        private void tusIstatistik(object sender, RoutedEventArgs e)
        {
            new IstatistikWPF().Show();
        }

        private void tusDebugPenceresi(object sender, RoutedEventArgs e)
        {
            new TestPencereWPF(this).ShowDialog(); 
        }

        private void tusDevPanel(object sender, RoutedEventArgs e)
        {
            new DevPanelWPF().ShowDialog();
        }

        private void StandartEtkilesimSayfasiAc(object sender, RoutedEventArgs e)
        {
            new EtkilesimPencereWPF().ShowDialog();
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