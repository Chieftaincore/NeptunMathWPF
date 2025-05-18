using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{

    /// <summary>
    /// Sayfada Alt Kısımda neler geleceğini eklemek içib
    /// </summary>
    public class EtkilesimTemplateModel : DataTemplateSelector
    {
        //Şıklar LaTeX ve Kısa ise:
        public DataTemplate SoruModuLaTeX { get; set; }

        //Şıklar Uzun veya LaTeX değilse;
        public DataTemplate SoruModuMetin { get; set; }

        //Şıklar kilitlenmesi için
        public DataTemplate SoruModuKilitli { get; set; }

        public DataTemplate Proompter { get; set; }
        public DataTemplate DialogModu { get; set; }
        public DataTemplate TestBitti { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            string view = item as string;

            switch (view)
            {
                case "SoruModuNormal":
                    return SoruModuLaTeX;
                case "SoruModuMetin":
                    return SoruModuMetin;
                case "Proompter":
                    return Proompter;
                case "SoruModuKilitli":
                    return SoruModuKilitli;
                case "TestBitti":
                    return TestBitti;
                case "Dialog":
                    return DialogModu;
                default:
                    return SoruModuMetin;
            };
        }
    }
}
