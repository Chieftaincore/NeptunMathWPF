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
    // deneme yazısı
    //Ortak ve çok yönlu kullanımı olan fonksiyonlar bu sınıfa yazılacaktır
    //bu fonksiyonlara yerel loglama, try catch, hata dialogbox vs örnek gösterilebilir
    static class Genel
    {
        private static readonly string logFilePath = "app_log.txt"; // Hata loglaması için path

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

    }
}
