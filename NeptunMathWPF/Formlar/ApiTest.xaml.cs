using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using NeptunMathWPF.Models;   // ← Models namespace’ini eklemeyi unutma!
using System.Configuration;
using NeptunMathWPF.SoruVeAjani.Problemler;

namespace NeptunMathWPF.Formlar
{
    public partial class ApiTest : Window
    {

        public ApiTest()
        {
            InitializeComponent();
            BtnCoz.IsEnabled = false;
        }

        private async void BtnUret_Click(object sender, RoutedEventArgs e)
        {
            TxtProblem.Text = TxtCozum.Text = "";
            UpdateStatus("Problem üretiliyor...");
            string prompt = "5. sınıf seviyesinde, ondalık sayılarda toplama ve çıkarma içeren bir matematik problem sorusu oluştur. Doğru sayısal cevabı hesapla. Ardından, doğru cevaptan farklı olan, makul görünen 3 adet yanlış cevap seçeneği üret.\r\n\r\nYanıtı SADECE aşağıdaki JSON formatında ver ve tüm değerler string olsun.:\r\n{\r\n  \"problem\": \"...\",\r\n  \"dogru_cevap\": ...,\r\n  \"yanlis_cevaplar\": [\r\n    \"...\",\r\n    \"...\",\r\n    \"...\"\r\n  ]\r\n}\r\n\r\nYanlış cevapların doğru cevaptan farklı olduğundan emin ol.";
            GenerateProblem(prompt);
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
            var solution = await APIOperations.CallGeminiApiTypedAsync(solvePrompt);
            if (solution != null) TxtCozum.Text = solution;
        }

        private async void GenerateProblem(string problemPrompt)
        {
            // BtnUret_Click veya BtnCoz_Click metodunuz içinde,
            // API metodundan yanıtı aldıktan sonra

            string generatedJsonString = await APIOperations.CallGeminiApiTypedAsync(problemPrompt);
            generatedJsonString = generatedJsonString.Replace("```json", "");
            generatedJsonString = generatedJsonString.Replace("```", "");


            
            if (!string.IsNullOrEmpty(generatedJsonString))
            {
                try
                {

                    var quizData = JsonSerializer.Deserialize<ProblemRepository>(generatedJsonString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    if (quizData != null)
                    {

                        TxtProblem.Text = quizData.problem; 

                        StringBuilder sbCozum = new StringBuilder();
                        sbCozum.AppendLine($"Doğru Cevap: {quizData.correctAnswer}");
                        sbCozum.AppendLine("Yanlış Cevap Seçenekleri:");
                        if (quizData.incorrectAnswers != null)
                        {
                            for (int i = 0; i < quizData.incorrectAnswers.Count; i++)
                            {
                                sbCozum.AppendLine($"{i + 1}. {quizData.incorrectAnswers[i]}");
                            }
                        }
                        TxtCozum.Text = sbCozum.ToString();

                        UpdateStatus("Problem ve cevaplar yüklendi.");
                    }
                    else
                    {
                        UpdateStatus("Gelen JSON ayrıştırılamadı veya boş.");
                    }
                }
                catch (JsonException jsonEx) 
                {
                    UpdateStatus($"JSON Ayrıştırma Hatası: {jsonEx.Message}");

                }
                catch (Exception ex) // Diğer hatalar
                {
                    UpdateStatus($"Beklenmeyen Hata: {ex.Message}");
                }
            }
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
