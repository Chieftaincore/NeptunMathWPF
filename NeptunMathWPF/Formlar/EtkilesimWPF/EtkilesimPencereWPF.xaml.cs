using HesapMakinesi;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM;
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
    /// EtkilesimPencereWPF.xaml etkileşim mantığı
    /// </summary>
    /// 

    //Bu Formun özellikleri ve fonksiyonelliği için EtkilesimMVVM'e bakınız
    public partial class EtkilesimPencereWPF : Window
    {
       
        public EtkilesimPencereWPF()
        {
            InitializeComponent();
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
    }
}
