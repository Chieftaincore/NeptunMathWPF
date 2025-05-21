using NeptunMathWPF;
using System;
using System.Linq;
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
            Genel.Handle(() =>
            {

                string kullaniciAdi = KayitKullaniciAdiTxt.Text.Trim();
                string sifre = KayitSifreTxt.Password.Trim();
                string sifreTekrar = KayitSifreTekrarTxt.Password.Trim();
                string eposta = KayitEpostaTxt.Text.Trim();
                string isim = KayitIsimTxt.Text.Trim();
                string soyisim = KayitSoyisimTxt.Text.Trim();
                string yetki = "Kullanıcı";

                if(Genel.dbEntities.USERS.Where(x=>x.USERNAME==kullaniciAdi).Count()!=0)
                {
                    MessageBox.Show("Bu kullanıcı adı zaten kayıtlı!", "UYARI!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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

                var Eposta = KayitEpostaTxt.Text.Trim();

                if (Eposta.EndsWith("."))
                {
                    MessageBox.Show("Eposta : sonu doğru bitmiyor", "Metin Kutusu Eksiği!", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                try
                {
                    var addr = new System.Net.Mail.MailAddress(KayitEpostaTxt.Text);

                    bool dogru = addr.Address == Eposta;

                    if (!dogru)
                    {
                        MessageBox.Show($"hatalı Eposta yazıldı", "Metin Kutusu Eksiği!", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Eposta Doğrulmada hata", "Metin Kutusu Eksiği!", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                Genel.ReloadEntity();
                Genel.dbEntities.USERS.Add(new USERS
                {
                    USERNAME = kullaniciAdi,
                    FIRST_NAME = isim,
                    LAST_NAME = soyisim,
                    E_MAIL = eposta,
                    PASSWORD = sifre,
                    USER_ROLE = yetki
                });

                Genel.dbEntities.SaveChanges();
                MessageBox.Show($"Kayıt Başarılı!\nKullanıcı Adı: {kullaniciAdi}\nE-posta: {eposta}\nİsim: {isim} {soyisim}", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close(); // Kayıt başarılı olduktan sonra formu kapat
            });
        }
    }
}