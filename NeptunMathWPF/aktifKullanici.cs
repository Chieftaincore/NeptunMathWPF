using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeptunMathWPF
{
    static class aktifKullanici
    {
        //KULLANICI ADI LOGİN PANEL YAPILDIĞINDA ORADAN ALINACAK!
        public static string kullaniciAdi { get; set; } = "DENEME1";
        public static int kullnId { get; set; } = Genel.dbEntities.USERS.Where(x => x.USERNAME == kullaniciAdi).Select(x => x.USERID).FirstOrDefault();
    }
}
