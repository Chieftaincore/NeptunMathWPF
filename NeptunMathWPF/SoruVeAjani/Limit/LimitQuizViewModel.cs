using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using static NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.EtkilesimMVM;

namespace NeptunMathWPF.SoruVeAjani.Limit
{
    // Burası Limit sorularını yazdıran yer bu projeye göre entegre edilmesi gerekecek
    public class LimitQuizViewModel : INotifyPropertyChanged
    {
        private LimitQuestion _currentQuestion;
        private int _correctAnswers;
        private int _totalQuestions;
        private Random _random = new Random();

        public ObservableCollection<OptionViewModel> Options { get; } = new ObservableCollection<OptionViewModel>();
        public ICommand CheckCommand { get; }
        public ICommand NextCommand { get; }

        private string _formula;
        public string Formula
        {
            get => _formula;
            set { _formula = value; OnPropertyChanged(nameof(Formula)); }
        }

        private string _questionTypeText;
        public string QuestionTypeText
        {
            get => _questionTypeText;
            set { _questionTypeText = value; OnPropertyChanged(nameof(QuestionTypeText)); }
        }

        private string _resultText;
        public string ResultText
        {
            get => _resultText;
            set { _resultText = value; OnPropertyChanged(nameof(ResultText)); }
        }

        private Brush _resultColor = Brushes.Black;
        public Brush ResultColor
        {
            get => _resultColor;
            set { _resultColor = value; OnPropertyChanged(nameof(ResultColor)); }
        }
        // Sanırım skora gerek yok
        private string _scoreText = "Skor: 0/0";
        public string ScoreText
        {
            get => _scoreText;
            set { _scoreText = value; OnPropertyChanged(nameof(ScoreText)); }
        }

        public LimitQuizViewModel()
        {
            // Butonları ayarlayınca burada ayarlanacak
            CheckCommand = new RelayCommand(CheckAnswer);
            NextCommand = new RelayCommand(GenerateNewQuestion);
            GenerateNewQuestion();
        }

        private void GenerateNewQuestion()
        {
            Options.Clear();
            ResultText = "";
            ResultColor = Brushes.Black;

            var questionTypes = Enum.GetValues(typeof(LimitQuestionType));
            var questionType = (LimitQuestionType)questionTypes.GetValue(_random.Next(questionTypes.Length));

            switch (questionType)
            {
                case LimitQuestionType.CommonFactor:
                    _currentQuestion = LimitQuestionGenerator.GenerateCommonFactorQuestion();
                    break;
                case LimitQuestionType.FindCoefficients:
                    _currentQuestion = LimitQuestionGenerator.GenerateFindCoefficientsQuestion();
                    break;
                case LimitQuestionType.CoefficientExpression:
                    _currentQuestion = LimitQuestionGenerator.GenerateCoefficientExpressionQuestion();
                    break;
                default:
                    throw new NotImplementedException();
            }

            Formula = _currentQuestion.QuestionLaTeX;
            QuestionTypeText = GetQuestionTypeText(questionType);

            // Şıkları oluştur
            var options = new List<double> { _currentQuestion.Answer };
            while (options.Count < 4)
            {
                double wrongOption = questionType == LimitQuestionType.CommonFactor
                    ? _currentQuestion.Answer + _random.Next(-10, 11)
                    : _currentQuestion.Answer + _random.Next(-5, 6);

                if (!options.Contains(wrongOption) && Math.Abs(wrongOption - _currentQuestion.Answer) > 0.001)
                    options.Add(wrongOption);
            }

            options = options.OrderBy(x => _random.Next()).ToList();
            char optionLetter = 'A';
            foreach (var option in options)
            {
                Options.Add(new OptionViewModel
                {
                    Text = $"{optionLetter}. {option}",
                    Value = option,
                    IsSelected = false
                });
                optionLetter++;
            }
        }

        private string GetQuestionTypeText(LimitQuestionType type)
        {
            switch (type)
            {
                case LimitQuestionType.CommonFactor:
                    return "Soru Tipi: Limit Değerini Bulma";
                case LimitQuestionType.FindCoefficients:
                    return "Soru Tipi: Katsayıları Bulma";
                case LimitQuestionType.CoefficientExpression:
                    return "Soru Tipi: Katsayı İfadesini Bulma";
                default:
                    return "";
            }
        }

        private void CheckAnswer()
        {
            var selectedOption = Options.FirstOrDefault(o => o.IsSelected);
            if (selectedOption == null)
            {
                ResultText = "Lütfen bir cevap seçiniz!";
                ResultColor = Brushes.Red;
                return;
            }

            _totalQuestions++;
            bool isCorrect = Math.Abs(selectedOption.Value - _currentQuestion.Answer) < 0.0001;

            if (isCorrect) _correctAnswers++;

            ResultText = isCorrect
                ? "Doğru Cevap!"
                : $"Yanlış! Doğru cevap: {_currentQuestion.Answer}";

            if (!string.IsNullOrEmpty(_currentQuestion.ExplanationText))
                ResultText += $"\n{_currentQuestion.ExplanationText}";

            ResultColor = isCorrect ? Brushes.Green : Brushes.Red;
            ScoreText = $"Skor: {_correctAnswers}/{_totalQuestions}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OptionViewModel : INotifyPropertyChanged
    {
        public string Text { get; set; }
        public double Value { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

