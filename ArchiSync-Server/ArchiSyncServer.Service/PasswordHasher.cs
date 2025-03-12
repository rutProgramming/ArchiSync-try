using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ArchiSyncServer.Service.Services
{
   

    public class PasswordHasher
    {
        private readonly byte[] fixedSalt = Convert.FromBase64String("YOUR_FIXED_SALT_BASE64_STRING");

        public string HashPassword(string password)
        {
            // Hashing של הסיסמה עם ה-Salt הקבוע
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: fixedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Hashing של הסיסמה הנכנסת עם ה-Salt הקבוע
            string hashedInputPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: fixedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // השוואת ההצפנות
            return hashedInputPassword == hashedPassword;
        }
    }

}
