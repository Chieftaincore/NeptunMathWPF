using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NeptunMathWPF.Formlar
{
    /// <summary>
    /// Interaction logic for kullaniciadiDegisPanelWPF.xaml
    /// </summary>
    public partial class kullaniciadiDegisPanelWPF : Window
    {
        public kullaniciadiDegisPanelWPF()
        {
            InitializeComponent();
        }

        private void kullaniciAdiDegisButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(yeniKullaniciAdiTextBox.Text) && !string.IsNullOrEmpty(sifreOnayPasswordBox.Password))
            {
                if (sifreOnayPasswordBox.Password == aktifKullanici.sifre)
                {

                    Genel.Handle(() =>
                    {
                        Genel.ReloadEntity();
                        var kullanici = Genel.dbEntities.USERS.Where(x => x.USERID == aktifKullanici.kullnId).FirstOrDefault();
                        kullanici.USERNAME = yeniKullaniciAdiTextBox.Text;
                        Genel.dbEntities.SaveChanges();
                        aktifKullanici.kullaniciAdi = kullanici.USERNAME;
                        MessageBox.Show("Kullanıcı adı başarıyla değiştirildi.", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                }
                else
                {
                    MessageBox.Show("Şifrenizi doğru giriniz!", "UYARI", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Boş bıraktığınız alanlar var!", "UYARI", MessageBoxButton.OK, MessageBoxImage.Warning); 
            }
        }
    }
}
