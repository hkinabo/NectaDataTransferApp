using NectaDataTransfer.Shared.Models;
using System.Security.Cryptography;
using System.Text;

namespace NectaDataTransfer.Services
{
    internal class Setting
    {
        //  public static ConnectionModel SqlConnectionModel { get; set; }
        //Used on Huduma sifa from Emis 
        public static ConnectionModel SifaSqlConnectionModel { get; set; }
        //public static ConnectionModel MysqlConnectionModel { get; set; }
        //public static MenuModel UserMenuModel { get; set; }

        public static string menuKey = "";

        //public static SifaConnectionModel MysqlSifaConnectionModel { get; set; }

        private static readonly string key = "ashproghelpdotnetmania2023key123";
        public static string EncryptionMe(string textme)
        {
            byte[] iv = new byte[16];
            byte[] arrary;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryotor = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream ms = new();
                using CryptoStream cryptoStream = new(ms, encryotor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(textme);
                }
                arrary = ms.ToArray();

            }
            return Convert.ToBase64String(arrary);

        }
        public static string DecryptionMe(string textme)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(textme);
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryotor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new(buffer))
            {
                using CryptoStream cryptoStream = new(ms, decryotor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                return streamReader.ReadToEnd();
            };

        }
    }
}
