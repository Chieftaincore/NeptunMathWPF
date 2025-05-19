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
        public enum CevapDurum
        {
            Bos,
            Dogru,
            Yanlis
        }

        public CevapDurum CevaplanmaDurumu { get; set; } = CevapDurum.Bos;

        public readonly string dogrusesDosyaYolu = @"Kaynaklar\Sesler\SesDogru5.wav";
        public readonly string yanlissesDosyaYolu = @"Kaynaklar\Sesler\SesYanlis6.wav";

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

        internal bool kilitli;
        
        public SeceneklerModel(ObservableCollection<string> _secenekler, string _dogrusecenek)
        {
            secenekler = _secenekler;
            dogrusecenek = _dogrusecenek;
        }

        public bool Cevapla()
        {
            kilitli = true;

            if (secilideger == dogrusecenek)
            {
                SesIslemleri.sesOynat(dogrusesDosyaYolu);

                CevaplanmaDurumu = CevapDurum.Dogru;

                return true;
            }
            else
            {
                SesIslemleri.sesOynat(yanlissesDosyaYolu);

                CevaplanmaDurumu = CevapDurum.Yanlis;

                return false;
            }
        }    

        public string DogruSecenekGetir()
        {
            return dogrusecenek;
        }
    }
}
