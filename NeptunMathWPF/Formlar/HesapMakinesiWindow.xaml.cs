using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.Text.RegularExpressions;

namespace HesapMakinesi
{
    public partial class HesapMakinesiWindow : Window
    {
        public HesapMakinesiWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void EkleText(string text)
        {
            int selectionStart = IslemKutusu.SelectionStart;
            IslemKutusu.Text = IslemKutusu.Text.Insert(selectionStart, text);
            IslemKutusu.SelectionStart = selectionStart + text.Length;
            IslemKutusu.Focus();
        }

        private void SayiButonu_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            EkleText(button.Content.ToString());
        }

        private void OperatorButonu_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            EkleText(button.Content.ToString());
        }

        private void TemizleButonu_Click(object sender, RoutedEventArgs e)
        {
            IslemKutusu.Text = "";
            SonucKutusu.Text = "";
            IslemKutusu.Focus();
        }

        private void FaktoriyelButonu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int cursorPos = IslemKutusu.SelectionStart;
                string textBeforeCursor = IslemKutusu.Text.Substring(0, cursorPos);

                // İmlecin solundaki sayıyı bul (en yakın tam sayı)
                Match numberMatch = Regex.Match(textBeforeCursor, @"(\d+)(?=\D*$)");

                if (numberMatch.Success && long.TryParse(numberMatch.Value, out long number))
                {
                    if (number < 0)
                    {
                        SonucKutusu.Text = "Hata: Negatif sayı!";
                        return;
                    }
                    else if (number > 20) // 21! long için çok büyük
                    {
                        SonucKutusu.Text = "Hata: Sayı çok büyük!";
                        return;
                    }

                    // Sadece ! işaretini ekle, sonucu gösterme
                    IslemKutusu.Text = IslemKutusu.Text.Insert(cursorPos, "!");
                    IslemKutusu.SelectionStart = cursorPos + 1;
                }
                else
                {
                    SonucKutusu.Text = "Hata: Geçerli bir sayı seçin!";
                }
            }
            catch (Exception ex)
            {
                SonucKutusu.Text = "Hata: " + ex.Message;
            }
            IslemKutusu.Focus();
        }

        private long FaktoriyelHesapla(long n)
        {
            if (n == 0) return 1;
            long result = 1;
            for (long i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        private void EsittirButonu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = IslemKutusu.Text;
                if (!string.IsNullOrEmpty(expression))
                {
                    // Faktöriyel işlemlerini hesapla
                    expression = HesaplaFaktoriyeller(expression);

                    // DataTable'ın Compute metodu ile matematiksel ifadeyi hesapla
                    var result = new DataTable().Compute(expression, null);

                    // Sonucu formatla
                    if (result is double || result is decimal || result is float)
                    {
                        double res = Convert.ToDouble(result);
                        SonucKutusu.Text = res.ToString(res % 1 == 0 ? "N0" : "0.##########");
                    }
                    else
                    {
                        SonucKutusu.Text = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                SonucKutusu.Text = "Hata: " + ex.Message;
            }
            IslemKutusu.Focus();
        }

        private string HesaplaFaktoriyeller(string expression)
        {
            // Faktöriyel işlemlerini bul ve hesapla
            var matches = Regex.Matches(expression, @"(\d+)\s*\!");
            foreach (Match match in matches)
            {
                if (long.TryParse(match.Groups[1].Value, out long number))
                {
                    if (number >= 0 && number <= 20)
                    {
                        long faktoriyel = FaktoriyelHesapla(number);
                        expression = expression.Replace(match.Value, faktoriyel.ToString());
                    }
                    else if (number > 20)
                    {
                        throw new Exception($"{number}! çok büyük (maksimum 20!)");
                    }
                }
            }
            return expression;
        }

        private void IslemKutusu_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string allowedChars = "0123456789+-*/().!";
            if (!allowedChars.Contains(e.Text))
            {
                e.Handled = true;
            }
        }

        private void IslemKutusu_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (IslemKutusu.SelectionStart > 0 && IslemKutusu.Text.Length > 0)
                {
                    int selectionStart = IslemKutusu.SelectionStart;
                    IslemKutusu.Text = IslemKutusu.Text.Remove(selectionStart - 1, 1);
                    IslemKutusu.SelectionStart = selectionStart - 1;
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Delete)
            {
                if (IslemKutusu.SelectionStart < IslemKutusu.Text.Length)
                {
                    int selectionStart = IslemKutusu.SelectionStart;
                    IslemKutusu.Text = IslemKutusu.Text.Remove(selectionStart, 1);
                    IslemKutusu.SelectionStart = selectionStart;
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Enter)
            {
                EsittirButonu_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}