using NeptunMathWPF;
using NeptunMathWPF.Formlar;
using System;
using System.IO;
using System.Linq;
using System.Windows;

namespace LoginApp
{
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
            CheckGeminiConfig();

            if (!File.Exists(Genel.dbPath))
            {
                string currentDir = AppDomain.CurrentDomain.BaseDirectory;
                string sourceDb = Path.Combine(currentDir, "NEPTUN_DB.mdf");

                Directory.CreateDirectory(Genel.dbDirectory);
                File.Copy(sourceDb, Genel.dbPath);
            }
        }

        private void GirisYap_Click(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                string kullaniciAdi = txtKullaniciAdi.Text.Trim();
                string sifre = txtSifre.Password.Trim();

                if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
                {
                    MessageBox.Show("Lütfen kullanıcı adı ve şifreyi giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var kullanici = Genel.dbEntities.USERS.Where(x => x.USERNAME == kullaniciAdi && x.PASSWORD == sifre).FirstOrDefault();

                // Basit bir örnek giriş kontrolü
                if (kullanici != null)
                {
                    MessageBox.Show("Giriş başarılı!", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

                    Genel.ReloadEntity();
                    aktifKullanici.kullnId = kullanici.USERID;
                    aktifKullanici.kullaniciAdi = kullanici.USERNAME;
                    aktifKullanici.isim = kullanici.FIRST_NAME;
                    aktifKullanici.soyisim = kullanici.LAST_NAME;
                    aktifKullanici.email = kullanici.E_MAIL;
                    aktifKullanici.sifre = kullanici.PASSWORD;
                    aktifKullanici.yetki = kullanici.USER_ROLE;

                    AnasayfaWPF.AnaSayfa anaSayfa = new AnasayfaWPF.AnaSayfa();
                    anaSayfa.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void KayitOl_Click(object sender, RoutedEventArgs e)
        {
            AnasayfaWPF.KayitFormu kayitFormu = new AnasayfaWPF.KayitFormu();
            kayitFormu.Show();
            // İsteğe bağlı: Giriş formunu kapatmak isterseniz aşağıdaki satırı etkinleştirin
            // this.Close();
        }

        private void CheckGeminiConfig()
        {


            if (!File.Exists(Genel.geminiFilePath))
            {
                new GetAPIKeyPanelWPF().ShowDialog();   
            }
        }
    }
}