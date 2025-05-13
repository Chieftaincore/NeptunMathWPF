using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF.SoruVeAjani.Algorithma
{
    public class ZorlukRepository
    {
        
        public static Dictionary<string, int[]> Araliklar = new Dictionary<string, int[]>
        {
            {"TAMSAYINORMAL", new int[] {2,50} }, {"TAMSAYIBOLME", new int[] {2,5} }, {"TAMSAYIYANILMA", new int[] {-30,30} },
            {"FAKTORIYELNORMAL",new int[]{2,6}}, {"KESIRTAMCARPAN",new int[] {2,15} }, {"FAKTORIYELSAYI", new int[] { 2, 6 } },
            {"USLUNORMAL",new int[]{1,11}}, {"USTAMCARPAN",new int[] {0,4} }, {"USBOLMECARPAN", new int[]{2,5}}
        };

        public List<ZorlukModel> Zorluklar { get; set; }

        public ZorlukRepository()
        {

        }
    }
}
