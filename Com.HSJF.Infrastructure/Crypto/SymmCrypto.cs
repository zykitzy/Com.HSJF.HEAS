using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Crypto
{
	/// <summary>
	/// 对称加密算法封装
	/// </summary>
	public class SymmCrypto : IDisposable
	{
		private AesCryptoServiceProvider crypto;
		private bool disposed;

		/// <summary>
		/// 加密密钥，16或32个字节，长度不相符会抛出ArgumentException。如不设置，会自动随机产生。
		/// </summary>
		public byte[] Key
		{
			get { return crypto.Key; }
			set { crypto.Key = value; }
		}

		/// <summary>
		/// 初始化向量，16个字节，长度不相符会抛出ArgumentException。如不设置，会自动随机产生。
		/// </summary>
		public byte[] IV
		{
			get { return crypto.IV; }
			set { crypto.IV = value; }
		}

		public SymmCrypto()
		{
			this.crypto = new AesCryptoServiceProvider();
			disposed = false;
		}

		public SymmCrypto(byte[] key, byte[] iv)
			: this()
		{
			this.Key = key;
			this.IV = iv;
		}

		/// <summary>
		/// </summary>
		/// <param name="key">8或16个字符的字符串，长度不符会抛出ArgumentException</param>
		/// <param name="iv">8个字符的字符串，长度不符会抛出ArgumentException</param>
		public SymmCrypto(string key, string iv)
			: this()
		{
			var e = Encoding.Unicode;
			this.Key = e.GetBytes(key);
			this.IV = e.GetBytes(key);
		}

		/// <summary>
		/// 加密
		/// </summary>
		/// <param name="data">数据明文</param>
		/// <returns>数据密文</returns>
		public byte[] Encrypt(byte[] plainText)
		{
			using(var ms = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(ms, crypto.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cs.Write(plainText, 0, plainText.Length);
				}
				return ms.ToArray();
			}
		}

		/// <summary>
		/// 解密
		/// </summary>
		/// <param name="cipherText">数据密文</param>
		/// <returns>数据明文</returns>
		public byte[] Decrypt(byte[] cipherText)
		{
			using (MemoryStream msCipher = new MemoryStream(cipherText))
			{
				using (CryptoStream cs = new CryptoStream(msCipher, crypto.CreateDecryptor(), CryptoStreamMode.Read))
				{
					var msPlain = new MemoryStream();
					cs.CopyTo(msPlain);
					return msPlain.ToArray();
				}
			}
		}

		/// <summary>
		/// 加密字符串
		/// </summary>
		/// <param name="plainText"></param>
		/// <returns></returns>
		public byte[] EncryptFromString(string plainText)
		{
			return this.Encrypt(Encoding.Default.GetBytes(plainText));
		}

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="plainText">原文</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>密文</returns>
        public byte[] EncryptFromString(string plainText, Encoding encoding)
	    {
	        return this.Encrypt(encoding.GetBytes(plainText));
	    }

		/// <summary>
		/// 解密为字符串
		/// </summary>
		/// <param name="cipherText"></param>
		/// <returns></returns>
		public string DecryptToString(byte[] cipherText)
		{
			return Encoding.Default.GetString(this.Decrypt(cipherText));
		}

        /// <summary>
        /// 解密为字符串
        /// </summary>
        /// <param name="cipherText">密文array</param>
        /// <param name="encoding">指定编码格式</param>
        /// <returns>解密后的字符串</returns>
	    public string DecryptToString(byte[] cipherText, Encoding encoding)
	    {
	        return encoding.GetString(this.Decrypt(cipherText));
	    }

		#region IDisposable实现
		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return; //已被回收过
			disposed = true;

			if (disposing)
			{
				//回收托管资源
				crypto.Dispose();
			}
			//回收非托管资源，没有
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region 静态快捷方法
		/// <summary>
		/// 加密，内部调用SymmCrypto.Encrypt
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] plainText, byte[] key, byte[] iv)
		{
			using (var sc = new SymmCrypto(key, iv))
			{
				return sc.Encrypt(plainText);
			}
		}
		
		/// <summary>
		/// 加密，内部调用SymmCrypto.EncryptFromString
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] EncryptFromString(string plainText, byte[] key, byte[] iv)
		{
			using (var sc = new SymmCrypto(key, iv))
			{
				return sc.EncryptFromString(plainText);
			}
		}

		/// <summary>
		/// 加密，内部调用SymmCrypto.EncryptFromString
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] EncryptFromString(string plainText, string key, string iv)
		{
			using (var sc = new SymmCrypto(key, iv))
			{
				return sc.EncryptFromString(plainText);
			}
		}

		/// <summary>
		/// 解密，内部调用SymmCrypto.Decrypt
		/// </summary>
		/// <param name="cipherText"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] cipherText, byte[] key, byte[] iv)
		{
			using (var sc = new SymmCrypto(key, iv))
			{
				return sc.Decrypt(cipherText);
			}
		}

		/// <summary>
		/// 解密，内部调用SymmCrypto.DecryptToString
		/// </summary>
		/// <param name="cipherText"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static string DecryptToString(byte[] cipherText, byte[] key, byte[] iv)
		{
			using (var sc = new SymmCrypto(key, iv))
			{
				return sc.DecryptToString(cipherText);
			}
		}

		/// <summary>
		/// 解密，内部调用SymmCrypto.DecryptToString
		/// </summary>
		/// <param name="cipherText"></param>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static string DecryptToString(byte[] cipherText, string key, string iv)
		{
			using (var sc = new SymmCrypto(key, iv))
			{
				return sc.DecryptToString(cipherText);
			}
		}
		#endregion
	}
}
