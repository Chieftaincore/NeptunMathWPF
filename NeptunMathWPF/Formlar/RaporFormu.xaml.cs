using System.Windows;
using System.Collections.ObjectModel; // ObservableCollection için
using System.Linq; // LINQ için
using DataVis = System.Windows.Forms.DataVisualization;

namespace AnasayfaWPF
{
    /// <summary>
    /// RaporFormu.xaml etkileşim mantığı
    /// </summary>
    public partial class RaporFormu : Window
    {
        public ObservableCollection<string> Konular { get; set; }
        public ObservableCollection<string> Oturumlar { get; set; }

        public RaporFormu()
        {
            InitializeComponent();

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


            btnGenelDegerlendirme_Click(null, null); 
        }

        private void GeriDon_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 


        }

        private void btnGenelDegerlendirme_Click(object sender, RoutedEventArgs e)
        {

            lstRaporVerileri.ItemsSource = new ObservableCollection<string>
            {
                "Genel Başarı Oranı: %85",
                "Toplam Çözülen Soru: 500",
                "Doğru Cevap: 425",
                "Yanlış Cevap: 75"
            };

            Chart1.Series[0].Points.Add(3.2, 4).AxisLabel = "DENEME";
        }


        private void btnKonuBazli_Click(object sender, RoutedEventArgs e)
        {

            lstRaporVerileri.ItemsSource = Konular;

            MessageBox.Show("Konu Bazlı rapor için konular listelendi.");
        }

        private void btnOturumBazli_Click(object sender, RoutedEventArgs e)
        {

            lstRaporVerileri.ItemsSource = Oturumlar;

            MessageBox.Show("Oturum Bazlı rapor için oturumlar listelendi.");
        }

        private void btnTrueFalse_Click(object sender, RoutedEventArgs e)
        {

            lstRaporVerileri.ItemsSource = new ObservableCollection<string>
            {
                "Doğru Cevaplar: 425",
                "Yanlış Cevaplar: 75"
            };

            MessageBox.Show("Doğru/Yanlış Dağılımı raporu yüklendi.");
        }

    }
}