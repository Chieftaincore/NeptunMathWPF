using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using WpfMath.Controls;

namespace NeptunMathWPF.SoruVeAjani.Turev.Views
//Burası tam class gibi değil bide namespace benim kodda .Views diye geçiyor
// Bide tüm kodu yoruma aldım çözümlecedikçe açılacaktır
{
    //public class TurevAnaCalısanYer
    //{
    //    private DerivativeQuestion currentQuestion;
    //    private int correctAnswers = 0;
    //    private int totalQuestions = 0;
    //    private Random random = new Random();
    //    private QuestionType currentQuestionType;

    //    public MainWindow()
    //    {
    //        InitializeComponent();
    //        GenerateNewQuestion();
    //    }

    //    private string GetQuestionTypeText(QuestionType type)
    //    {
    //        switch (type)
    //        {
    //            case QuestionType.BasicDerivative:
    //                return "Soru Tipi: Temel Türev Hesaplama";
    //            case QuestionType.PointDerivative:
    //                return "Soru Tipi: Belirli Noktada Türev Değeri";
    //            case QuestionType.FindCoefficient:
    //                return "Soru Tipi: Katsayı Belirleme";
    //            case QuestionType.ProductRule:
    //                return "Soru Tipi: Çarpım Kuralı ile Türev";
    //            default:
    //                return "";
    //        }
    //    }

    //    private void GenerateNewQuestion()
    //    {
    //        // Rastgele soru tipini seç
    //        Array questionTypes = Enum.GetValues(typeof(QuestionType));
    //        currentQuestionType = (QuestionType)questionTypes.GetValue(random.Next(questionTypes.Length));

    //        switch (currentQuestionType)
    //        {
    //            case QuestionType.BasicDerivative:
    //                currentQuestion = DerivativeQuestionGenerator.GenerateBasicDerivativeQuestion();
    //                break;
    //            case QuestionType.PointDerivative:
    //                currentQuestion = DerivativeQuestionGenerator.GeneratePointDerivativeQuestion();
    //                break;
    //            case QuestionType.FindCoefficient:
    //                currentQuestion = DerivativeQuestionGenerator.GenerateFindCoefficientQuestion();
    //                break;
    //            case QuestionType.ProductRule:
    //                currentQuestion = DerivativeQuestionGenerator.GenerateProductRuleQuestion();
    //                break;
    //        }

    //        DisplayQuestion();
    //        GenerateOptions();
    //    }

    //    private void DisplayQuestion()
    //    {
    //        // LaTeX formatında soruyu göster
    //        formulaControl.Formula = currentQuestion.QuestionLaTeX;

    //        // Soru tipini göster
    //        questionTypeTextBlock.Text = GetQuestionTypeText(currentQuestionType);

    //        // Skor güncelleme
    //        scoreTextBlock.Text = $"Skor: {correctAnswers}/{totalQuestions}";
    //    }

    //    private void GenerateOptions()
    //    {
    //        optionsPanel.Children.Clear();

    //        // Doğru cevap ve şaşırtmacalar
    //        List<double> options = new List<double>();
    //        options.Add(currentQuestion.Answer);

    //        // 3 tane farklı yanlış şık oluştur
    //        while (options.Count < 4)
    //        {
    //            double wrongOption;

    //            // Doğru cevaba göre makul şaşırtıcı seçenekler oluştur
    //            if (Math.Abs(currentQuestion.Answer) < 10)
    //            {
    //                // Küçük sayılar için daha küçük farklar
    //                wrongOption = currentQuestion.Answer + random.Next(-5, 6);
    //                while (wrongOption == currentQuestion.Answer)
    //                    wrongOption = currentQuestion.Answer + random.Next(-5, 6);
    //            }
    //            else
    //            {
    //                // Büyük sayılar için daha büyük yanlış seçenekler
    //                int variation = (int)Math.Max(3, Math.Abs(currentQuestion.Answer) * 0.3);
    //                wrongOption = currentQuestion.Answer + random.Next(-variation, variation + 1);
    //                while (wrongOption == currentQuestion.Answer)
    //                    wrongOption = currentQuestion.Answer + random.Next(-variation, variation + 1);
    //            }

    //            // Aynı şık olmamasını sağla
    //            if (!options.Contains(wrongOption) && Math.Abs(wrongOption - currentQuestion.Answer) > 0.001)
    //            {
    //                options.Add(wrongOption);
    //            }
    //        }

    //        // Şıkları karıştır
    //        options = options.OrderBy(x => random.Next()).ToList();

    //        // Şıkları ekrana ekle
    //        char optionLetter = 'A';
    //        foreach (double option in options)
    //        {
    //            RadioButton radioButton = new RadioButton
    //            {
    //                Content = $"{optionLetter}. {option}",
    //                Margin = new Thickness(5),
    //                FontSize = 14,
    //                Tag = option
    //            };
    //            optionsPanel.Children.Add(radioButton);
    //            optionLetter++;
    //        }
    //    }

    //    private void CheckButton_Click(object sender, RoutedEventArgs e)
    //    {
    //        bool answerSelected = false;
    //        bool isCorrect = false;

    //        foreach (RadioButton rb in optionsPanel.Children.OfType<RadioButton>())
    //        {
    //            if (rb.IsChecked == true)
    //            {
    //                answerSelected = true;
    //                double selectedAnswer = (double)rb.Tag;

    //                // Hassasiyet toleransını azaltarak tam sayı karşılaştırmaları daha doğru yapalım
    //                if (Math.Abs(selectedAnswer - currentQuestion.Answer) < 0.0001)
    //                {
    //                    isCorrect = true;
    //                    correctAnswers++;
    //                }

    //                totalQuestions++;
    //                break;
    //            }
    //        }

    //        if (!answerSelected)
    //        {
    //            MessageBox.Show("Lütfen bir cevap seçiniz!", "Uyarı");
    //            return;
    //        }

    //        // Sonucu göster
    //        resultTextBlock.Text = isCorrect ? "Doğru Cevap!" : $"Yanlış! Doğru cevap: {currentQuestion.Answer}";
    //        resultTextBlock.Foreground = isCorrect ? Brushes.Green : Brushes.Red;

    //        // Açıklama göster
    //        if (currentQuestion.ExplanationText != null)
    //        {
    //            resultTextBlock.Text += $"\n{currentQuestion.ExplanationText}";
    //        }

    //        // İstatistikleri güncelle
    //        scoreTextBlock.Text = $"Skor: {correctAnswers}/{totalQuestions}";
    //    }

    //    private void NextButton_Click(object sender, RoutedEventArgs e)
    //    {
    //        resultTextBlock.Text = "";
    //        GenerateNewQuestion();
    //    }
    //}
}
