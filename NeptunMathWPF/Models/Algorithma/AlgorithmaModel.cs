using NeptunMathWPF.Formlar.EtkilesimWPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.SoruVeAjani.Algorithma
{

    /// <summary>
    /// etkileşim sayfası için algorithma nesnesi
    /// kullanıcıya göre veya özel algorithma oluşturulması
    /// 
    /// </summary>
    class AlgorithmaModel
    {
        public ZorlukRepository repo { get; set; }


        public Func<Soru> SonrakiAlgorithma { get; set; }

        internal EtkilesimMVM MVVM;

        public AlgorithmaModel(EtkilesimMVM _MVVM)
        {
            MVVM = _MVVM;
        }



        
    }
}
