using NeptunMathWPF.Formlar;
using NeptunMathWPF.SoruVeAjani;
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

namespace NeptunMathWPF
{
    /// <summary>
    /// TestPencereWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class TestPencereWPF : Window
    {
        //Mergelendi
        public TestPencereWPF()
        {
            InitializeComponent();
        }

        //HES kod
        //Yeni Latex pencere açar 
        private void LaTeXtestformuTikla(object sender, RoutedEventArgs e)
        {
            (new LatexTestPencere()).Show();
            //WPF'de Close() kaynakları siler Hide() ise gizler
            this.Close();
        }

        private void BasitSoruTus(object sender, RoutedEventArgs e)
        {
            //Soru soru = SoruAjani.YerelSoruOlustur(SoruTerimleri.ifadeTurleri.sayi,seceneksayisi:7);

            List<Ifade> liste = SoruAjani.IfadeListesiOlustur(SoruTerimleri.ifadeTurleri.sayi,4);
            Soru soru = SoruAjani.YerelSoruBirlestir(liste);
        }

        private void SoruTestPaneliTikla(object sender, RoutedEventArgs e)
        {
            (new SoruNesneTestWPF()).Show();
            this.Close();
        }

        private void EtkilesimSayfasi_Click(object sender, RoutedEventArgs e)
        {
            (new EtkilesimPencereWPF()).Show();
            this.Close();
        }

        private void fonksiyonSayfasi_Click(object sender, RoutedEventArgs e)
        {
            (new FunctionsWPF()).Show();
            this.Close();
        }

        private void dbTestButton_Click(object sender, RoutedEventArgs e)
        {
            (new dbTestWPF()).Show();
            this.Close();
        }

        private void apiTest_Click(object sender, RoutedEventArgs e)
        {
            (new ApiTest()).Show();
            this.Close();
        }

        private void devPanelButton_Click(object sender, RoutedEventArgs e)
        {
            (new DevPanelWPF()).Show();
            this.Close();
        }

        private void ButonLimit_Click(object sender, RoutedEventArgs e)
        {
            LimitPanel limitPanel = new LimitPanel();
            limitPanel.Show();
        }
    }
}
