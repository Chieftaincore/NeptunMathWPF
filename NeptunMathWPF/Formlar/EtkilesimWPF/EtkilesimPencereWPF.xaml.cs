using HesapMakinesi;
using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM;
using NeptunMathWPF.SoruVeAjani.Algorithma;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NeptunMathWPF.Formlar
{
    /// <summary>
    /// Genellikle Pencereyi kapsayan tuşlar veya MVVM dışı eventler içindir
    /// formun diğer özellikleri için EtkilesimMVVM'e bakınız
    /// </summary>
    /// 
    public partial class EtkilesimPencereWPF : Window
    {
        Window Onceki;



        public EtkilesimPencereWPF()
        {
            if(this.DataContext == null)
                this.DataContext = new EtkilesimMVM();

            InitializeComponent();
        }

        public EtkilesimPencereWPF(Window _onceki)
        {
            if (this.DataContext == null)
                this.DataContext = new EtkilesimMVM();

            InitializeComponent();

            Genel.Handle(() =>
            {
                if (DataContext.GetType() == typeof(TestEtkilesimMVM))
                {
                    MessageBox.Show("EventEklendi");

                    ((TestEtkilesimMVM)this.DataContext).TestBittiEvent += TestBitti;
                }
            });

            Onceki = _onceki;
        }

        internal EtkilesimPencereWPF(EtkilesimMVM MVVM, Window _onceki)
        {
            this.DataContext = MVVM;

            InitializeComponent();

            Genel.Handle(() =>
            {
                if (DataContext.GetType() == typeof(TestEtkilesimMVM))
                {
                    ((TestEtkilesimMVM)this.DataContext).TestBittiEvent += TestBitti;
                }
            });

            Onceki = _onceki;
        }

        public EtkilesimPencereWPF(Window _onceki, SoruTerimleri.soruTuru[] Turler)
        {
            if (this.DataContext == null)
                this.DataContext = new EtkilesimMVM();

            InitializeComponent();
            
            ((EtkilesimMVM)this.DataContext).Algorithma = new AlgorithmaModel(turler: Turler);

            Onceki = _onceki;
        }

        private void HesapMakinesi_Click(object sender, RoutedEventArgs e)
        {
            HesapMakinesiWindow hesapMakinesiWindow = new HesapMakinesiWindow();
            hesapMakinesiWindow.Show();
        }

        private void YerelHesapMakinesi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("calc.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yerel hesap makinesi açılırken hata oluştu: " + ex.Message);
            }
        }

        private void NotAlma_Click(object sender, RoutedEventArgs e)
        {
            NotAlmaWindow notAlmaWindow = new NotAlmaWindow();
            notAlmaWindow.Show();
        }

        private void PenecereyiKapat(object sender, EventArgs e)
        {
            if (Onceki != null)
            {
                Onceki.Show();
            }
            else
            {
                Genel.UygulamaKapat();
            }
        }

        private void TestBitti(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DialogScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

    }
}
