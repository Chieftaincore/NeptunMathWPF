using NeptunMathWPF.SoruVeAjani.Problemler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF
{
    public static class ProblemGenerator
    {
        public async static Task<ProblemRepository> GenerateProblem(ProblemType problemType, ProblemDifficulty problemDifficulty)
        {
            string problemTypeString = ProblemTypeToString(problemType);

            string difficultyDetail = "";
            switch (problemDifficulty)
            {
                case ProblemDifficulty.Kolay:
                    difficultyDetail = "temel düzeyde, çok basit ve kolayca çözülebilir bir problem olsun.";
                    break;
                case ProblemDifficulty.Orta:
                    difficultyDetail = "orta düzeyde, az miktarda zorlayan ve azcık zorlanarak çözülebilir bir problem olsun.";
                    break;
                case ProblemDifficulty.Zor: 
                    difficultyDetail = "zor düzeyde, zor ve zorlanarak çözülebilir bir problem olsun.";
                    break;
                case ProblemDifficulty.EnZor: 
                    difficultyDetail = "en zor düzeyde, çok zor ve aşırı zorlanarak çözülebilir bir problem olsun.";
                    break;
            }

            string problemPrompt = $"Lütfen Türkçe bir matematik problem sorusu üretin.\r\nProblem sorusu tipi **{problemTypeString}**" +
                $".\r\nProblemin **zorluk seviyesi 100 üzerinden yaklaşık {(int)problemDifficulty}" +
                $" olsun**, yani {difficultyDetail}\r\n\r\nProblem metnini oluşturun.\r\nBu problem " +
                "için **doğru sayısal cevabı** hesaplayın.\r\nArdından, doğru cevaptan farklı olan, ancak problemi çözen bir öğrenci için **makul " +
                "görünebilecek, sık yapılan hatalardan kaynaklanabilecek veya kafa karıştırıcı 3 adet yanlış cevap seçeneği** oluşturun." +
                "\r\n\r\nYanıtınızı **SADECE** aşağıdaki JSON formatında verin. JSON bloğu dışında hiçbir metin, açıklama, giriş cümlesi " +
                "(örn. \"İşte sorunuz:\") veya kapanış cümlesi eklemeyin. Sadece JSON bloğu olmalı.\r\n\r\nJSON formatı şu şekilde olsun:\r\n{\r\n  " +
                "\"problem\": \"[Buraya oluşturulan problem metni gelecek. Metin Türkçe olacak.]\",\r\n  \"dogru_cevap\": \"[Buraya hesaplanan doğru " +
                "sayısal cevap gelecek. Sadece sayı veya kesir/ondalık değer olarak.]\",\r\n  \"yanlis_cevaplar\": [\r\n    \"[Buraya ilk yanlış cevap " +
                "seçeneği gelecek. Sadece sayı veya kesir/ondalık değer olarak.]\",\r\n    \"[Buraya ikinci yanlış cevap seçeneği gelecek. Sadece sayı" +
                " veya kesir/ondalık değer olarak.]\",\r\n    \"[Buraya üçüncü yanlış cevap seçeneği gelecek. Sadece sayı veya kesir/ondalık değer" +
                " olarak.]\"\r\n  ]\r\n}\r\n\r\nÖrnek: Eğer doğru cevap 10 ise, yanlış cevaplar 5, 12, 8 gibi sayılar olabilir. Lütfen problemle" +
                " ilgili mantıksal olarak türetilebilecek yanlışlar olsunlar.\r\n\r\nProblem ve tüm cevap değerleri **Türkçe** olmalıdır. Sorduğun sorunun ve cevabın doğru olduğundan kesinlikle emin ol. Yanlış soru olmasın.";

            string generatedJsonString = await APIOperations.CallGeminiApiTypedAsync(problemPrompt);
            generatedJsonString = generatedJsonString.Replace("```json", "");
            generatedJsonString = generatedJsonString.Replace("```", "");



            if (!string.IsNullOrEmpty(generatedJsonString))
            {
                try
                {
                    var quizData = JsonSerializer.Deserialize<ProblemRepository>(generatedJsonString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    return quizData;
                }
                catch (JsonException jsonEx)
                {
                    MessageBox.Show(jsonEx.Message);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("else");
                return null;
            }
        }

        private static string ProblemTypeToString(ProblemType problemType)
        {
            string returnString = "";
            switch (problemType)
            {
                case ProblemType.Havuz:
                    returnString = "Havuz Problemleri";
                    break;
                case ProblemType.Isci: 
                    returnString = "İşçi Problemleri";
                    break;
                case ProblemType.SayiveKesir: 
                    returnString = "Sayı ve Kesir Problemleri";
                    break;
                case ProblemType.Yas:
                    returnString = "Yaş Problemleri";
                    break;
                case ProblemType.Karisim: 
                    returnString = "Karışım Problemleri";
                    break;
                case ProblemType.KarZarar:
                    returnString = "Kar ve Zarar Problemleri";
                    break;
                case ProblemType.Hareket:
                    returnString = "Hareket(Yol/Hız/Zaman) Problemleri";
                    break;
            }
            return returnString;
        }
    }

    public enum ProblemType
    {
        Havuz,
        Isci,
        SayiveKesir,
        Yas,
        Yuzde,
        Karisim,
        KarZarar,
        Hareket
    }

    public enum ProblemDifficulty
    {
        Kolay = 25,
        Orta = 50,
        Zor = 75,
        EnZor = 100
    }
}
