using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using NeptunMathWPF.Models;   // ← Models namespace’ini eklemeyi unutma!

namespace NeptunMathWPF.Formlar
{
    public partial class ApiTest : Window
    {
        private const string apiKey = "AIzaSyAZ6k8-gyEsXonU9EP30DzFyMxBwH2Y_Go";
        private const string baseApiUrl =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";
        private readonly HttpClient _httpClient = new HttpClient();

        public ApiTest()
        {
            InitializeComponent();
        }

        private async Task<string> CallGeminiApiTypedAsync(string promptText)
        {
            UpdateStatus("API isteği hazırlanıyor...");

            // 1) İstek gövdesi nesnesi
            var requestObj = new GenerateContentRequest
            {
                Contents = new List<ContentItem>
                {
                    new ContentItem
                    {
                        Parts = new List<Part>
                        {
                            new Part { Text = promptText }
                        }
                    }
                }
            };

            // 2) JSON serialize
            string requestJson = JsonSerializer.Serialize(requestObj);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            // 3) URL’e API anahtarını ekle
            string requestUrl = $"{baseApiUrl}?key={apiKey}";

            try
            {
                UpdateStatus("API isteği gönderiliyor...");
                var response = await _httpClient.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                UpdateStatus("Yanıt alındı, işleniyor...");

                // 4) Tipli nesneye deserialize
                var responseObj = JsonSerializer.Deserialize<
                    GenerateContentResponse>(
                        jsonResponse,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                // 5) Metni çek ve döndür
                var aiText = responseObj?
                    .Candidates?[0]
                    .Content?
                    .Parts?[0]
                    .Text;

                if (string.IsNullOrEmpty(aiText))
                {
                    UpdateStatus("Cevap boş döndü.");
                    return null;
                }

                UpdateStatus("Başarılı.");
                return aiText.Trim();
            }
            catch (HttpRequestException httpEx)
            {
                UpdateStatus($"HTTP hatası: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Genel hata: {ex.Message}");
            }

            return null;
        }

        private async void BtnUret_Click(object sender, RoutedEventArgs e)
        {
            TxtProblem.Text = TxtCozum.Text = "";
            UpdateStatus("Problem üretiliyor...");
            string prompt = "Lütfen 5. sınıf seviyesinde, kesir içeren basit bir matematik problem sorusu oluştur ve sadece soruyu yaz. (Türkçe)";
            var problem = await CallGeminiApiTypedAsync(prompt);
            if (problem != null) TxtProblem.Text = problem;
        }

        private async void BtnCoz_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtProblem.Text))
            {
                UpdateStatus("Önce bir problem üretin.");
                return;
            }

            TxtCozum.Text = "";
            UpdateStatus("Çözüm alınıyor...");
            string solvePrompt =
                $"Aşağıdaki matematik problemini adım adım çöz ve en son cevabı belirt. Çözümü ve cevabı Türkçe yaz.\n\nProblem: {TxtProblem.Text}";
            var solution = await CallGeminiApiTypedAsync(solvePrompt);
            if (solution != null) TxtCozum.Text = solution;
        }

        private void UpdateStatus(string text)
        {
            if (Dispatcher.CheckAccess())
                TxtStatus.Text = text;
            else
                Dispatcher.Invoke(() => TxtStatus.Text = text);
        }
    }
}
