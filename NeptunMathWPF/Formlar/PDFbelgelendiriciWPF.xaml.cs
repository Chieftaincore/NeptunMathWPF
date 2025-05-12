using Microsoft.Win32;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace NeptunMathWPF.Formlar
{
    /// <summary>
    /// PDFbelgelendiriciWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class PDFbelgelendiriciWPF : Window
    {

        UserControl pdflatex;

        UserControl normlpdf;

        ObservableCollection<SoruCardModel> Modeller;

        string[] turler = { "NormalPDF", "PDFlatex" };

        internal PDFbelgelendiriciWPF(ObservableCollection<SoruCardModel> _modeller)
        {
            InitializeComponent();

            Modeller = _modeller;

            cmbxPDFYineleyiciler.ItemsSource = turler;
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

        private void cmbxSeciliDegisme(object sender, SelectionChangedEventArgs e)
        {

            Genel.Handle(() =>
            {

                ComboBox obj = (ComboBox)sender;

                if ((string)obj.SelectedValue == turler[0])
                {
                    cmbxSeciliTur = pdflatex;
                }

                if ((string)obj.SelectedValue == turler[0])
                {
                    cmbxSeciliTur = normlpdf;
                }

            });
        }

        internal void TextBlockYenile()
        {

        }
    }
}
