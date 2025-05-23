﻿using System;
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
    /// Interaction logic for konuEkleUC.xaml
    /// </summary>
    public partial class konuEkleUC : UserControl
    {
        public konuEkleUC()
        {
            InitializeComponent();
            LoadTopics();
            LoadSubtopics();
        }

        private void addTopicButton_Click(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                int topicId = Genel.dbEntities.TOPICS.Where(x => x.TOPIC == topicTextBox.Text).Select(x => x.TOPIC_ID).FirstOrDefault();
                if (topicTextBox.Text != string.Empty && topicId == 0)
                {
                    Genel.ReloadEntity();
                    string topic = topicTextBox.Text;
                    Genel.dbEntities.TOPICS.Add(new TOPICS
                    {
                        TOPIC = topic
                    });
                    Genel.dbEntities.SaveChanges();
                    MessageBox.Show("Başarıyla eklendi!");
                    LoadTopics();
                    LoadSubtopics();
                }
            });
        }

        private void addSubtopicButton_Click(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                int subtopicId = Genel.dbEntities.SUBTOPICS.Where(x => x.SUBTOPIC == subtopicTextBox.Text).Select(x => x.SUBTOPIC_ID).FirstOrDefault();

                if (topicComboBox.Text != string.Empty && subtopicTextBox.Text != string.Empty && subtopicId==0)
                {

                    Genel.ReloadEntity();
                    string subtopic = subtopicTextBox.Text;
                    string _topic = topicComboBox.Text;
                    var topic = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == _topic);
                    Genel.dbEntities.SUBTOPICS.Add(new SUBTOPICS
                    {
                        SUBTOPIC = subtopic,
                        TOPICS = topic
                    });
                    Genel.dbEntities.SaveChanges();
                    MessageBox.Show("Başarıyla eklendi!");
                    LoadSubtopics();
                }
            });
        }

        private void LoadSubtopics()
        {
            Genel.Handle(() =>
            {
                subtopicComboBox.ItemsSource = Genel.dbEntities.SUBTOPICS.Select(x => x.SUBTOPIC).ToList();
            });
        }

        private void LoadTopics()
        {
            Genel.Handle(() =>
            {
                topicComboBox.ItemsSource = Genel.dbEntities.TOPICS.Select(x => x.TOPIC).ToList();
                topicComboBox2.ItemsSource = Genel.dbEntities.TOPICS.Select(x => x.TOPIC).ToList();
            });
        }

        private void rmvTopicButton_Click(object sender, RoutedEventArgs e)
        {
            if (topicComboBox2.SelectedIndex != -1)
            {

                Genel.Handle(() =>
                {
                    Genel.ReloadEntity();
                    try
                    {
                        Genel.ReloadEntity();
                        string topic = topicComboBox2.SelectedItem.ToString();
                        var entityToDelete = Genel.dbEntities.TOPICS.FirstOrDefault(x => x.TOPIC == topic);
                        Genel.dbEntities.TOPICS.Attach(entityToDelete);
                        Genel.dbEntities.TOPICS.Remove(entityToDelete);
                        Genel.dbEntities.SaveChanges();
                        MessageBox.Show("Başarıyla silindi!");
                        LoadTopics();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException)
                    {
                        MessageBox.Show("Bu veriye bağlı veriler bulunuyor!");
                    }
                });
            }
        }

        private void rmvSubtopicButton_Click(object sender, RoutedEventArgs e)
        {
            // eğer aynı alt konu ismi farklı konularda varsa topic id kontrolü de eklenebilir
            if (subtopicComboBox.SelectedIndex != -1)
            {
                Genel.Handle(() =>
                {
                    Genel.ReloadEntity();
                    try
                    {

                        string subtopic = subtopicComboBox.SelectedItem.ToString(); ;
                        var entityToDelete = Genel.dbEntities.SUBTOPICS.FirstOrDefault(x => x.SUBTOPIC == subtopic);
                        Genel.dbEntities.SUBTOPICS.Attach(entityToDelete);
                        Genel.dbEntities.SUBTOPICS.Remove(entityToDelete);
                        Genel.dbEntities.SaveChanges();
                        MessageBox.Show("Başarıyla silindi!");
                        LoadSubtopics();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException)
                    {
                        MessageBox.Show("Bu veriye bağlı veriler bulunuyor!");
                    }
                });
            }
        }
    }
}
