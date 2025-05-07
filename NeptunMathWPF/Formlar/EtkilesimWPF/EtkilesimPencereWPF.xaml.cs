using HesapMakinesi;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NeptunMathWPF.Formlar
{
    /// <summary>
    /// Genellikle Pencereyi kapsayan tuşlar veya MVVM dışı eventler içindir
    /// formun diğer özellikleri için EtkilesimMVVM'e bakınız
    /// </summary>
    /// 
    public partial class EtkilesimPencereWPF : Window
    {
        Window Onceki;
        public EtkilesimPencereWPF()
        {
            InitializeComponent();
        }

        public EtkilesimPencereWPF(Window _onceki)
        {
            InitializeComponent();

            Onceki = _onceki;
        }

        private void HesapMakinesi_Click(object sender, RoutedEventArgs e)
        {
            HesapMakinesiWindow hesapMakinesiWindow = new HesapMakinesiWindow();
            hesapMakinesiWindow.Show();
        }

        private void YerelHesapMakinesi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("calc.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yerel hesap makinesi açılırken hata oluştu: " + ex.Message);
            }
        }

        private void NotAlma_Click(object sender, RoutedEventArgs e)
        {
            NotAlmaWindow notAlmaWindow = new NotAlmaWindow();
            notAlmaWindow.Show();
        }

        private void PenecereyiKapat(object sender, EventArgs e)
        {
            if (Onceki != null)
            {
                Onceki.Show();
            }
            else
            {
                Genel.UygulamaKapat();
            }
        }

        private void DialogScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
