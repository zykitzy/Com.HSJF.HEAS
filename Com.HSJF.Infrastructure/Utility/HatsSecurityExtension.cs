using System;
using System.Text;
using Com.HSJF.Infrastructure.Crypto;
using Newtonsoft.Json;

namespace Com.HSJF.Infrastructure.Utility
{
    public static class HatsSecurityExtension
    {
        private const string Key = "HSJF!@#$12345678";

        private const string Iv = "HSJF^%$#12345678";

        public static string ToHatsString<Src>(this Src srcVal)
        {

            if (srcVal == null)
            {
                throw new ArgumentNullException("srcVal", "参数不能为空");
            }

            string serializedValue = JsonConvert.SerializeObject(srcVal);
            byte[] valBytes = Encoding.UTF8.GetBytes(serializedValue);
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(Iv);
            byte[] encrypted = new SymmCrypto(keyBytes, ivBytes).Encrypt(valBytes);

            return Convert.ToBase64String(encrypted);
        }

        public static string ToHatsString(this string srcVal)
        {

            if (srcVal == null)
            {
                throw new ArgumentNullException("srcVal", "参数不能为空");
            }

            byte[] valBytes = Encoding.UTF8.GetBytes(srcVal);
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(Iv);
            byte[] encrypted = new SymmCrypto(keyBytes, ivBytes).Encrypt(valBytes);

            return Convert.ToBase64String(encrypted);
        }

        public static Des FromHatsString<Des>(this string encryptValue) where Des : new()
        {
            if (string.IsNullOrEmpty(encryptValue) || string.IsNullOrWhiteSpace(encryptValue))
            {
                throw new ArgumentNullException("encryptValue", "参数不能为空");
            }

            byte[] valBytes = Convert.FromBase64String(encryptValue);
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(Iv);
            string decryptedBytes = new SymmCrypto(keyBytes, ivBytes).DecryptToString(valBytes, Encoding.UTF8);

            return JsonConvert.DeserializeObject<Des>(decryptedBytes);
        }

        public static string FromHatsString(this string encryptValue)
        {
            if (string.IsNullOrEmpty(encryptValue) || string.IsNullOrWhiteSpace(encryptValue))
            {
                throw new ArgumentNullException("encryptValue", "参数不能为空");
            }

            byte[] valBytes = Convert.FromBase64String(encryptValue);
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(Iv);
            string decryptedStr = new SymmCrypto(keyBytes, ivBytes).DecryptToString(valBytes, Encoding.UTF8);

            return decryptedStr;
        }
    }
}