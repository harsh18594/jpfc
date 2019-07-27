using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Classes
{
    public static class Helper
    {
        public static string FormatPhoneNumber(object phoneNumber)
        {
            var retVal = "";
            try
            {
                if (phoneNumber != null)
                {
                    var strPhoneNumber = phoneNumber.ToString();
                    if (strPhoneNumber?.Length == 10)
                    {
                        retVal = $"({strPhoneNumber.Substring(0, 3)}) {strPhoneNumber.Substring(3, 3)}-{strPhoneNumber.Substring(6)}";
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return retVal;
        }

        public static string StripNonNumericCharacters(string input)
        {
            var retVal = "";

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    retVal = new string(input.Where(c => char.IsDigit(c)).ToArray());
                }
            }
            catch (Exception)
            {
                retVal = input;
            }

            return retVal;
        }
    }
}
