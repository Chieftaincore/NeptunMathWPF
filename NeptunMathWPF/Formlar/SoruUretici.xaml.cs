using System.Windows;

namespace AnaSayfaForm
{
    public partial class SoruFormu : Window
    {
        public SoruFormu()
        {
            InitializeComponent();
        }

        private void GonderButton_Click(object sender, RoutedEventArgs e)
        {
            // Kullanıcının girdiği soruyu al
            string soru = SoruTextBox.Text;

            // Soruyu işleme veya kaydetme kodları buraya gelecek
            MessageBox.Show("Gönderilen Soru: " + soru);

            // Formu kapat
            this.Close();
        }
    }
}