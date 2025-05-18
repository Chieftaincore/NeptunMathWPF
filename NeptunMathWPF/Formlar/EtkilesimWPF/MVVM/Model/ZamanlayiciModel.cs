using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NeptunMathWPF.Formlar.EtkilesimWPF.MVVM.Model
{
    /// <summary>
    /// Zamanlayıcı eklemek için kullanılır.
    /// Constructorında zamanı ve zaman bitiminde olacak fonksiyonu yazınız
    /// </summary>
    class ZamanlayiciModel
    {
        System.Timers.Timer timer;

        public ZamanlayiciModel( double sure, Task timeout)
        {
            timer = new Timer(sure);
            timer.Start();

        }

    }
}
