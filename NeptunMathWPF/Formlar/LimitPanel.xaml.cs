using HesapMakinesi;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LimitPanel.xaml
    /// </summary>
    public partial class LimitPanel : Window
    {
        private fLimitQuestion currentQuestion;
        private int correctAnswers = 0;
        private int totalQuestions = 0;
        private Random random = new Random();
        private QuestionType currentQuestionType;
        public LimitPanel()
        {
            InitializeComponent();
            GenerateNewQuestion();
        }
        private void GenerateNewQuestion()
        {
            // Rastgele soru tipini seç
            Array questionTypes = Enum.GetValues(typeof(QuestionType));
            currentQuestionType = (QuestionType)questionTypes.GetValue(random.Next(questionTypes.Length));

            switch (currentQuestionType)
            {
                case QuestionType.CommonFactor:
                    currentQuestion = fLimitQuestionGenerator.GenerateCommonFactorQuestion();
                    break;
                case QuestionType.FindCoefficients:
                    currentQuestion = fLimitQuestionGenerator.GenerateFindCoefficientsQuestion();
                    break;
                case QuestionType.CoefficientExpression:
                    currentQuestion = fLimitQuestionGenerator.GenerateCoefficientExpressionQuestion();
                    break;
            }

            DisplayQuestion();
            GenerateOptions();
        }

        private void DisplayQuestion()
        {
            // LaTeX formatında soruyu göster
            formulaControl.Formula = currentQuestion.QuestionLaTeX;

            // Soru tipini göster
            questionTypeTextBlock.Text = GetQuestionTypeText(currentQuestionType);

            // Skor güncelleme
            scoreTextBlock.Text = $"Skor: {correctAnswers}/{totalQuestions}";
        }

        private string GetQuestionTypeText(QuestionType type)
        {
            switch (type)
            {
                case QuestionType.CommonFactor:
                    return "Soru Tipi: Limit Değerini Bulma";
                case QuestionType.FindCoefficients:
                    return "Soru Tipi: Katsayıları Bulma";
                case QuestionType.CoefficientExpression:
                    return "Soru Tipi: Katsayı İfadesini Bulma";
                default:
                    return "";
            }
        }

        private void GenerateOptions()
        {
            optionsPanel.Children.Clear();

            // Doğru cevap ve şaşırtmacalar
            List<double> options = new List<double>();
            options.Add(currentQuestion.Answer);

            // 3 tane farklı yanlış şık oluştur
            while (options.Count < 4)
            {
                double wrongOption;

                if (currentQuestionType == QuestionType.CommonFactor)
                {
                    // Tam sayı şıklar oluştur (doğru cevaptan belirli uzaklıkta)
                    wrongOption = currentQuestion.Answer + random.Next(-10, 11);
                }
                else
                {
                    // Katsayı sorularında daha küçük aralıkta şaşırtma cevaplar
                    wrongOption = currentQuestion.Answer + random.Next(-5, 6);
                }

                // Aynı şık olmamasını sağla
                if (!options.Contains(wrongOption) && Math.Abs(wrongOption - currentQuestion.Answer) > 0.001)
                {
                    options.Add(wrongOption);
                }
            }

            // Şıkları karıştır
            options = options.OrderBy(x => random.Next()).ToList();

            // Şıkları ekrana ekle
            char optionLetter = 'A';
            foreach (double option in options)
            {
                RadioButton radioButton = new RadioButton
                {
                    Content = $"{optionLetter}. {option}",
                    Margin = new Thickness(5),
                    FontSize = 14,
                    Tag = option
                };

                optionsPanel.Children.Add(radioButton);
                optionLetter++;
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            bool answerSelected = false;
            bool isCorrect = false;

            foreach (RadioButton rb in optionsPanel.Children.OfType<RadioButton>())
            {
                if (rb.IsChecked == true)
                {
                    answerSelected = true;
                    double selectedAnswer = (double)rb.Tag;

                    // Hassasiyet toleransını azaltarak tam sayı karşılaştırmaları daha doğru yapalım
                    if (Math.Abs(selectedAnswer - currentQuestion.Answer) < 0.0001)
                    {
                        isCorrect = true;
                        correctAnswers++;
                    }

                    totalQuestions++;
                    break;
                }
            }

            if (!answerSelected)
            {
                MessageBox.Show("Lütfen bir cevap seçiniz!", "Uyarı");
                return;
            }

            // Sonucu göster
            resultTextBlock.Text = isCorrect ? "Doğru Cevap!" : $"Yanlış! Doğru cevap: {currentQuestion.Answer}";
            resultTextBlock.Foreground = isCorrect ? Brushes.Green : Brushes.Red;

            // Açıklama göster
            if (currentQuestion.ExplanationText != null)
            {
                resultTextBlock.Text += $"\n{currentQuestion.ExplanationText}";
            }

            // İstatistikleri güncelle
            scoreTextBlock.Text = $"Skor: {correctAnswers}/{totalQuestions}";
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            resultTextBlock.Text = "";
            GenerateNewQuestion();
        }

        private void HesapMakinesiButon_Click(object sender, RoutedEventArgs e)
        {
            HesapMakinesiWindow hesapMakinesi = new HesapMakinesiWindow();
            hesapMakinesi.Show();
        }

        private void NotAlmaButon_Click(object sender, RoutedEventArgs e)
        {
            NotAlmaWindow notAlma = new NotAlmaWindow();
            notAlma.Show();
        }
    }
    public enum QuestionType
    {
        CommonFactor,          // Standart limit değeri bulma sorusu
        FindCoefficients,      // Katsayıları bulma sorusu
        CoefficientExpression  // Katsayı ifadelerini bulma sorusu
    }

    public class fLimitQuestion
    {
        public string QuestionLaTeX { get; set; }
        public double Answer { get; set; }
        public string QuestionText { get; set; }
        public string ExplanationText { get; set; }
    }

     static class fLimitQuestionGenerator
    {
        private static Random random = new Random();

        // Ortak çarpanlı limit soruları (x-a paydada)
        public static fLimitQuestion GenerateCommonFactorQuestion()
        {
            // Limit noktasını belirle (-10 ile 10 arasında, 0 hariç)
            int a = random.Next(-10, 10);
            if (a >= 0) a++; // 0'ı atla

            // b'yi belirle (-10 ile 10 arasında)
            int b = random.Next(-10, 11);

            // a ve b farklı olmalı
            while (a == b)
            {
                b = random.Next(-10, 11);
            }

            // İfade (x-a)(x+b)/(x-a) şeklinde, sadeleşince (x+b) kalır
            // (x-a)(x+b) ifadesini açalım
            int coefX2 = 1;           // x² katsayısı
            int coefX = b - a;        // x katsayısı
            int constant = -a * b;    // sabit terim

            // x=a yerine koyulduğunda (a+b) elde edilir
            double answer = a + b; // Doğru cevap a+b

            string questionText = $"Limit (x→{a}) (x²+{coefX}x+{constant})/(x-{a})";
            string latexString = $"\\lim_{{x \\to {a}}} \\frac{{x^2{(coefX >= 0 ? "+" : "")}{coefX}x{(constant >= 0 ? "+" : "")}{constant}}}{{x-{a}}}";

            string explanation = $"Bu ifade aslında (x-{a})(x+{b})/(x-{a}) şeklindedir. " +
                                $"Ortak çarpanları sadeleştirince x→{a} için limit (x+{b}) = {a}+{b} = {answer} olur.";

            return new fLimitQuestion
            {
                QuestionText = questionText,
                QuestionLaTeX = latexString,
                Answer = answer,
                ExplanationText = explanation
            };
        }

        // Katsayıları bulma soruları 
        public static fLimitQuestion GenerateFindCoefficientsQuestion()
        {
            // Limit noktasını belirle (-5 ile 5 arasında, 0 hariç)
            int a = random.Next(-5, 5);
            if (a >= 0) a++; // 0'ı atla

            // b'yi belirle (-5 ile 5 arasında)
            int b = random.Next(-5, 6);

            // a ve b farklı olmalı
            while (a == b)
            {
                b = random.Next(-5, 6);
            }

            // Soruyu hangi katsayı için soracağız (d veya e)
            bool askForD = random.Next(2) == 0;

            // (x-a)(x+b) = x²+(b-a)x-ab
            int realCoefX = b - a;  // Gerçek x katsayısı
            int realConstant = -a * b; // Gerçek sabit terim

            double answer;
            string questionText, latexString, explanation;

            if (askForD) // d değerini sor
            {
                answer = realCoefX;

                // limit sonucu (a+b)
                int limitResult = a + b;

                questionText = $"x→{a} için x²+dx+{realConstant})/(x-{a}) limitinin değeri {limitResult} ise d kaçtır?";
                latexString = $"\\text{{Eğer }} \\lim_{{x \\to {a}}} \\frac{{x^2+dx{(realConstant >= 0 ? "+" : "")}{realConstant}}}{{x-{a}}} = {limitResult} \\text{{ ise, d kaçtır?}}";

                explanation = $"Bu limit ifadesinde pay x→{a} için belirsizlik içermelidir. " +
                            $"Bu nedenle x-{a} faktörü payda da vardır.\n" +
                            $"Pay = (x-{a})(x+{b}) = x²+dx{(realConstant >= 0 ? "+" : "")}{realConstant} şeklinde olmalıdır.\n" +
                            $"Çarpımı açarsak: x²+{b}x-{a}x-{a * b} = x²+({b}-{a})x-{a * b}\n" +
                            $"Buradan d = {b}-{a} = {realCoefX} bulunur.";
            }
            else // e değerini sor
            {
                answer = realConstant;

                // limit sonucu (a+b)
                int limitResult = a + b;

                questionText = $"x→{a} için x²+{realCoefX}x+e)/(x-{a}) limitinin değeri {limitResult} ise e kaçtır?";
                latexString = $"\\text{{Eğer }} \\lim_{{x \\to {a}}} \\frac{{x^2{(realCoefX >= 0 ? "+" : "")}{realCoefX}x+e}}{{x-{a}}} = {limitResult} \\text{{ ise, e kaçtır?}}";

                explanation = $"Bu limit ifadesinde pay x→{a} için belirsizlik içermelidir. " +
                            $"Bu nedenle x-{a} faktörü payda da vardır.\n" +
                            $"Pay = (x-{a})(x+{b}) = x²{(realCoefX >= 0 ? "+" : "")}{realCoefX}x+e şeklinde olmalıdır.\n" +
                            $"Çarpımı açarsak: x²+{b}x-{a}x-{a * b} = x²+({b}-{a})x-{a * b}\n" +
                            $"Buradan e = -{a}·{b} = {realConstant} bulunur.";
            }

            return new fLimitQuestion
            {
                QuestionText = questionText,
                QuestionLaTeX = latexString,
                Answer = answer,
                ExplanationText = explanation
            };
        }

        // Katsayı ifadelerini bulma soruları
        public static fLimitQuestion GenerateCoefficientExpressionQuestion()
        {
            // Limit noktasını belirle (-5 ile 5 arasında, 0 hariç)
            int a = random.Next(-5, 5);
            if (a >= 0) a++; // 0'ı atla

            // b'yi belirle (-5 ile 5 arasında)
            int b = random.Next(-5, 6);

            // a ve b farklı olmalı
            while (a == b)
            {
                b = random.Next(-5, 6);
            }

            // (x-a)(x+b) = x²+(b-a)x-ab
            int d = b - a;  // x katsayısı
            int e = -a * b; // sabit terim

            // İfade tipini seç (d+e, 2d-e, 3e-2d, v.b.)
            int expressionType = random.Next(5);

            double answer;
            string questionText, latexString, expressionString, explanation;

            switch (expressionType)
            {
                case 0: // d+e
                    answer = d + e;
                    expressionString = "d + e";
                    explanation = $"d = {d}, e = {e} olduğundan, d + e = {d} + {e} = {answer}";
                    break;
                case 1: // 2d-e
                    answer = 2 * d - e;
                    expressionString = "2d - e";
                    explanation = $"d = {d}, e = {e} olduğundan, 2d - e = 2·{d} - {e} = {2 * d} - {e} = {answer}";
                    break;
                case 2: // 3e-2d
                    answer = 3 * e - 2 * d;
                    expressionString = "3e - 2d";
                    explanation = $"d = {d}, e = {e} olduğundan, 3e - 2d = 3·{e} - 2·{d} = {3 * e} - {2 * d} = {answer}";
                    break;
                case 3: // d²-e
                    answer = d * d - e;
                    expressionString = "d² - e";
                    explanation = $"d = {d}, e = {e} olduğundan, d² - e = {d}² - {e} = {d * d} - {e} = {answer}";
                    break;
                case 4: // 2*(d+e)
                    answer = 2 * (d + e);
                    expressionString = "2(d + e)";
                    explanation = $"d = {d}, e = {e} olduğundan, 2(d + e) = 2·({d} + {e}) = 2·{(d + e)} = {answer}";
                    break;
                default: // e-d²
                    answer = e - d * d;
                    expressionString = "e - d²";
                    explanation = $"d = {d}, e = {e} olduğundan, e - d² = {e} - {d}² = {e} - {d * d} = {answer}";
                    break;
            }

            // limit sonucu (a+b)
            int limitResult = a + b;

            questionText = $"x→{a} için x²+dx+e)/(x-{a}) limitinin değeri {limitResult} ise {expressionString} kaçtır?";
            latexString = $"\\text{{Eğer }} \\lim_{{x \\to {a}}} \\frac{{x^2+dx+e}}{{x-{a}}} = {limitResult} \\text{{ ise, {expressionString} kaçtır?}}";

            explanation = $"Bu limit ifadesinde pay x→{a} için belirsizlik içermelidir.\n" +
                        $"Bu nedenle x-{a} faktörü payda da vardır.\n" +
                        $"Pay = (x-{a})(x+{b}) = x²+dx+e şeklinde olmalıdır.\n" +
                        $"Çarpımı açarsak: x²+{b}x-{a}x-{a * b} = x²+({b}-{a})x-{a * b}\n" +
                        $"Katsayıları karşılaştırırsak:\n" +
                        $"d = {b}-{a} = {d}\n" +
                        $"e = -{a}·{b} = {e}\n\n" +
                        explanation;

            return new fLimitQuestion
            {
                QuestionText = questionText,
                QuestionLaTeX = latexString,
                Answer = answer,
                ExplanationText = explanation
            };
        }
    }
}
