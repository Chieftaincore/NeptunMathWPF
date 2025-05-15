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

namespace NeptunMathWPF.Formlar.EtkilesimWPF
{
    /// <summary>
    /// Interaction logic for sifreDegisPanelWPF.xaml
    /// </summary>
    public partial class sifreDegisPanelWPF : Window
    {
        public sifreDegisPanelWPF()
        {
            InitializeComponent();
        }
        private void sifreDegisButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(eskiSifreTextBox.Password) && !string.IsNullOrEmpty(yeniSifreTextBox.Password))
            {
                if (eskiSifreTextBox.Password == aktifKullanici.sifre)
                {
                    if (tekrarSifreTextBox.Password == yeniSifreTextBox.Password)
                    {
                        Genel.Handle(() =>
                        {
                            Genel.ReloadEntity();
                            var kullanici = Genel.dbEntities.USERS.Where(x => x.USERID == aktifKullanici.kullnId).FirstOrDefault();
                            kullanici.PASSWORD = yeniSifreTextBox.Password;
                            Genel.dbEntities.SaveChanges();
                            aktifKullanici.sifre = kullanici.PASSWORD;
                            MessageBox.Show("Şifre başarıyla değiştirildi.", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                    else
                    {
                        MessageBox.Show("Yeni Şifreleriniz Eşleşmiyor", "UYARI", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
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
