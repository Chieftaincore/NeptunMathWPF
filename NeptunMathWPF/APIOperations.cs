using NeptunMathWPF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static System.Net.WebRequestMethods;

namespace NeptunMathWPF
{
    static class APIOperations
    {

        private static string apiKey = GetGeminiApiKey();
        public static string baseApiUrl = GetGeminiBaseUrl();
        private static readonly HttpClient _httpClient = new HttpClient();

        public static string GetGeminiApiKey()
        {
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = "GEMINI.config"
            };

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            return config.AppSettings.Settings["GeminiApiKey"]?.Value;
        }

        public static string GetGeminiBaseUrl()
        {
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = "GEMINI.config"
            };

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            return config.AppSettings.Settings["GeminiBaseUrl"]?.Value;
        }

        internal async static Task<string> CallGeminiApiTypedAsync(string promptText)
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

        internal async static Task<string> CallGeminiApiTypedAsync(string promptText, string apikey, string baseUrl)
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
            string requestUrl = $"{baseUrl}?key={apikey}";

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


        private static void UpdateStatus(string text)
        {

        }
    }
}
