namespace jpfc.Classes
{
    public class Constants
    {
        public static class Toastr
        {
            public const string Success = "Toastr.Success";
            public const string Error = "Toastr.Error";
            public const string Warning = "Toastr.Warning";
            public const string Information = "Toastr.Information";

            public class Ajax
            {
                public const string Success = "Toastr.Ajax.Success";
                public const string Warning = "Toastr.Ajax.Warning";
                public const string Error = "Toastr.Ajax.Error";
                public const string Information = "Toastr.Ajax.Information";
            }
        }

        public static class ViewComponentKeys
        {
            public const string SiteAjaxMessageView = "SiteAjaxMessageView";
            public const string SiteMessageView = "SiteMessageView";
        }

        public static class TransactionAction
        {
            public const string Sell = "Sell";
            public const string Purchase = "Purchase";
            public const string Loan = "Loan";
        }

        public static class OzToGm
        {
            public const decimal _1Oz = (decimal)31.103;
        }

        public static class KaratId
        {
            public const string _9K = "F47E14CB-9FC9-4AAF-9F2D-A64FC6A15C2F";
            public const string _10K = "0BAB0D22-F831-4B6C-B177-C623BA4BF5B9";
            public const string _14K = "775D10C4-A955-4039-AAF6-16A80B0759F7";
            public const string _18K = "4AAF69A9-7BB0-4F70-956D-4F55CD98FE1E";
            public const string _22K = "4DA2D061-E089-4C8D-BFA4-534A301E0C87";
            public const string _24K = "D9FB756F-933D-4CFA-9DC3-76714B84B256";
            public const string _925 = "EF76A7FE-0D8D-4814-83DF-47A4A035D703";
            public const string _950 = "06FF5E11-00CB-4CAB-AE61-8237A13AE60F";
        }

        public static class System
        {
            public const string TimeZone = "Eastern Standard Time";
        }

        public static class PaymentMethod
        {
            public const string Cash = "Cash";
            public const string eTransfer = "E-Transfer";
        }
    }
}
