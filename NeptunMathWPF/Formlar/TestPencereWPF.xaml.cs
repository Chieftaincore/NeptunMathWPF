using NeptunMathWPF.Formlar;
using NeptunMathWPF.SoruVeAjani;
using LoginApp;
using System;
using System.Collections.Generic;
using System.Windows;

namespace NeptunMathWPF
{
    /// <summary>
    /// TestPencereWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class TestPencereWPF : Window
    {
        Window onceki;

        //Mergelendi
        public TestPencereWPF()
        {
            InitializeComponent();
        }
        
        public TestPencereWPF(Window _onceki)
        {
            InitializeComponent();

            onceki = _onceki;
        }

        //HES kod
        //Yeni Latex pencere açar 
        private void LaTeXtestformuTikla(object sender, RoutedEventArgs e)
        {
            Window a = new LatexTestPencere();
            a.ShowDialog();
            //WPF'de Close() kaynakları siler Hide() ise gizler

        }

        private void BasitSoruTus(object sender, RoutedEventArgs e)
        {
            //Soru soru = SoruAjani.YerelSoruOlustur(SoruTerimleri.ifadeTurleri.sayi,seceneksayisi:7);

            List<Ifade> liste = SoruAjani.IfadeListesiOlustur(SoruTerimleri.ifadeTurleri.sayi, 4);
            Soru soru = SoruAjani.YerelSoruBirlestir(liste);
        }

        private void EtkilesimSayfasi_Click(object sender, RoutedEventArgs e)
        {
            (new EtkilesimPencereWPF(this)).Show();
            this.Hide();
        }

        private void fonksiyonSayfasi_Click(object sender, RoutedEventArgs e)
        {
            (new FunctionsWPF()).ShowDialog();
        }

        private void dbTestButton_Click(object sender, RoutedEventArgs e)
        {
            (new dbTestWPF()).ShowDialog();
        }

        private void apiTest_Click(object sender, RoutedEventArgs e)
        {
            (new ApiTest()).ShowDialog();
        }

        private void devPanelButton_Click(object sender, RoutedEventArgs e)
        {
            (new DevPanelWPF()).ShowDialog();
        }

        private void ButonLimit_Click(object sender, RoutedEventArgs e)
        {
            LimitPanel limitPanel = new LimitPanel();
            limitPanel.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.onceki == null)
            {
                Genel.UygulamaKapat();
            }
            else
            {
                onceki.Show();
                this.Close();

            }
        }

        private void FormlarAc_Click(object sender, RoutedEventArgs e)
        {
            new LoginForm().ShowDialog();
           
        }
    }
}
