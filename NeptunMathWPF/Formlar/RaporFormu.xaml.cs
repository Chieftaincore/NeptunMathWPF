using System.Windows;
using System.Collections.ObjectModel; // ObservableCollection için
using System.Linq; // LINQ için

namespace AnasayfaWPF
{
    /// <summary>
    /// RaporFormu.xaml etkileşim mantığı
    /// </summary>
    public partial class RaporFormu : Window
    {
        // Örnek veriler için ObservableCollection'lar (gerçek uygulamada veritabanından gelecektir)
        public ObservableCollection<string> Konular { get; set; }
        public ObservableCollection<string> Oturumlar { get; set; }

        public RaporFormu()
        {
            InitializeComponent();

            // Örnek veri yükleme (gerçek uygulamada veritabanı veya API'den gelecektir)
            Konular = new ObservableCollection<string>
            {
                "Matematik - Cebir",
                "Matematik - Geometri",
                "Fizik - Mekanik",
                "Fizik - Elektrik",
                "Kimya - Organik",
                "Kimya - İnorganik"
            };

            Oturumlar = new ObservableCollection<string>
            {
                "Oturum #1 (2024-01-15)",
                "Oturum #2 (2024-01-16)",
                "Oturum #3 (2024-01-18)",
                "Oturum #4 (2024-01-20)"
            };

            // Başlangıçta listbox'ı konularla doldurabiliriz veya boş bırakabiliriz
            // lstRaporVerileri.ItemsSource = Konular; // Varsayılan olarak konuları göster

            // Alternatif: Genel Değerlendirme ile başla
            btnGenelDegerlendirme_Click(null, null); // Form açıldığında genel değerlendirmeyi yükle
        }

        // Ana Sayfaya Geri Dön Butonu
        private void GeriDon_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Rapor formunu kapat

            // Eğer ana sayfayı ShowDialog() ile açtıysanız, tekrar göstermenize gerek yok.
            // Eğer Show() ile açtıysanız ve gizlediyseniz, burada Show() yapmanız gerekebilir:
            // if (this.Owner is AnaSayfa anaSayfa)
            // {
            //     anaSayfa.Show();
            // }
        }

        // Genel Değerlendirme Butonu Click Olayı
        private void btnGenelDegerlendirme_Click(object sender, RoutedEventArgs e)
        {
            // Bu bölümde genel bir değerlendirme grafiği ve bilgileri gösterilir.
            // Örneğin: Toplam doğru/yanlış sayısı, genel başarı oranı vb.
            // lstRaporVerileri.ItemsSource = null; // Listbox'ı temizle (isteğe bağlı)
            // Ya da genel istatistikleri ListBox'a ekleyebilirsiniz:
            lstRaporVerileri.ItemsSource = new ObservableCollection<string>
            {
                "Genel Başarı Oranı: %85",
                "Toplam Çözülen Soru: 500",
                "Doğru Cevap: 425",
                "Yanlış Cevap: 75"
            };
            // Grafik alanını güncelleme kodu buraya gelecek.
            // Örneğin: UpdateChartForGeneralEvaluation();
            MessageBox.Show("Genel Değerlendirme raporu yüklendi.");
        }

        // Konu Bazlı Buton Click Olayı
        private void btnKonuBazli_Click(object sender, RoutedEventArgs e)
        {
            // ListBox'ı Konular ile doldur
            lstRaporVerileri.ItemsSource = Konular;
            // Grafik alanını güncelleme kodu buraya gelecek (seçili konuya göre).
            // Örneğin: UpdateChartForTopicBased();
            MessageBox.Show("Konu Bazlı rapor için konular listelendi.");
        }

        // Oturum Bazlı Buton Click Olayı
        private void btnOturumBazli_Click(object sender, RoutedEventArgs e)
        {
            // ListBox'ı Oturumlar ile doldur
            lstRaporVerileri.ItemsSource = Oturumlar;
            // Grafik alanını güncelleme kodu buraya gelecek (seçili oturuma göre).
            // Örneğin: UpdateChartForSessionBased();
            MessageBox.Show("Oturum Bazlı rapor için oturumlar listelendi.");
        }

        // True/False Buton Click Olayı
        private void btnTrueFalse_Click(object sender, RoutedEventArgs e)
        {
            // Bu bölümde doğru ve yanlış cevapların dağılımını gösteren bir grafik güncellenir.
            // ListBox'a belki doğru/yanlış sayılarını ekleyebilirsiniz:
            lstRaporVerileri.ItemsSource = new ObservableCollection<string>
            {
                "Doğru Cevaplar: 425",
                "Yanlış Cevaplar: 75"
            };
            // Grafik alanını güncelleme kodu buraya gelecek.
            // Örneğin: UpdateChartForTrueFalse();
            MessageBox.Show("Doğru/Yanlış Dağılımı raporu yüklendi.");
        }

        // İPUCU: ListBox'tan bir öğe seçildiğinde grafik güncellemek isterseniz:
        // private void lstRaporVerileri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        //     if (lstRaporVerileri.SelectedItem != null)
        //     {
        //         string selectedItem = lstRaporVerileri.SelectedItem.ToString();
        //         // selectedItem'a göre grafiği güncelleme mantığı buraya gelecek.
        //     }
        // }
    }
}