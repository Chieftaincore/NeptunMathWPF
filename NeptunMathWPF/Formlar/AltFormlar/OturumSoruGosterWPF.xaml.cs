using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NeptunMathWPF.Formlar.AltFormlar
{
    /// <summary>
    /// OturumSoruGosterWPF.xaml etkileşim mantığı
    /// </summary>
    public partial class OturumSoruGosterWPF : Window
    {
        string title;

        Collection<SoruCardModel> Sorular;

        public OturumSoruGosterWPF(string baslik)
        {
            InitializeComponent();

            title = baslik;

            var examEntities = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.EXAM_SESSIONS.EXAM_TITLE == title && x.EXAM_SESSIONS.USERID == aktifKullanici.kullnId).ToList();

            foreach(var item in examEntities)
            {
                List<string> diger = new List<string>();

                string[] splitString = item.WRONG_ANSWERS.Split('#');
                foreach (string sec in splitString)
                {
                    diger.Add(sec);
                }

            }

            listViewSorular.ItemsSource = Sorular;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }

    class RaporCardModel : IStyleAnahtar
    {
        public string soruStyle { get; set; }
        string soruTurstring { get; set; }

        string LaTeX;

        string Sonuc;

        string Cevaplanan;

        List<string> DigerSecenekler;

        public SolidColorBrush TabBrush { get; set; }

        private Color _tabRenk;
        public Color TabRenk
        {
            get => _tabRenk; set
            {

                if (_tabRenk != value)
                {
                    _tabRenk = value;
                    TabBrush = new SolidColorBrush(TabRenk);
                }
            }
        }


        public RaporCardModel(string _LaTeX, string _Sonuc, string _SoruTur, List<string> Secenekler, string _Cevaplanan)
        {
            LaTeX = _LaTeX;
            soruTurstring = _SoruTur;
            Sonuc = _Sonuc;
            DigerSecenekler = Secenekler;

            Cevaplanan = _Cevaplanan;

            if(Cevaplanan == Sonuc)
            {
                TabRenk = Colors.LightSkyBlue;
            }
            else
            {
                TabRenk = Colors.IndianRed;
            }
        }

          
    }
}
