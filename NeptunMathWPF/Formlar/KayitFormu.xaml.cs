using System.Windows;

namespace AnasayfaWPF
{
    public partial class KayitFormu : Window
    {
        public KayitFormu()
        {
            InitializeComponent();
        }

        private void KayitOlButton_Click(object sender, RoutedEventArgs e)
        {
            string kullaniciAdi = KayitKullaniciAdiTxt.Text.Trim();
            string sifre = KayitSifreTxt.Password.Trim();
            string sifreTekrar = KayitSifreTekrarTxt.Password.Trim();
            string eposta = KayitEpostaTxt.Text.Trim();
            string isim = KayitIsimTxt.Text.Trim();
            string soyisim = KayitSoyisimTxt.Text.Trim();

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre) || string.IsNullOrEmpty(eposta) || string.IsNullOrEmpty(isim) || string.IsNullOrEmpty(soyisim))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (sifre != sifreTekrar)
            {
                MessageBox.Show("Şifreler eşleşmiyor!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Burada kullanıcı bilgilerini veritabanına kaydetme işlemleri yapılmalı
            MessageBox.Show($"Kayıt Başarılı!\nKullanıcı Adı: {kullaniciAdi}\nE-posta: {eposta}\nİsim: {isim} {soyisim}", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close(); // Kayıt başarılı olduktan sonra formu kapat
        }
    }
}