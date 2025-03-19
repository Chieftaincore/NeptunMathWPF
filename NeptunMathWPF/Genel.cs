using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NeptunMathWPF
{
    //Herhangi bir sınıfdan çekilebilecek terimler -Hüseyin
    public struct SoruTerimleri
    {
        public enum ifadeTurleri
        {
            sayi,
            kesir,
            degisken,
        }
    }

    // deneme yazısı
    //Ortak ve çok yönlu kullanımı olan fonksiyonlar bu sınıfa yazılacaktır
    //bu fonksiyonlara yerel loglama, try catch, hata dialogbox vs örnek gösterilebilir
    static class Genel
    {
        private static readonly string logFilePath = "app_log.txt"; // Hata loglaması için path
        static string YerelLogPath = "..\\Loglar\\";

        internal static void Handle(Action action) //Hata yönetimi için hazır try-catch blokları
        /***********************
        Kullanım: Handle(() => {
        *KODLAR*
        }); 
        ***********************/
        {
            try { action(); }
            catch (SqlException sqlEx)
            {
                LogToDatabase("ERROR", $"{sqlEx.Message}\n{sqlEx.InnerException}\n{sqlEx.StackTrace}");
                MessageBox.Show("Veritabanı bağlantısında bir sorun oluştu!", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            catch (FileNotFoundException fileEx)
            {
                LogToDatabase("ERROR", $"{fileEx.Message}\n{fileEx.InnerException}\n{fileEx.StackTrace}");
                MessageBox.Show("Gerekli dosya bulunamadı!", "Dosya Hatası", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (UnauthorizedAccessException authEx)
            {
                LogToDatabase("ERROR", $"{authEx.Message}\n{authEx.InnerException}\n{authEx.StackTrace}");
                MessageBox.Show("Bu işlem için yetkiniz yok!", "Yetki Hatası", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                LogToDatabase("ERROR", $"{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}");
                MessageBox.Show("Bir hata oluştu!", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        internal static void LogToDatabase(string level, string message)
        {
            // TAMAMLANACAK
        }

        //Yerel Hata Logu (Hüseyin E)
        //Catchin kendisini göstermenizde yeter
        //Kullanım 
        /***********
         * try(){
         * }catch(exception ex){
         * Genel.YerelMetinKaydet(ex);
         *  YA DA 
         * Genel.YerelMetinKaydet(ex, true, "Bağlantı kısmı SQL bulunamadı")
         * }
         ***********/
        internal static void YerelMetinKaydet(Exception err, bool goster = false, string ekyazi = "")
        {
            if (goster)
            {
                MessageBox.Show($"HATA :: Yerel dosyaya kaydediliyor {err.HResult}\nKayıt Konumu {YerelLogPath}");
            }

            string logDosyaPath = YerelLogPath + System.DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
            string Mesaj = $"[{System.DateTime.Today.ToString("dd-MMMM-yyyy")} :: {DateTime.Now.Hour}] HATA | {ekyazi} | HRESULT : {err.HResult} MESAJ :: {err.Message}";

            FileInfo logDosyaInfo = new FileInfo(logDosyaPath);
            DirectoryInfo logDirInfo = new DirectoryInfo(logDosyaInfo.DirectoryName);

            //Dosya yoksa yenisini oluşturur.
            if (!logDirInfo.Exists) logDirInfo.Create();

            //Dosya içine yazma. yeni mesaj olarak içine yazar eskisini silmez
            using (FileStream fileStream = new FileStream(logDosyaPath, FileMode.Append))
            {
                using (StreamWriter logYazici = new StreamWriter(fileStream))
                {
                    logYazici.WriteLine(Mesaj);
                }
            }
        }
    }
}
