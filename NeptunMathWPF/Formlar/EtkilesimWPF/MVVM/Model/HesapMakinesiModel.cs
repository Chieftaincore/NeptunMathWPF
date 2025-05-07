using HesapMakinesi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    class HesapMakinesiModel
    {
        public Visibility Gorunur { get => pencere.Visibility; }
        public HesapMakinesiWindow pencere { get; set; }

        public HesapMakinesiModel()
        {
            pencere = new HesapMakinesiWindow()
            {
                Visibility = System.Windows.Visibility.Hidden
            };
        }

        public void GosterGizle()
        {
            if (Gorunur == Visibility.Hidden)
            {
                pencere.Visibility = Visibility.Visible;
            }
            else
            {
                if (Gorunur == Visibility.Visible)
                    pencere.Visibility = Visibility.Hidden;
            }
        }
    }
}
