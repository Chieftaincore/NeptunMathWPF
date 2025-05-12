using HesapMakinesi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    public class HesapMakinesiModel
    {
        public Visibility Gorunur { get => pencere.Visibility; }
        
        public HesapMakinesiWindow pencere = new HesapMakinesiWindow()
        {
            Visibility = System.Windows.Visibility.Hidden
        };

        public void GosterGizle()
        {

            //Eventleri gizleme yapmama rağmen ka
            Genel.Handle(() =>
            {
                //Hesap makinesi daha önceden açık mı kontrol etmek için
                var acik = PresentationSource.FromVisual(pencere);

                if (acik == null)
                {
                    (pencere = new HesapMakinesiWindow()).Show();
                }
                else
                {
                    pencere.Visibility = Visibility.Visible;
                }
            });
        }
    }
}
