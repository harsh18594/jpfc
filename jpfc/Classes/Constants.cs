﻿namespace jpfc.Classes
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
    }
}
