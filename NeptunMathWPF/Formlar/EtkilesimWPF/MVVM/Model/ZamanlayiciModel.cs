using HonkSharp.Fluency;
using System;
using System.Windows;
using System.Windows.Threading;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    /// <summary>
    /// Zamanlayıcı eklemek için kullanılır.
    /// Constructorında zamanı ve zaman bitiminde olacak fonksiyonu yazınız
    /// </summary>
    class ZamanlayiciModel : ObservableObject
    {

        DispatcherTimer Zamanlayici;

        public event EventHandler ZamanBittiEvent;

        internal TimeSpan _Sure;


        private string _goruntu;
        public string goruntu { get => _goruntu;
            set {
                if (_goruntu != value) 
                { 
                    _goruntu = value; 
                    OnPropertyChanged(nameof(goruntu));
                    OnPropertyChanged();
                } 
            }
        }

        public ZamanlayiciModel(double dakika)
        {
            OnPropertyChanged();
            _Sure = TimeSpan.FromMinutes(dakika);
            
            Zamanlayici = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                goruntu = _Sure.ToString("c");

                if (_Sure == TimeSpan.Zero)
                {
                    ZamanBitti();
                    Zamanlayici.Stop();
                }

                _Sure = _Sure.Add(TimeSpan.FromSeconds(-1));

            }, Application.Current.Dispatcher);

           

        }

        public void ZamanBitti()
        {
            ZamanBittiEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
