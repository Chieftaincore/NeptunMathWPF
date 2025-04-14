using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    //Model WPF sayfası için bir öğedir
    class SoruCardModel
    {
        public Soru soru { get; set; }
       
        public List<string> secenekler { get; set; }

        public string LaTeX { get; set; }
        public string kaynak { get; set; }
        public DateTime zaman { get; set; }
        public bool Aktif { get; set; }

        public SoruCardModel(Soru s)
        {
            soru = s;
            LaTeX = soru.GetLaTex();

            Genel.Handle(() =>
            {
                Random random = new Random();
                int inject = random.Next(0, soru.GetDigerSecenekler().Length);

                List<String> Liste = new List<string>();
                for (int i=0; i<soru.GetDigerSecenekler().Length; i++)
                {
                   if(i == inject)
                       Liste.Add(soru.GetSonucSecenek());

                    Liste.Add(soru.GetDigerSecenekler()[i]);
                }

                secenekler = Liste;
            });
        }
    }
}
