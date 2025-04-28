using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    class SeceneklerModel : ObservableObject
    {
        public ObservableCollection<string> secenekler { get; set; }

        string _secilideger;
        public string secilideger { get => _secilideger; set { 

                if(_secilideger != value)
                {
                    _secilideger = value;

                    OnPropertyChanged(nameof(secilideger));
                }
            } 
        }

        string dogrusecenek { get; set; }

        
        public SeceneklerModel(ObservableCollection<string> _secenekler, string _dogrusecenek)
        {
            secenekler = _secenekler;
            dogrusecenek = _dogrusecenek;
        }

        public bool Cevapla()
        {
            MessageBox.Show($"seçilen {secilideger} | dogru cevap{dogrusecenek}");

            if (secilideger == dogrusecenek)
            {
            
                return true;
            }
            else
            {
                return false;
            }
        }    

        public string DogruSecenekGetir()
        {
            return dogrusecenek;
        }
    }
}
