using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using DataVis = System.Windows.Forms.DataVisualization;
using NeptunMathWPF;
using System.Data.Entity;
using System;

namespace AnasayfaWPF
{
    public enum lbType
    {
        oturumbazli
    }
    public partial class RaporFormu : Window
    {
        lbType lbtype;
        public RaporFormu()
        {
            InitializeComponent();

            btnGenelDegerlendirme_Click(null, null); 
        }

        private void GeriDon_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 


        }

        private void btnGenelDegerlendirme_Click(object sender, RoutedEventArgs e)
        {


        }


        private void btnKonuBazli_Click(object sender, RoutedEventArgs e)
        {
            lbtype = lbType.oturumbazli;
            lstRaporVerileri.Items.Clear();
            var topicids = Genel.dbEntities.EXAM_SESSION_DETAILS.Select(x => x.TOPIC_ID).Distinct().ToList();

            foreach (var topicid in topicids)
            {
                MessageBox.Show(topicid.ToString());
                string topic = Genel.dbEntities.TOPICS.Where(x => x.TOPIC_ID == topicid).Select(x => x.TOPIC).FirstOrDefault();
                lstRaporVerileri.Items.Add(topic);
            }
        }

        private void btnOturumBazli_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnTrueFalse_Click(object sender, RoutedEventArgs e)
        {


        }

        private void lstRaporVerileri_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Chart1.Series[0].Points.Clear();
            if (lbtype==lbType.oturumbazli)
            {
                string topic = lstRaporVerileri.SelectedItem.ToString();
                int topicid = Genel.dbEntities.TOPICS.Where(x => x.TOPIC == topic).Select(x => x.TOPIC_ID).FirstOrDefault();
                var subtopics = Genel.dbEntities.SUBTOPICS.Where(x => x.TOPIC_ID == topicid).Select(x => x.SUBTOPIC_ID).ToList();

                foreach (var subtopicid in subtopics)
                {
                    int count = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.SUBTOPIC_ID == subtopicid).Count();
                    int correct = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.SUBTOPIC_ID == subtopicid && x.ISCORRECT==true).Count();
                    int incorrect = count - correct;

                    Chart1.Series[0].Points.Add(count, 0).AxisLabel = Genel.dbEntities.SUBTOPICS.Where(x => x.SUBTOPIC_ID == subtopicid).Select(x => x.SUBTOPIC).FirstOrDefault();
                }
            }
        }
    }
}