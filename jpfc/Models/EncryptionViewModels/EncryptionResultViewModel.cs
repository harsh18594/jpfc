using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Models.EncryptionViewModels
{
    public class EncryptionResultViewModel
    {
        public string EncryptedString { get; set; }
        public string UniqueKey { get; set; }
        public bool Success { get; set; }
    }
}
