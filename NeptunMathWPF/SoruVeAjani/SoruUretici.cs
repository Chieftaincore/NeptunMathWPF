using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;

namespace NeptunMathWPF
{
    //Soru olusturucu sınıf statik fonksiyonlar içinde yer alacaktır
    public static class SoruUretici
    {
        //Baslangıc için testler detaylandırılacak ve nesnelere bağlanacak
        public static void IslemSorusuOlustur(Soru soru, int seceneksayisi) {
            //Randomluk baslat
            Random rng = new Random();
            List<int> intler = new List<int>();

            string metin = "";
            //seviyeye göre karmasiklik yap
            switch (soru.GetSeviye())
            {
                case 0:
                    //Şimdilik datatable'la deniyorum sonra değiştirilecektir
                    DataTable dt = new DataTable();

                for(int y=0; y < 3;y++)
                {
                    int sayi = rng.Next(1,50);
                    metin += sayi.ToString();

                    if (y == 0) 
                        metin += KarakterDondur(new[] {'+','-','*'});

                    if (y == 1)
                        metin += KarakterDondur(new[] { '+', '-'});
                }
                    MessageBox.Show(metin);
                    var son = dt.Compute(metin,"");
                    for(int i = 0; i < seceneksayisi - 1; i++)
                    {
                    int fark;
                    int sonuc = Int32.Parse(son.ToString());
                        if (sonuc < 100)
                        {
                        fark = rng.Next(1,20);
                        }
                        else
                        {
                        fark = rng.Next(25, 36);
                        }

                        int degisim;
                        if(KarakterDondur(new[] { '+', '-' }) == '-')
                        {
                        degisim = sonuc - fark;
                        }
                        else
                        {
                        degisim = sonuc + fark;
                        }
                    intler.Add(degisim);
                    }
                    string sayilar = "";
                    foreach (int item in intler)
                    {
                        sayilar += (item.ToString() + " ");
                    }
                MessageBox.Show($"Random İşlem Sorusu :  {metin} \nÇıkan Sonuç : {son} \n Diger Secenekler :: {sayilar}");
                    break;
            }
        }
        public static char KarakterDondur(char[] charlar)
        {
            //rastgele karakter döndür
            Random rng = new Random();

            return charlar[rng.Next(0,charlar.Length)];
        }
    }
}
