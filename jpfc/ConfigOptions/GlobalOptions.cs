using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.ConfigOptions
{
    public class GlobalOptions
    {
        public decimal LoanPercentForCalc { get; set; }
        public string ResetVerificationCode { get; set; }
        public string AccessCodeToReset { get; set; }
        public string PasswordToReset { get; set; }
        public string AdminUserId { get; set; }
    }
}
