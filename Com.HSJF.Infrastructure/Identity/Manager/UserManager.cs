using Com.HSJF.Infrastructure.Crypto;
using Com.HSJF.Infrastructure.Identity.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Com.HSJF.Infrastructure.Identity.Store;

namespace Com.HSJF.Infrastructure.Identity.Manager
{
    public class UserManager : Microsoft.AspNet.Identity.UserManager<User, string>
    {
        UserStore UserStore;
        public UserManager(UserStore store) : base(store)
        {
            this.PasswordHasher = new PassWordHashExtend();
            UserStore = store;
        }

        /// <summary>
        /// 
        /// </summary>
        public Task<IdentityResult> FlashRoles(string userid,params string[] roleid)
        {
            return Task<IdentityResult>.Run(() =>
            {
                return UserStore.FlashRolesAsync(userid, roleid);
            });
        }
    }


    public class PassWordHashExtend : Microsoft.AspNet.Identity.PasswordHasher
    {
        private byte[] _Key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Cryptokey"] ?? "HSJF!@#$12345678");
        private byte[] _IV = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["CryptoIV"] ?? "HSJF^%$#12345678");
        public override string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("Password is Null");
            }
            SymmCrypto symm = new SymmCrypto(_Key, _IV);
            return Convert.ToBase64String(symm.EncryptFromString(password));
        }

        public byte[] HashPass(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("Password is Null");
            }
            SymmCrypto symm = new SymmCrypto(_Key, _IV);
            return symm.EncryptFromString(password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
            {
                return PasswordVerificationResult.Failed;
            }
            var P1 = Convert.FromBase64String(hashedPassword);
            if (P1.Length < 1)
            {
                return PasswordVerificationResult.Failed;
            }
            var P2 = HashPass(providedPassword);
            if (P2.Length < 1)
            {
                return PasswordVerificationResult.Failed;
            }
            if (ByteEquals(P1, P2))
                return PasswordVerificationResult.Success;
            return PasswordVerificationResult.Failed;
        }



        public bool ByteEquals(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null || b1.Length != b2.Length)
            {
                return false;
            }
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                {
                    return false;
                }
            }
            return true;
        }


    }

}
