using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;

namespace jpfc.Views.Services
{
    public static class ServicesNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Loan => "Pawn Precious Things";
        public static string LoanNavClass(ViewContext viewContext) => PageNavClass(viewContext, Loan);

        public static string BuySellGold => "Trade Precious Metal Bullion";
        public static string BuySellGoldNavClass(ViewContext viewContext) => PageNavClass(viewContext, BuySellGold);

        public static string Mortgage => "Mortgage";
        public static string MortgageNavClass(ViewContext viewContext) => PageNavClass(viewContext, Mortgage);

        public static string HAInsurance => "Home & Auto Insurance";
        public static string HAInsuranceNavClass(ViewContext viewContext) => PageNavClass(viewContext, HAInsurance);

        public static string LifeInsurance => "Life Insurance";
        public static string LifeInsuranceNavClass(ViewContext viewContext) => PageNavClass(viewContext, LifeInsurance);

        public static string PersonalLoan => "Personal Loan";
        public static string PersonalLoanNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalLoan);

        public static string CarLoan => "Car Loan";
        public static string CarLoanNavClass(ViewContext viewContext) => PageNavClass(viewContext, CarLoan);

        public static string CreditCards => "Credit Cards";
        public static string CreditCardsNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreditCards);

        public static string FinancePlanning => "Investment & Finance Planning";
        public static string FinancePlanningNavClass(ViewContext viewContext) => PageNavClass(viewContext, FinancePlanning);

        public static string Monetary => "Money Transfer/Currency Exchange";
        public static string MonetaryNavClass(ViewContext viewContext) => PageNavClass(viewContext, Monetary);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData[ActivePageKey] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
