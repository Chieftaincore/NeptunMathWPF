
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using LoginApp;
using NeptunMathWPF;
using NeptunMathWPF.Formlar; // LoginApp namespace'ini ekledik

namespace AnasayfaWPF
{
    public partial class AnaSayfa : Window
    {

        public List<SoruTerimleri.soruTuru> OzelSessionTurler = new List<SoruTerimleri.soruTuru>();

        public AnaSayfa()
        {
            InitializeComponent();

            cmbxSoruTurler.Items.Add(SoruTerimleri.soruTuru.islem);
            cmbxSoruTurler.Items.Add(SoruTerimleri.soruTuru.problem);
            cmbxSoruTurler.Items.Add(SoruTerimleri.soruTuru.limit);
            cmbxSoruTurler.Items.Add(SoruTerimleri.soruTuru.fonksiyon);

            listKonu.ItemsSource = OzelSessionTurler;

            if(aktifKullanici.yetki == "admin")
            {
                tusAdminDebug.Visibility = Visibility.Visible;
                tusAdminGelistirici.Visibility = Visibility.Visible;
            }
            else
            {
                tusAdminDebug.Visibility = Visibility.Collapsed;
                tusAdminGelistirici.Visibility = Visibility.Collapsed;
            }

            KullaniciBilgileri();
        }

        private void KullaniciBilgileri()
        {
            kKullaniciAdi.Content = $"@{aktifKullanici.kullaniciAdi}";
            kAd.Content = $"İsim : {aktifKullanici.isim}";
            kSoyAd.Content = $"Soyisim : {aktifKullanici.soyisim}";
            kYetki.Content = $"Yetki : {aktifKullanici.yetki}";
            kEmail.Content = $"E-posta : {aktifKullanici.email}";
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

        private void tusStandartSession(object sender, RoutedEventArgs e)
        {
            (new EtkilesimPencereWPF(this)).Show();
            this.Hide();
        }

        private void tusOzelSession(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (listKonu.Items.Count > 0 && !OzelSessionTurler.Contains(SoruTerimleri.soruTuru.problem))
                {
                    new EtkilesimPencereWPF(this, OzelSessionTurler.ToArray()).Show();
                    this.Hide();

                    return;
                }

                if (listKonu.Items.Count > 1 && OzelSessionTurler.Contains(SoruTerimleri.soruTuru.problem))
                {
                    new EtkilesimPencereWPF(this, OzelSessionTurler.ToArray()).Show();
                    this.Hide();

                    return;
                }
                else
                {
                    MessageBox.Show("Problemler Yapay Zeka API tarafından oluşturulduğundan lütfen başka bir konu daha ekleyiniz","Sadece problemler!", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            });
        }

        private void tusKonuEkle(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                var ek = cmbxSoruTurler.SelectedItem;

                SoruTerimleri.soruTuru _tur;
                Enum.TryParse(ek.ToString(), out _tur);

                if (ek != null && !listKonu.Items.Contains(_tur))
                {
                   
                    OzelSessionTurler.Add(_tur);
                }

                listKonu.Items.Refresh();
            });
        }

        private void listKonu_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (e.Key == Key.Delete)
                {
                    if (listKonu.SelectedItem != null && listKonu.SelectedItems.Count < 2)
                    {
                        OzelSessionTurler.RemoveAt(listKonu.SelectedIndex);
                        listKonu.Items.Refresh();
                    }
                }
            });
        }

        private void tusKaydedilenler(object sender, RoutedEventArgs e)
        {
           
        }
    }
}