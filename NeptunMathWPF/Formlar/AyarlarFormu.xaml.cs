using NeptunMathWPF;
using NeptunMathWPF.Formlar;
using NeptunMathWPF.Formlar.EtkilesimWPF;
using System;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace AnasayfaWPF
{
    public partial class AyarlarFormu : Window
    {
        public AyarlarFormu()
        {
            InitializeComponent();
            apiUrlComboBox.Items.Add("Pro");
            apiUrlComboBox.Items.Add("Flash");
            apikeyTextBox.Text = APIOperations.GetGeminiApiKey();

            if (APIOperations.GetGeminiBaseUrl().Contains("flash"))
                apiUrlComboBox.SelectedIndex = 1;
            else apiUrlComboBox.SelectedIndex = 0;
        }

        private void KaydetButton_Click(object sender, RoutedEventArgs e)
        {
            // Ayarları kaydetme işlemleri burada yapılacak
            MessageBox.Show("Ayarlar kaydedildi.");
            this.Close();
        }

        private void KullaniciAdiSifirla_Click(object sender, RoutedEventArgs e)
        {
            new kullaniciadiDegisPanelWPF().ShowDialog();
        }

        private void SifreSifirla_Click(object sender, RoutedEventArgs e)
        {
            // Şifre sıfırlama işlemleri burada yapılacak
            new sifreDegisPanelWPF().ShowDialog();
        }

        private void SoruProfiliSifirla_Click(object sender, RoutedEventArgs e)
        {
            // Soru profili sıfırlama işlemleri burada yapılacak
            MessageBox.Show("Soru profili sıfırlama işlemi başlatıldı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void editApiUrlButton_Click(object sender, RoutedEventArgs e)
        {
            bool deger = apiUrlComboBox.IsEnabled;
            apiUrlComboBox.IsEnabled = !deger;
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (apiUrlComboBox.IsEnabled == true)
            {
                if (!APIOperations.GetGeminiBaseUrl().Contains(apiUrlComboBox.SelectedItem.ToString().ToLower()))
                {

                    string baseApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-pro-preview-05-06:generateContent";
                    if (apiUrlComboBox.SelectedIndex == 0)
                    {
                        saveButton.IsEnabled = false;
                        saveButton.Content = "Lütfen bekleyin...";
                        var deneme = await APIOperations.CallGeminiApiTypedAsync("Selam", apikeyTextBox.Text, baseApiUrl);
                        if (deneme == null)
                        {
                            MessageBox.Show("API Key'iniz bu modeli desteklemiyor!", "UYARI", MessageBoxButton.OK, MessageBoxImage.Warning);
                            saveButton.IsEnabled = true;
                            saveButton.Content = "Kaydet";
                            return;
                        }

                    }
                    else if (apiUrlComboBox.SelectedIndex == 1)
                    {
                        baseApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-preview-05-20:generateContent";
                    }
                    else return;

                    UpdateGeminiBaseUrl(Genel.geminiFilePath, baseApiUrl);

                }
                else
                {
                    MessageBox.Show("Zaten bu modeli kullanıyorsunuz!", "UYARI!");
                }
            }
            else
            {
                MessageBox.Show("Düzeltmeyi aktif ediniz!", "UYARI!");
            }
        }

        void UpdateGeminiBaseUrl(string filePath, string newUrl)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Config dosyası bulunamadı.", "HATA!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                XDocument doc = XDocument.Load(filePath);

                XElement appSettings = doc.Root.Element("appSettings");

                if (appSettings != null)
                {
                    XElement baseUrlElement = null;

                    foreach (XElement el in appSettings.Elements("add"))
                    {
                        if ((string)el.Attribute("key") == "GeminiBaseUrl")
                        {
                            baseUrlElement = el;
                            break;
                        }
                    }

                    if (baseUrlElement != null)
                    {
                        baseUrlElement.SetAttributeValue("value", newUrl);
                        MessageBox.Show("Model güncellendi.", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                        saveButton.IsEnabled = true;
                        saveButton.Content = "Kaydet";
                    }
                    else
                    {
                        // Eğer key yoksa ekle
                        appSettings.Add(new XElement("add",
                            new XAttribute("key", "GeminiBaseUrl"),
                            new XAttribute("value", newUrl)));
                        MessageBox.Show("Model Eklendi.", "INFO", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    doc.Save(filePath);
                }
                else
                {
                    MessageBox.Show("<appSettings> etiketi bulunamadı.", "HATA!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Genel.LogToDatabase(LogLevel.ERROR, ex.Message);
                MessageBox.Show("Hata oluştu!", "HATA!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
