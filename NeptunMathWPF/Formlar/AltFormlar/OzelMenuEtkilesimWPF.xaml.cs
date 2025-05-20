using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM;
using NeptunMathWPF.SoruVeAjani.Algorithma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NeptunMathWPF.Formlar.AltFormlar
{
    /// <summary>
    /// OzelEtkilesimSinavWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class OzelMenuEtkilesimWPF : Window
    {

        private static readonly Regex _redler = new Regex("[^0-9.-]+");

        public List<SoruTerimleri.soruTuru> OzelSessionTurler = new List<SoruTerimleri.soruTuru>();

        double _dakika;

        int _soruSayisi;

        Window Onceki;

        string baslik;

        public OzelMenuEtkilesimWPF(Window _onceki)
        {
            InitializeComponent();

            cmbxSoruTurler.Items.Add(SoruTerimleri.soruTuru.islem);
            cmbxSoruTurler.Items.Add(SoruTerimleri.soruTuru.limit);
            cmbxSoruTurler.Items.Add(SoruTerimleri.soruTuru.fonksiyon);

            listKonu.ItemsSource = OzelSessionTurler;

            Onceki = _onceki;
        }

        private void KonuEkle(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                var ek = cmbxSoruTurler.SelectedItem;

                SoruTerimleri.soruTuru _tur;
                Enum.TryParse(ek.ToString(), out _tur);

                if (ek != null && !listKonu.Items.Contains(_tur))
                {
                    OzelSessionTurler.Add(_tur);
                }

                listKonu.Items.Refresh();
            });
        }

        private void SessionAc(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (Kosullar() == false)
                    return;

                if (string.IsNullOrEmpty(txtboxBaslik.Text))
                {
                    baslik = $"SınavSession{DateTime.Now.ToString()}";
                }
                else
                {
                    baslik = txtboxBaslik.Text;
                }

                if (Genel.dbEntities.EXAM_SESSIONS.Where(x => x.EXAM_TITLE == baslik && x.USERID == aktifKullanici.kullnId).Select(x => x.EXAM_ID).FirstOrDefault() != 0)
                {
                    MessageBox.Show("Bu isimlendirmeye sahip bir sınavınız bulunuyor!", "UYARI!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                AlgorithmaModel algorithma = new AlgorithmaModel(OzelSessionTurler.ToArray());

                TestEtkilesimMVM testMVVM = new TestEtkilesimMVM(_soruSayisi)
                {
                    Zamanlayici = new EtkilesimWPF.MVVM.Model.ZamanlayiciModel(_dakika),
                    Algorithma = algorithma,
                    Baslik = baslik
                };

                EtkilesimPencereWPF _test = new EtkilesimPencereWPF(testMVVM, Onceki);

                _test.Show();
                _test.TimerPartner.Visibility = Visibility.Visible;
                _test.TimerPartner.IsEnabled = true;

                Hide();
            });
        }


        private bool Kosullar()
        {
            if ((listKonu.Items.Count > 0 && !OzelSessionTurler.Contains(SoruTerimleri.soruTuru.problem))
                || (listKonu.Items.Count > 1 && OzelSessionTurler.Contains(SoruTerimleri.soruTuru.problem)))
            {
                //Test etmek için 1'e düşürdüm arttırmadıysam arttırın -Hüseyin
                if (_dakika > 0 && _soruSayisi >= 5)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Soru Sayisini 5 veya fazlasını yapınız");
                }
            }

            return false;
        }

        private void SayiGozleyici(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !YaziOlur(e.Text);
        }

        //Harf/Karakter Girmeyi Engellemek için
        private static bool YaziOlur(string text)
        {
            return !_redler.IsMatch(text);
        }

        private void SoruSayisiDegisken(object sender, TextChangedEventArgs e)
        {
            int.TryParse(txtboxSoru.Text, out int sayi);

            _soruSayisi = sayi;
            _dakika = sayi * 1.5f;
            txtboxSure.Text = _dakika.ToString();
        }

        private void listKonu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (e.Key == Key.Delete)
                {
                    if (listKonu.SelectedItem != null && listKonu.SelectedItems.Count < 2)
                    {
                        OzelSessionTurler.RemoveAt(listKonu.SelectedIndex);
                        listKonu.Items.Refresh();
                    }
                }
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (Onceki != null)
            {
                Onceki.Show();
            }

            this.Close();
        }
    }
}
