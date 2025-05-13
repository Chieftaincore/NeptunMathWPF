using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.ObjectModel;
using System.IO;
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
                if ((string)cmbxPDFYineleyiciler.SelectedValue == turler[1])
                {
                    latexPDFCikart();
                }
                else
                {
                    if ((string)cmbxPDFYineleyiciler.SelectedValue == turler[0])
                    {
                        normalPDFCikart();
                    }
                }
            });
        }


        /// <summary>
        /// Latexpdf çıkarıcı Huseyin tarafından yapıldı
        /// Çalışması için bilgisayarda latexpdf olan bir TeX motoru gereklidir
        /// </summary>
        internal void latexPDFCikart()
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
        
        /// <summary>
        /// Normal PDF çıkarıcı Batın' tarafından yapuldı
        /// </summary>
        internal void normalPDFCikart()
        {
            // SaveFileDialog ile dosya adı ve konum seçimi
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "PDF Dosyasını Kaydet",
                FileName = "Sorular.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string pdfPath = saveFileDialog.FileName;

                // PDF belgesi oluştur
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
                document.Open();

                // Sorular koleksiyonunu alın
                var sorular = Modeller;

                if (sorular != null)
                {
                    foreach (var soru in sorular)
                    {
                        // Soruyu ekle
                        // Özellikle itexSharp.text yazması lazım
                        document.Add(new iTextSharp.text.Paragraph($"Soru: {soru.soru.GetMetin()}"));

                        // Seçenekleri ekle
                        if (soru.NesneSecenekler != null)
                        {
                            var secenekler = soru.NesneSecenekler.secenekler;
                            for (int i = 0; i < secenekler.Count; i++)
                            {
                                // 'a', 'b', 'c', ... için
                                char secenekHarf = (char)('a' + i);
                                document.Add(new iTextSharp.text.Paragraph($"{secenekHarf}) {secenekler[i]}"));
                            }
                        }

                        // Boşluk ekle
                        document.Add(new iTextSharp.text.Paragraph("\n"));

                    }
                }

                document.Close();

                // Kullanıcıya bilgi ver
                MessageBox.Show($"Sorular PDF dosyasına aktarıldı: {pdfPath}");
            }
        }

        private void cmbxSeciliDegisme(object sender, SelectionChangedEventArgs e)
        {

            Genel.Handle(() =>
            {
                ComboBox obj = (ComboBox)sender;

                if ((string)obj.SelectedValue == turler[1])
                {
                    cmbxSeciliTur = pdflatex;
                }
                else
                {
                    if ((string)obj.SelectedValue == turler[0])
                    {
                        cmbxSeciliTur = normlpdf;
                    }
                }
            });
        }

        internal void TextBlockYenile()
        {

        }
    }
}
