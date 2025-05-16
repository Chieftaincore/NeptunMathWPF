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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NeptunMathWPF.Formlar
{
    /// <summary>
    /// Interaction logic for soruHavuzuUC.xaml
    /// </summary>
    public partial class soruHavuzuUC : UserControl
    {
        public soruHavuzuUC()
        {
            InitializeComponent();
            LoadTopics();
            LoadListBox();
        }

        private void LoadTopics()
        {
            Genel.Handle(() =>
            {
                topicComboBox.ItemsSource = Genel.dbEntities.TOPICS.Select(x => x.TOPIC).ToList();
            });
        }

        private void LoadSubtopics()
        {
            Genel.Handle(() =>
            {
                string topic = topicComboBox.SelectedItem.ToString();
                subtopicComboBox.ItemsSource = Genel.dbEntities.SUBTOPICS.Where(x => x.TOPICS.TOPIC == topic).Select(x => x.SUBTOPIC).ToList();
            });
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (topicComboBox.Text != string.Empty && subtopicComboBox.Text != string.Empty &&
                soruTextBox.Text != string.Empty && latexTextBox.Text != string.Empty && dogrucevapTextBox.Text != string.Empty
                && yanliscevapTextBox.Text != string.Empty
                )
            {

                Genel.Handle(() =>
                {
                    Genel.ReloadEntity();
                    var topic = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == topicComboBox.Text);
                    var subtopic = Genel.dbEntities.SUBTOPICS.FirstOrDefault(x => x.SUBTOPIC == subtopicComboBox.Text);

                    Genel.dbEntities.QUESTION_POOL.Add(new QUESTION_POOL
                    {
                        TOPICS = topic,
                        SUBTOPICS = subtopic,
                        QUESTION_TEXT = soruTextBox.Text,
                        LATEX_TEXT = latexTextBox.Text,
                        CORRECT_ANSWER = dogrucevapTextBox.Text,
                        WRONG_ANSWERS = yanliscevapTextBox.Text
                    });
                    Genel.dbEntities.SaveChanges();
                    MessageBox.Show("Başarıyla eklendi!");
                    LoadListBox();
                });
            }
        }
        private void LoadListBox()
        {
            Genel.Handle(() =>
            {
                listbox1.ItemsSource = Genel.dbEntities.QUESTION_POOL.Select(x => x.QUESTION_TEXT).ToList();
            });
        }

        private void rmvButton_Click(object sender, RoutedEventArgs e)
        {
            if (listbox1.SelectedItems.Count > 0)
            {
                Genel.Handle(() =>
                {
                    try
                    {
                        Genel.ReloadEntity();
                        string qtext = listbox1.SelectedItem.ToString();
                        var question = Genel.dbEntities.QUESTION_POOL.FirstOrDefault(x => x.QUESTION_TEXT == qtext);
                        Genel.dbEntities.QUESTION_POOL.Attach(question);
                        Genel.dbEntities.QUESTION_POOL.Remove(question);
                        Genel.dbEntities.SaveChanges();
                        MessageBox.Show("Başarıyla silindi!");
                        LoadListBox();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException)
                    {
                        MessageBox.Show("Bu veriye bağlı veriler bulunuyor!");
                    }
                });
            }
        }

        private void topicComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (topicComboBox.SelectedIndex != -1)
            {
                LoadSubtopics();
            }
        }
    }
}
