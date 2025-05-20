using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using DataVis = System.Windows.Forms.DataVisualization;
using NeptunMathWPF;
using System.Data.Entity;
using System;
using GenericTensor.Functions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace AnasayfaWPF
{
    public enum lbType
    {
        oturumbazli,
        konubazli
    }
    public partial class RaporFormu : Window
    {
        lbType lbtype;
        public RaporFormu()
        {
            InitializeComponent();

            Chart1.Series.Clear();
            Chart1.Series.Add("BOŞ");

            #region ChartStyle
            Chart1.BackColor = Color.White;
            Chart1.ChartAreas[0].BackColor = Color.White;
            Chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            Chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            Chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            Chart1.BorderlineColor = Color.LightGray;
            Chart1.BorderlineWidth = 1;
            Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 10);
            Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 10);

            if (Chart1.Legends.Count == 0)
            {
                Chart1.Legends.Add(new Legend("Default"));
            }
            Chart1.Legends[0].Font = new Font("Segoe UI", 10);
            Chart1.Legends[0].Docking = Docking.Top;
            Chart1.Legends[0].Alignment = StringAlignment.Center;

            #endregion
        }

        private void GeriDon_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGenelDegerlendirme_Click(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (Genel.dbEntities.EXAM_SESSIONS.Count() >= 2)
                {
                    lstRaporVerileri.Items.Clear();
                    Chart1.Series.Clear();
                    Chart1.Series.Add("Puan");
                    Chart1.Series[0].ChartType = DataVis.Charting.SeriesChartType.Line;
                    Chart1.Series[0].BorderWidth = 4;
                    Chart1.ChartAreas[0].AxisY.Maximum = 100;
                        
                    var examList = Genel.dbEntities.EXAM_SESSIONS.Where(x => x.USERID == aktifKullanici.kullnId).Select(x => new
                    {
                        x.SCORE,
                        x.EXAM_TITLE
                    }).ToList();

                    foreach (var exam in examList)
                    {
                        Chart1.Series[0].Points.Add(exam.SCORE, 0).AxisLabel = exam.EXAM_TITLE;
                    }

                }
                else
                {
                    MessageBox.Show("Bu özellik için en az 2 sınava girmiş olmanız gerekiyor.", "UYARI!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }


        private void btnKonuBazli_Click(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {

                lbtype = lbType.konubazli;
                lstRaporVerileri.Items.Clear();
                var topicids = Genel.dbEntities.EXAM_SESSION_DETAILS.Select(x => x.TOPIC_ID).Distinct().ToList();

                foreach (var topicid in topicids)
                {
                    string topic = Genel.dbEntities.TOPICS.Where(x => x.TOPIC_ID == topicid).Select(x => x.TOPIC).FirstOrDefault();
                    lstRaporVerileri.Items.Add(topic);
                }
            });
        }

        private void btnOturumBazli_Click(object sender, RoutedEventArgs e)
        {
            Genel.Handle(() =>
            {

                lbtype = lbType.oturumbazli;
                lstRaporVerileri.Items.Clear();

                var examList = Genel.dbEntities.EXAM_SESSIONS.Where(x => x.USERID == aktifKullanici.kullnId).OrderByDescending(x => x.EXAM_ID).Select(x => x.EXAM_TITLE).ToList();
                foreach (var exam in examList)
                {
                    lstRaporVerileri.Items.Add(exam);
                }
            });
        }

        private void btnTrueFalse_Click(object sender, RoutedEventArgs e)
        {


        }

        private void lstRaporVerileri_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Genel.Handle(() =>
            {
                if (lstRaporVerileri.SelectedItems.Count > 0)
                {
                    if (lbtype == lbType.oturumbazli)
                    {
                        Chart1.ChartAreas[0].AxisY.Maximum = 20;
                        Chart1.Series.Clear();
                        var dogruSerisi = new Series("Doğru");
                        var yanlisSerisi = new Series("Yanlış");

                        dogruSerisi.ChartType = SeriesChartType.Column;
                        yanlisSerisi.ChartType = SeriesChartType.Column;

                        string title = lstRaporVerileri.SelectedItem.ToString();
                        int examId = Genel.dbEntities.EXAM_SESSIONS.Where(x => x.EXAM_TITLE == title && x.USERID == aktifKullanici.kullnId).Select(x => x.EXAM_ID).FirstOrDefault();

                        var topicsIds = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.EXAM_ID == examId).Select(x => x.TOPIC_ID).Distinct().ToList();
                        foreach (var topicid in topicsIds)
                        {
                            string topic = Genel.dbEntities.TOPICS.Where(x => x.TOPIC_ID == topicid).Select(x => x.TOPIC).FirstOrDefault();
                            int count = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.EXAM_ID == examId && x.TOPIC_ID == topicid).Count();
                            int correct = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.EXAM_ID == examId && x.TOPIC_ID == topicid && x.ISCORRECT == true).Count();
                            int incorrect = count - correct;

                            dogruSerisi.Points.AddXY(topic, correct);
                            yanlisSerisi.Points.AddXY(topic, incorrect);
                        }

                        dogruSerisi.Color = Color.FromArgb(255, 72, 201, 176);
                        yanlisSerisi.Color = Color.FromArgb(255, 231, 76, 60);
                        Chart1.Series.Add(dogruSerisi);
                        Chart1.Series.Add(yanlisSerisi);
                    }


                    else if (lbtype == lbType.konubazli)
                    {
                        Chart1.ChartAreas[0].AxisY.Maximum = double.NaN;
                        Chart1.Series.Clear();
                        string topic = lstRaporVerileri.SelectedItem.ToString();
                        int topicid = Genel.dbEntities.TOPICS.Where(x => x.TOPIC == topic).Select(x => x.TOPIC_ID).FirstOrDefault();
                        var subtopics = Genel.dbEntities.SUBTOPICS.Where(x => x.TOPIC_ID == topicid).Select(x => x.SUBTOPIC_ID).ToList();


                        var dogruSerisi = new Series("Doğru");
                        var yanlisSerisi = new Series("Yanlış");

                        dogruSerisi.ChartType = SeriesChartType.Column;
                        yanlisSerisi.ChartType = SeriesChartType.Column;


                        foreach (var subtopicid in subtopics)
                        {
                            int count = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.SUBTOPIC_ID == subtopicid).Count();
                            int correct = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.SUBTOPIC_ID == subtopicid && x.ISCORRECT == true).Count();
                            int incorrect = count - correct;
                            string subtopic = Genel.dbEntities.SUBTOPICS.Where(x => x.SUBTOPIC_ID == subtopicid).Select(x => x.SUBTOPIC).FirstOrDefault();

                            dogruSerisi.Points.AddXY(subtopic, correct);
                            yanlisSerisi.Points.AddXY(subtopic, incorrect);
                        }

                        dogruSerisi.Color = Color.FromArgb(255, 72, 201, 176);
                        yanlisSerisi.Color = Color.FromArgb(255, 231, 76, 60);
                        Chart1.Series.Add(dogruSerisi);
                        Chart1.Series.Add(yanlisSerisi);
                    }
                }
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}