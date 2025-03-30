using NeptunMathWPF.SoruVeAjani;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// SoruNesneTestWPF.xaml etkileşim mantığı
    /// </summary>
    using ifadeTuru = SoruTerimleri.ifadeTurleri;
    public partial class SoruNesneTestWPF : Window
    {
        //Huseyin
        //Soru, Nesne, Parametre ve Sonuç Test etme formu
        //Reddedilen karakterler
        private static readonly Regex _redler = new Regex("[^0-9.-]+");
        List<RadioButton> Secenekler = new List<RadioButton>();
        List<ifadeTuru> CokluIfadeIstegi = new List<ifadeTuru>();

        Soru seciliSoru;

        public SoruNesneTestWPF()
        {
            InitializeComponent();

            for(int i = 0; i < SoruAjani.Araliklar.Count; i++)
            {
                comboBoxParametreler.Items.Add(SoruAjani.Araliklar.ElementAt(i).Key);
            }
            CokluIfadeListBox.ItemsSource = CokluIfadeIstegi;
            CokluIfadeComboBox.ItemsSource = Enum.GetNames(typeof(ifadeTuru));
            
        }

        //SORU OLUSTUR DORT İŞLEM
        private void TusTamSayiSoruOlustur(object sender, RoutedEventArgs e)
        {
            ParametreGuncelle();

            List<ifade> liste = SoruAjani.IfadeListesiOlustur(SoruTerimleri.ifadeTurleri.sayi, (int)sliderIfade.Value);
            Soru soru = SoruAjani.YerelSoruBirlestir(liste, seceneksayisi: (int)sliderSecenek.Value);

            SoruLOG.Text = soru.GetOlusturmaLogu();
            LatexCikti.Formula = soru.GetMetin();

            seciliSoru = soru;
            WrapPanelYenile();
        }

        private void comboBoxParametreler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Genel.Handle(() =>
            {
                ParametreGuncelle();

                //Kutuları Günceller
                min.Text = SoruAjani.Araliklar[(sender as ComboBox).SelectedItem.ToString()][0].ToString();
                max.Text = SoruAjani.Araliklar[(sender as ComboBox).SelectedItem.ToString()][1].ToString();
            });
        }

        //Metin Kutusu girdisi
        //StackOverflow'dan Alındı
        private void SayiGozleyici(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !YaziOlur(e.Text);
        }

        //Harf/Karakter Girmeyi Engellemek için
        private static bool YaziOlur(string text)
        {
            return !_redler.IsMatch(text);
        }

        private void ParametreGuncelle()
        {
            //Değişeni Güncelle
            if (comboBoxParametreler.Text.ToString() != String.Empty)
            {
                if (min.Text != String.Empty && max.Text != String.Empty)
                {
                    if (int.Parse(min.Text) < int.Parse(max.Text))
                    {
                        SoruAjani.Araliklar[comboBoxParametreler.Text.ToString()][0] = int.Parse(min.Text);
                        SoruAjani.Araliklar[comboBoxParametreler.Text.ToString()][1] = int.Parse(max.Text);
                    }
                }
            }
        }

        private void TusKesirSayiOlustur(object sender, RoutedEventArgs e)
        {
            List<ifade> liste = SoruAjani.IfadeListesiOlustur(SoruTerimleri.ifadeTurleri.kesir, (int)sliderIfade.Value);
            Soru soru = SoruAjani.YerelSoruBirlestir(liste, seceneksayisi: (int)sliderSecenek.Value);

            SoruLOG.Text = soru.GetOlusturmaLogu();
            LatexCikti.Formula = soru.GetLaTex();
            seciliSoru = soru;
            WrapPanelYenile();
        }

        //Wrap Panel (Sorunun altındaki yer)'e  Secenekleri ekleme)
        internal void WrapPanelYenile()
        {
            Genel.Handle(() =>
            {
                //Sıfırla sonra tekrar ekle
                SeceneklerWrapPanel.Children.Clear();
                //Rastgale bir sıraya sıkıştır
                Random rng = new Random();
                int rand = rng.Next(0, seciliSoru.GetDigerSecenekler().Length);

                for (int i = 0; i < seciliSoru.GetDigerSecenekler().Length; i++)
                {
                    string icerik;
                    //Sonucu Rastgele bir noktaya koymak için
                    if (i == rand)
                    {
                        RadioButton Randy = new RadioButton() { GroupName = "Sonuclar", Content = seciliSoru.GetSonucSecenek(), FontSize = 22, Width = 139, Height = 59 };
                        SeceneklerWrapPanel.Children.Add(Randy);
                        Secenekler.Add(Randy);
                    }

                    //Listedeki Diğer Seçenekler
                    RadioButton Rad = new RadioButton() { GroupName = "Sonuclar", Content = seciliSoru.GetDigerSecenekler()[i], FontSize = 22, Width = 139, Height = 59 };

                    SeceneklerWrapPanel.Children.Add(Rad);
                    Secenekler.Add(Rad);
                }
            });
        }

        private void tusCevapla_Click(object sender, RoutedEventArgs e)
        {
            //DAHA iyi bir seçenek buluna kadar DOĞRU/YANLIŞ kontrolü
            RadioButton CevapFetch = null;

            foreach(RadioButton r in Secenekler)
            {
                if (r.IsChecked == true)
                {
                    CevapFetch = r;
                }
            }

            if (CevapFetch != null)
            {
                if (CevapFetch.Content.ToString() == seciliSoru.GetSonucSecenek())
                {
                    MessageBox.Show($"DOĞRU CEVAP :: Seçilen Cevap {CevapFetch.Content.ToString()}");
                }
                else
                {
                    MessageBox.Show($"YANLIŞ CEVAP :: Seçilen Cevap {CevapFetch.Content.ToString()} \n DOĞRU CEVAP :: {seciliSoru.GetSonucSecenek()}");
                    // Soruya yanlış cevap verildiğinde, doğru cevabı göstermeyi ekledim.
                }
            }
        }

        private void TusCokluIfadeSoruOlustur(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (CokluIfadeIstegi.Count != 0)
                {
                    List<ifade> Liste = SoruAjani.CokluIfadeListesiOlustur(CokluIfadeIstegi);
                    Soru soru = SoruAjani.YerelSoruBirlestir(Liste);

                    SoruLOG.Text = soru.GetOlusturmaLogu();
                    LatexCikti.Formula = soru.GetLaTex();
                    seciliSoru = soru;
                    WrapPanelYenile();
                }
            });
        }

        
        private void TusEkleClick(object sender, RoutedEventArgs e)
        {
            if (CokluIfadeComboBox.SelectedValue == null)
            {
                MessageBox.Show("Lütfen bir ifade türü seçin.");
                return;
            }

            string a = CokluIfadeComboBox.SelectedValue.ToString();
            
            if (a != null)
            {
                ifadeTuru tur;
                Enum.TryParse<ifadeTuru>(a, out tur);

                CokluIfadeIstegi.Add(tur);
            }           
            CokluIfadeListBox.Items.Refresh();
        }
             

        private void CokluIfadeListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                if (CokluIfadeListBox.SelectedItem != null && CokluIfadeListBox.SelectedItems.Count < 2)
                {
                    CokluIfadeIstegi.RemoveAt(CokluIfadeListBox.SelectedIndex);
                    CokluIfadeListBox.Items.Refresh();
                }
            }
        }
    }
}
