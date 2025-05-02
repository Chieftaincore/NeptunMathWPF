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
            string difficulty = problemDifficulty.ToString();
            if (problemDifficulty == ProblemDifficulty.CokZor)
            {
                difficulty = "çok zor";
            }

            string type = ProblemTypeToString(problemType);

            string problemPrompt = $"Aşağıdaki JSON yapısına uygun bir matematik problemi, doğru cevabı ve üç adet yanlış cevap üret. Üretilen problem ve doğru cevap matematiksel olarak %100 doğru olmalıdır. Problem, ortaokul veya lise seviyesine uygun ancak {difficulty} seviyede olmalı ve çözülebilir nitelikte olmalıdır. Problem sorusunun türü {type} Problemi olmalıdır. Soru içeriğini rastgele seç. Yanlış cevaplar, olası mantık hatalarını veya işlem hatalarını yansıtan, makul görünen ama yanlış seçenekler olmalıdır. Soruyu ve cevabı ürettikten sonra kendin cevabın ve sorunun doğruluğunu test et ve eğer doğru değilse soruyu tekrar yaz.\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n İstenen JSON formatı: \r\n\r\n\r\n\r\n ```json \r\n\r\n\r\n\r\n  {{  \r\n\r\n\r\n\r\n   \"problem\": \"Matematik problemi metni buraya\", \r\n\r\n\r\n\r\n   \"dogru_cevap\": \"Doğru sayısal cevap buraya\", \r\n\r\n\r\n\r\n   \"yanlis_cevaplar\": [ \r\n\r\n\r\n\r\n     \"Yanlış cevap 1 (sayısal)\", \r\n\r\n\r\n\r\n     \"Yanlış cevap 2 (sayısal)\", \r\n\r\n\r\n\r\n     \"Yanlış cevap 3 (sayısal)\" \r\n\r\n\r\n\r\n   ] \r\n\r\n\r\n\r\n }} \r\n\r\n\r\n\r\n ``` \r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n Sadece bu JSON çıktısını üret, başına veya sonuna kesinlikle başka hiçbir metin ekleme.";

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


        private static string ProblemTypeToString(ProblemType type)
        {
            string returnString = "";
            switch (type)
            {
                case ProblemType.Havuz:
                    returnString = "Havuz";
                    break;
                case ProblemType.Isci:
                    returnString = "İşçi";
                    break;
                case ProblemType.SayiveKesir:
                    returnString = "Sayı ve Kesir";
                    break;
                case ProblemType.Yas:
                    returnString = "Yaş";
                    break;
                case ProblemType.Yuzde:
                    returnString = "Yüzde";
                    break;
                case ProblemType.Karisim:
                    returnString = "Karışım";
                    break;
                case ProblemType.KarZarar:
                    returnString = "Kar Zarar";
                    break;
                case ProblemType.Hareket:
                    returnString = "Hareket";
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
        Kolay,
        Orta,
        Zor,
        CokZor
    }
}
