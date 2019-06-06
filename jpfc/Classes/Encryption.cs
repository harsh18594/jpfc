using jpfc.Models.EncryptionViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace jpfc.Classes
{
    public static class Encryption
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rijndaelmanaged?view=netcore-2.1

        private const int RandomStringLength = 15;
        private const string MyKey = "jPf*";
        private static readonly byte[] ByteIv = { 50, 89, 163, 1, 132, 74, 11, 115, 255, 46, 97, 78, 14, 211, 22, 10 };

        /// <summary>
        /// Encrypt sensitive data with Rijndael 
        /// </summary>
        /// <param name="oInput"></param>
        /// <returns></returns>
        public static EncryptionResultViewModel Encrypt(object oInput)
        {
            EncryptionResultViewModel result = new EncryptionResultViewModel();

            string cInput = null;
            if (oInput != null)
            {
                cInput = Convert.ToString(oInput);
                if (!string.IsNullOrEmpty(cInput))
                {
                    result = EncryptStringToBytes(cInput);
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt sensitive data with Rijndael 
        /// </summary>
        /// <param name="oInput"></param>
        /// <param name="uniqueKey"></param>
        /// <returns></returns>
        public static string Decrypt(object oInput, string uniqueKey)
        {
            string resultString = string.Empty;

            if (oInput != null)
            {
                byte[] cInput = ConvertB64StringToArray(oInput.ToString());
                if (cInput != null)
                {
                    resultString = DecryptStringFromBytes(cInput, uniqueKey);
                }
            }

            return resultString;
        }

        private static EncryptionResultViewModel EncryptStringToBytes(string plainText)
        {
            EncryptionResultViewModel result = new EncryptionResultViewModel();
            try
            {
                //   ********************************************************************
                //   ******   Encryption Key must be 256 bits long (32 bytes)      ******
                //   ******   If it is longer than 32 bytes it will be truncated.  ******
                //   ******   If it is shorter than 32 bytes it will be padded     ******
                //   ******   with upper-case Xs.                                  ****** 
                //   ********************************************************************
                var randomKey = RandomString();
                var key = MyKey + randomKey;
                var intLength = key.Length;
                if (intLength >= 32)
                {
                    key = Left(key, 32);
                }
                else
                {
                    var intRemaining = 32 - intLength;
                    key = key + new string(Convert.ToChar("X"), intRemaining);
                }
                var byteKey = Encoding.ASCII.GetBytes(key.ToCharArray());

                // Create an RijndaelManaged object
                // with the specified key and IV.
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(byteKey, ByteIv);

                    // create the streams used for encryption
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                // write all data to the stream
                                sw.Write((plainText));
                            }

                            result.EncryptedString = ConvertArrayToB64String(ms.ToArray());
                        }
                    }

                    result.UniqueKey = randomKey;
                    result.Success = true;
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return result;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, string uniqueKey)
        {
            string retVal = string.Empty;

            try
            {
                //   ********************************************************************
                //   ******   Encryption Key must be 256 bits long (32 bytes)      ******
                //   ******   If it is longer than 32 bytes it will be truncated.  ******
                //   ******   If it is shorter than 32 bytes it will be padded     ******
                //   ******   with upper-case Xs.                                  ****** 
                //   ********************************************************************
                var key = MyKey + uniqueKey;
                var intLength = key.Length;
                if (intLength >= 32)
                {
                    key = Left(key, 32);
                }
                else
                {
                    var intRemaining = 32 - intLength;
                    key = key + new string(Convert.ToChar("X"), intRemaining);
                }
                var byteKey = Encoding.ASCII.GetBytes(key.ToCharArray());

                // Create an RijndaelManaged object
                // with the specified key and IV.
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(byteKey, ByteIv);

                    // Create the streams used for decryption.
                    using (MemoryStream ms = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                // Read the decrypted bytes from the decrypting stream and place them in a string.
                                retVal = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return retVal;
        }

        private static string ConvertArrayToB64String(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        private static byte[] ConvertB64StringToArray(string input)
        {
            return Convert.FromBase64String(input);
        }

        private static string RandomString()
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, RandomStringLength).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = string.Empty;
            if (param.Length >= length)
            {
                result = param.Substring(0, length);
            }
            else
            {
                result = param;
            }

            //return the result of the operation
            return result;
        }
    }
}
