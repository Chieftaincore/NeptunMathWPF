using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for GetAPIKeyPanelWPF.xaml
    /// </summary>
    public partial class GetAPIKeyPanelWPF : Window
    {
        private bool closeApplication = true;
        public GetAPIKeyPanelWPF()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (closeApplication)
                Application.Current.Shutdown();
        }

        private async void onaylaButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(apikeyTextBox.Text))
            {
                await CreateGeminiConfig();
            }
        }

        private async Task CreateGeminiConfig()
        {
            string baseApiUrl = Genel.baseUrlFlash;

            if (flashRadioButton.IsChecked == true) { }
            else if (proRadioButton.IsChecked == true)
            {
                baseApiUrl = Genel.baseUrlPro;
            }
            else
            {
                MessageBox.Show("Model seçiniz!", "UYARI!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            onaylaButton.IsEnabled = false;
            onaylaButton.Content = "Lütfen bekleyin...";
            var deneme = await APIOperations.CallGeminiApiTypedAsync("Selam", apikeyTextBox.Text, baseApiUrl);
            if (deneme != null)
            {

                string configContent = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <appSettings>
        <add key=""GeminiApiKey"" value=""{apikeyTextBox.Text}"" /> 
        <add key=""GeminiBaseUrl"" value=""{baseApiUrl}""/>
    </appSettings>
</configuration>";

                try
                {
                    File.WriteAllText(Genel.geminiFilePath, configContent);
                    closeApplication = false;
                    this.Close();
                }
                catch (Exception ex)
                {
                    onaylaButton.IsEnabled = true;
                    onaylaButton.Content = "Onayla";
                    Genel.LogToDatabase(LogLevel.ERROR, ex.Message);
                    MessageBox.Show("Config dosyası oluşturulurken bir hata oluştu. Yetkiliyle iletişime geçiniz.", "HATA!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                onaylaButton.IsEnabled = true;
                onaylaButton.Content = "Onayla";
                MessageBox.Show("Doğru API key/model giriniz ya da internet bağlantınızı kontrol ediniz!", "HATA!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
