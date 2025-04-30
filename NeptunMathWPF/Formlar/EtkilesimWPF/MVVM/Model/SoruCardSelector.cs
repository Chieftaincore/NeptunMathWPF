using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    class SoruCardSelector : StyleSelector
    {
        public Style LaTeXStyle { get; set; }
        public Style MetinStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if(item is IStyleAnahtar anahtar) {

                string style = ((IStyleAnahtar)item).soruStyle;

                switch (style)
                {
                    case "SoruModuNormal":
                        return LaTeXStyle;
                    case "SoruModuMetin":
                        return MetinStyle;
                    default:
                        return LaTeXStyle;
                };
            }
            else
            {
                return base.SelectStyle(item, container);
            }
        }
    }
}
