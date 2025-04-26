using System.Windows;


namespace LoginApp
{
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void GirisYap_Click(object sender, RoutedEventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Password.Trim();

            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi giriniz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Basit bir örnek giriş kontrolü
            if (kullaniciAdi == "admin" && sifre == "123")
            {
                MessageBox.Show("Giriş başarılı!", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);

                AnasayfaWPF.AnaSayfa anaSayfa = new AnasayfaWPF.AnaSayfa();
                
                // Örnek: Ana pencereye geçiş
                // MainWindow mainWindow = new MainWindow();
                // mainWindow.Show();
                // this.Close();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
