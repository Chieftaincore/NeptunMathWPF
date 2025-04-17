using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    public class EtkilesimTemplateModel : DataTemplateSelector
    {
        public DataTemplate SoruModu { get; set; }
        public DataTemplate Proompter { get; set; }
        public DataTemplate DialogModu { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            string view = item as string;

            switch (view)
            {
                case "SoruModu":
                    return SoruModu;
                case "Proompter":
                    return Proompter;
                case "Dialog":
                    return DialogModu;
                default:
                    return base.SelectTemplate(item, container);
            };
        }
    }
}
