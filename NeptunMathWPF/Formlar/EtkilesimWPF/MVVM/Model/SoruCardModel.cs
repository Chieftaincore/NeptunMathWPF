using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    //Model WPF sayfası için bir öğedir
    class SoruCardModel
    {
        public Soru secilisoru { get; set; }
        
        public string kaynak { get; set; }
        public DateTime zaman { get; set; }
        public bool Aktif { get; set; }
    }
}
