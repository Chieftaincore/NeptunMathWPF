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
                    if (SubtopicsListBox.SelectedItems.Count>0)
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
            }
            catch (System.NullReferenceException) { }
        }

        private void SubtopicsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GetStatistic();
            }
            catch (System.NullReferenceException) { }
        }
    }
}
