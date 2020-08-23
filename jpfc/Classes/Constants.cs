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

        public static class Business
        {
            public const string TermsConditions = "1) The client hereby acknowledges receipt of loan/sell amount, copy of this contract and payment schedule. " +
                "2) We agree to return the described property(ies) to the client only upon presentation of this contract and payment of principal loan amount plus applicable interest and service charges. " +
                "3) Client hereby certifies that he or she is the legal owner of the property(ies) as described above, empowered to sell or dispose of the above property(ies) and is/are free and clear of all liens and encumbrances. " +
                "4) Client will be responsible for any legal fees incurred by J.P. Finance Group Ltd. resulting from this transaction. " +
                "5) All interest charges, service fee, storage fee are calculated per month and due 30 days from the date of this loan contract. " +
                "6) No credit shall be allowed for redemption in less than 30 days. " +
                "7) This loan is due in 30 days from the date of this contract and if we do not receive at least minimum interest amount, service fee, and storage fee as mentioned in this contract, you will forfeit your ownership of the above property(ies) and it will be sold at public or private sale by J.P. Finance Group Ltd. " +
                "8) Once property(ies) is/are sold to J.P. Finance Group Ltd. at agreed upon price, you will forfeit your rights of ownership immediately. " +
                "9) As per Pawn Broker Law, you allow us to share your information with legal authorities if requested by court order, police department or any other official government authorities. " +
                "10) Loan contract is non-transferable. " +
                "11) Only person named on the receipt can redeem the loan contract. " +
                "12) Lost loan contract may result in additional cost of duplicate loan contract and affidavit.";
        }

        public static class JobPost
        {
            public static class JobTypeId
            {
                public const string FtPermanent = "524da1c1-41d1-48a3-ae86-9ed0b5697083";
                public const string PtPermanent = "2c848d1c-5384-44f8-a0ce-1b4b32262440";
                public const string FtContract = "2e9526db-e626-421e-8baf-88da023cc13b";
                public const string PtContract = "e94a32b3-caf2-4824-8608-b13582618f5b";
                public const string FtTemporary = "6cfb1f29-ee23-41e0-89ed-082665a4bdfe";
                public const string PtTemporary = "92c30141-3d65-4131-805a-5d4d56dc1cf0";
            }
        }
    }
}
