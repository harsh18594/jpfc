using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.ClientViewModels
{
    public class ClientIdentificationListViewModel
    {
        public int ClientIdentificationId { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public string IdentificationNumberEncryptedString { get; set; }
        public string IdentificationNumberUniqueKey { get; set; }
        public string DisplayValue
        {
            get
            {
                var idNumberLen = IdentificationNumber.Length;
                var last4Id = idNumberLen > 4 ? $"** {IdentificationNumber.Substring(idNumberLen - 4)}" : IdentificationNumber;
                return $"{IdentificationType} - {last4Id}";
            }
        }
    }
}
