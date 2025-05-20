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

        public OturumSoruGosterWPF(string baslik)
        {
            OturumSorulariMVM MVVM = new OturumSorulariMVM(baslik);

            InitializeComponent();

            this.DataContext = MVVM;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }

    internal class OturumSorulariMVM : ObservableObject
    {
        public ObservableCollection<RaporCardModel> Sorular { get; set; }
        public string title { get; set; }

        public OturumSorulariMVM(string baslik)
        {
            title = baslik;

            Sorular = new ObservableCollection<RaporCardModel>();

            var examEntities = Genel.dbEntities.EXAM_SESSION_DETAILS.Where(x => x.EXAM_SESSIONS.EXAM_TITLE == title && x.EXAM_SESSIONS.USERID == aktifKullanici.kullnId).ToList();

            foreach (var item in examEntities)
            {
                List<string> diger = new List<string>();

                string[] splitString = item.WRONG_ANSWERS.Split('#');
                foreach (string sec in splitString)
                {
                    diger.Add(sec);
                }

                RaporCardModel card = new RaporCardModel(item.LATEX_TEXT, item.CORRECT_ANSWER, item.TOPICS.TOPIC, diger, item.USERS_ANSWER);

                Sorular.Add(card);
            }

            OnPropertyChanged(nameof(Sorular));
            OnPropertyChanged();
        }
    }


    class RaporCardModel : IStyleAnahtar
    {
        public string soruStyle { get => SoruStyleGetir(); }
        public string soruTurstring { get; set; }
        public string LaTeX { get; set; }
        public string Metin { get; set; }
        public string Sonuc { get; set; }
        public string Cevaplanan { get; set; }

        public List<string> DigerSecenekler { get; set; }

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
          
        public string SoruStyleGetir()
        {
            switch (soruTurstring)
            {
                case "Problem":
                    return "SoruModuMetin";
                default:
                    return "SoruModuNormal";
            }
        }
    }

    class RaporCardSelector : StyleSelector
    {
        public Style LaTeXStyle { get; set; }
        public Style MetinStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is IStyleAnahtar anahtar)
            {

                string style = ((IStyleAnahtar)item).soruStyle;

                switch (style)
                {
                    case "SoruModuNormal":
                        return LaTeXStyle;
                    case "SoruModuMetin":
                        return MetinStyle;
                    default:
                        return LaTeXStyle;
                }
                ;
            }
            else
            {
                return base.SelectStyle(item, container);
            }
        }
    }
}
