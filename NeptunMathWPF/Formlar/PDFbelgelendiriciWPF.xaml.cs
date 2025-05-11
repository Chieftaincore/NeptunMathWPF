using Microsoft.Win32;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// PDFbelgelendiriciWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class PDFbelgelendiriciWPF : Window
    {

        ObservableCollection<SoruCardModel> Modeller;

        internal PDFbelgelendiriciWPF(ObservableCollection<SoruCardModel> _modeller)
        {
            InitializeComponent();

            Modeller = _modeller;
        }

        private void tusPDFCikart(object sender, RoutedEventArgs e)
        {

            Genel.Handle(() =>
            {
                if (MessageBox.Show("pdflatex tarafından dosya çıkarılması için bilgisayarınızda standart LatexLive kütüphanesi veya pdflatex komutları taşıyan bir TeX motoru bulunmalıdır", "Gereksinim Bildirisi", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    FileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Klasör | Directory",
                        Title = "Kaydetmek için klasör yeri ve ismi (Ek dosyalar eklenebilir)",
                        FileName = $"LatexPDF{DateTime.Now:yyyyMMdd_HHmmss}",

                    };


                    if (saveFileDialog.ShowDialog() == true)
                    {
                        new PDFLatexYineleyici().LaTeXPDFolustur(Modeller, saveFileDialog.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Seçim yapılmadı", "Geçersiz Dosya Seçimi", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            });

        }
    }
}
