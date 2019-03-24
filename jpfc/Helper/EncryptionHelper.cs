using System.Security.Cryptography;
using System.Text;

namespace jpfc.Helper
{
    public static class EncryptionHelper
    {
        public static class SaltGenerator
        {
            private static RNGCryptoServiceProvider _cryptoServiceProvider = null;
            private const int SALT_SIZE = 24;
            static SaltGenerator()
            {
                _cryptoServiceProvider = new RNGCryptoServiceProvider();
            }

            public static string GetSaltString()
            {
                // TO STORE SALT BYTES
                byte[] saltBytes = new byte[SALT_SIZE];

                // GENERATE THE SALT
                _cryptoServiceProvider.GetNonZeroBytes(saltBytes);

                // CONVERT SALT TO STRING
                string saltString = Encoding.ASCII.GetString(saltBytes);
                return saltString;
            }
        }

        public static class HashGenerator
        {
            public static string GetPasswordHashAndSalt(string plainText)
            {
                // SHA256 algorithm to 
                // generate the hash from this salted password
                SHA256 sha = new SHA256CryptoServiceProvider();
                byte[] dataBytes = Encoding.ASCII.GetBytes(plainText);
                byte[] resultBytes = sha.ComputeHash(dataBytes);

                return Encoding.ASCII.GetString(resultBytes);
            }
        }

        public class EncryptionManager
        {
            public string GeneratePasswordHash(string plainTextString, out string salt)
            {
                salt = SaltGenerator.GetSaltString();

                string finalString = plainTextString + salt;

                return HashGenerator.GetPasswordHashAndSalt(finalString);
            }

            public bool IsStringMatch(string plainTextString, string salt, string hash)
            {
                string finalString = plainTextString + salt;
                return hash == HashGenerator.GetPasswordHashAndSalt(finalString);
            }
        }
    }
}
