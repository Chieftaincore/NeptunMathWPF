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
using System.Xml.Linq;

namespace NeptunMathWPF.Formlar
{
    /// <summary>
    /// Interaction logic for IstatistikWPF.xaml
    /// </summary>
    public partial class IstatistikWPF : Window
    {
        public IstatistikWPF()
        {
            InitializeComponent();
            LoadTopics();
        }

        private void GetStatistic()
        {
            Genel.Handle(() =>
            {
                try
                {
                    if (SubtopicsListBox.SelectedItems.Count > 0)
                    {
                        string subtopic = SubtopicsListBox.SelectedItem.ToString();
                        int subtopicid = Genel.dbEntities.SUBTOPICS.Where(x => x.SUBTOPIC == subtopic).Select(x => x.SUBTOPIC_ID).FirstOrDefault();
                        lblYanlisCount.Content = Genel.dbEntities.WRONG_ANSWERED_QUESTIONS.Where(x => x.SUBTOPIC_ID == subtopicid).Count();
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
            try
            {
                GetStatistic();
                Genel.Handle(() =>
                {
                    string topicstr = TopicsListBox.SelectedItem.ToString();
                    var topicId = Genel.dbEntities.TOPICS.Where(x => x.TOPIC == topicstr).Select(x => x.TOPIC_ID).FirstOrDefault();
                    var subtopicId = Genel.dbEntities.SUBTOPICS.Where(x => x.TOPIC_ID == topicId).Select(x => x.SUBTOPIC_ID).FirstOrDefault();

                    QuestionsListBox.ItemsSource = Genel.dbEntities.WRONG_ANSWERED_QUESTIONS.Where(x => x.SUBTOPIC_ID == subtopicId).Select(x => x.QUESTION_TEXT).ToList();
                });
            }
            catch (System.NullReferenceException) { }
        }

        private void QuestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuestionsListBox.SelectedItem!=null)
            {

                Genel.Handle(() =>
                {
                    lblA.Content = "A)";
                    lblA.Foreground = new SolidColorBrush(Colors.Black);
                    lblB.Content = "B)";
                    lblB.Foreground = new SolidColorBrush(Colors.Black);
                    lblC.Content = "C)";
                    lblC.Foreground = new SolidColorBrush(Colors.Black);
                    lblD.Content = "D)";
                    lblD.Foreground = new SolidColorBrush(Colors.Black);
                    lblE.Content = "E)";
                    lblE.Foreground = new SolidColorBrush(Colors.Black);

                    string sorustr = QuestionsListBox.SelectedItem.ToString();
                    var soru = Genel.dbEntities.WRONG_ANSWERED_QUESTIONS.Where(x => x.QUESTION_TEXT == sorustr).FirstOrDefault();
                    lblSoru.Content = soru.QUESTION_TEXT;
                    List<string> answers = new List<string>();
                    foreach (var item in soru.WRONG_ANSWERS.Split('#'))
                    {
                        if (!string.IsNullOrEmpty(item))
                            answers.Add(item);
                    }
                    answers.Add(soru.ANSWER);
                    Genel.Shuffle(answers);


                    CheckTrueOrWrong(answers[0], lblA, soru.ANSWER, soru.USERS_ANSWER);
                    CheckTrueOrWrong(answers[1], lblB, soru.ANSWER, soru.USERS_ANSWER);
                    CheckTrueOrWrong(answers[2], lblC, soru.ANSWER, soru.USERS_ANSWER);
                    CheckTrueOrWrong(answers[3], lblD, soru.ANSWER, soru.USERS_ANSWER);

                    if (soru.WRONG_ANSWERS.Split('#')[4] != null)
                    {
                        CheckTrueOrWrong(answers[4], lblE, soru.ANSWER, soru.USERS_ANSWER);
                    }
                });
            }
        }

        private void CheckTrueOrWrong(string answer, Label lbl, string correctanswer, string usersanswer)
        {
            lbl.Content += answer;
            if (answer == correctanswer)
                lbl.Foreground = new SolidColorBrush(Colors.Green);
            else if (answer == usersanswer)
                lbl.Foreground = new SolidColorBrush(Colors.Red);
        }
    }
}
