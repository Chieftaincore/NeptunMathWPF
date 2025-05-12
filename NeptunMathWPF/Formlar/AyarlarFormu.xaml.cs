using System.Windows;

namespace AnasayfaWPF
{
    public partial class AyarlarFormu : Window
    {
        public AyarlarFormu()
        {
            InitializeComponent();
        }

        private void KaydetButton_Click(object sender, RoutedEventArgs e)
        {
            // Ayarları kaydetme işlemleri burada yapılacak
            MessageBox.Show("Ayarlar kaydedildi.");
            this.Close();
        }

        private void KullaniciAdiSifirla_Click(object sender, RoutedEventArgs e)
        {
            // Kullanıcı adı sıfırlama işlemleri burada yapılacak
            MessageBox.Show("Kullanıcı adı sıfırlama işlemi başlatıldı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SifreSifirla_Click(object sender, RoutedEventArgs e)
        {
            // Şifre sıfırlama işlemleri burada yapılacak
            MessageBox.Show("Şifre sıfırlama işlemi başlatıldı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SoruProfiliSifirla_Click(object sender, RoutedEventArgs e)
        {
            // Soru profili sıfırlama işlemleri burada yapılacak
            MessageBox.Show("Soru profili sıfırlama işlemi başlatıldı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
