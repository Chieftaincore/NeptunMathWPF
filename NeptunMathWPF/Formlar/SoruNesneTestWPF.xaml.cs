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
    public partial class SoruNesneTestWPF : Window
    {
        //Huseyin
        //Reddedilenler
        private static readonly Regex _redler = new Regex("[^0-9.-]+");
        public SoruNesneTestWPF()
        {
            InitializeComponent();

            for(int i = 0; i < SoruAjani.Araliklar.Count; i++)
            {
                comboBoxParametreler.Items.Add(SoruAjani.Araliklar.ElementAt(i).Key);
            }
        }

        //SORU OLUSTUR DORT İŞLEM
        private void TamSayiSoruOlustur(object sender, RoutedEventArgs e)
        {
            ParametreGuncelle();

            List<ifade> liste = SoruAjani.IfadeListesiOlustur(SoruTerimleri.ifadeTurleri.sayi, (int)sliderIfade.Value);
            Soru soru = SoruAjani.YerelSoruBirlestir(liste, seceneksayisi: (int)sliderSecenek.Value);

            SoruLOG.Text = soru.GetOlusturmaLogu();
            LatexCikti.Formula = soru.GetMetin();
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

        private void TusKesiSayiOlustur(object sender, RoutedEventArgs e)
        {
            List<ifade> liste = SoruAjani.IfadeListesiOlustur(SoruTerimleri.ifadeTurleri.kesir, (int)sliderIfade.Value);
            Soru soru = SoruAjani.YerelSoruBirlestir(liste, seceneksayisi: (int)sliderSecenek.Value);

            SoruLOG.Text = soru.GetOlusturmaLogu();
            LatexCikti.Formula = soru.GetLaTex();
        }
    }
}
