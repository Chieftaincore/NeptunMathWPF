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
    /// Interaction logic for BookmarkedQuestionsPanelWPF.xaml
    /// </summary>
    public partial class BookmarkedQuestionsPanelWPF : Window
    {
        public BookmarkedQuestionsPanelWPF()
        {
            InitializeComponent();
            LoadTopics();
        }

        private void GetQuestionCount()
        {
            Genel.Handle(() =>
            {
                try
                {
                    if (SubtopicsListBox.SelectedItems.Count > 0)
                    {
                        string subtopic = SubtopicsListBox.SelectedItem.ToString();
                        int subtopicid = Genel.dbEntities.SUBTOPICS.Where(x => x.SUBTOPIC == subtopic).Select(x => x.SUBTOPIC_ID).FirstOrDefault();
                        lblKaydedilenCount.Content = Genel.dbEntities.BOOKMARKED_QUESTIONS.Where(x => x.SUBTOPIC_ID == subtopicid).Count();
                    }
                }
                catch (System.NullReferenceException)
                { }
            });
        }

        private void LoadTopics()
        {
            Genel.Handle(() =>
            {
                try
                {
                    // YAZDIRMAK İÇİN
                    TopicsListBox.ItemsSource = Genel.dbEntities.TOPICS.Select(x => x.TOPIC).ToList();
                }
                catch (System.NullReferenceException) { }
            });
        }

        private void TopicsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int topicid = Genel.dbEntities.TOPICS.Where(x => x.TOPIC == TopicsListBox.SelectedItem).Select(x => x.TOPIC_ID).FirstOrDefault();
                SubtopicsListBox.ItemsSource = Genel.dbEntities.SUBTOPICS.Where(x => x.TOPIC_ID == topicid).Select(x => x.SUBTOPIC).ToList();
                QuestionsListBox.ItemsSource = null;
            }
            catch (System.NullReferenceException) { }
        }

        private void SubtopicsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (SubtopicsListBox.SelectedItem != null)
                {

                    GetQuestionCount();
                    string subtopic = SubtopicsListBox.SelectedItem.ToString();
                    var subtopicId = Genel.dbEntities.SUBTOPICS.Where(x => x.SUBTOPIC == subtopic).Select(x => x.SUBTOPIC_ID).FirstOrDefault();

                    QuestionsListBox.ItemsSource = Genel.dbEntities.BOOKMARKED_QUESTIONS.Where(x => x.SUBTOPIC_ID == subtopicId).Select(x => x.QUESTION_TEXT).ToList();
                }
            });
        }

        private void QuestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuestionsListBox.SelectedItem != null)
            {
                Genel.Handle(() =>
                {
                    Uniform.Children.Clear();

                    string sorustr = QuestionsListBox.SelectedItem.ToString();
                    var soru = Genel.dbEntities.BOOKMARKED_QUESTIONS.Where(x => x.QUESTION_TEXT == sorustr).FirstOrDefault();
                    lblSoru.Text = soru.QUESTION_TEXT;
                    List<string> answers = new List<string>();

                    char optchar = 'A';
                    foreach (var item in soru.WRONG_ANSWERS.Split('#'))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            answers.Add(item);
                        }
                    }
                    answers.Add(soru.CORRECT_ANSWER);
                    Genel.Shuffle(answers);

                    for (int i = 0; i < answers.Count; i++)
                    {

                        Label lbl = new Label();
                        lbl.Foreground = new SolidColorBrush(Colors.Black);

                        lbl.Content = $"{optchar})";
                        Uniform.Children.Add(lbl);

                        CheckTrueOrWrong(answers[i], lbl, soru.CORRECT_ANSWER);
                        optchar++;
                    }
                });
            }
        }

        private void CheckTrueOrWrong(string answer, Label lbl, string correctanswer)
        {
            lbl.Content += answer;
            if (answer == correctanswer)
                lbl.Foreground = new SolidColorBrush(Colors.Green);
        }
    }
}
