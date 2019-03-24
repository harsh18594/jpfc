using jpfc.Classes;
using jpfc.Models.ComponentViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace jpfc.Component
{
    [ViewComponent(Name = Constants.ViewComponentKeys.SiteMessageView)]
    public class SiteViewComponent : ViewComponent
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public SiteViewComponent(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SiteMessageViewModel();
            var tempData = _tempDataFactory.GetTempData(HttpContext);

            if (tempData[Constants.Toastr.Error] != null && !string.IsNullOrEmpty(tempData[Constants.Toastr.Error].ToString()))
            {
                model.ErrorMessage = tempData[Constants.Toastr.Error].ToString();
            }
            if (tempData[Constants.Toastr.Success] != null && !string.IsNullOrEmpty(tempData[Constants.Toastr.Success].ToString()))
            {
                model.SuccessMessage = tempData[Constants.Toastr.Success].ToString();
            }
            if (tempData[Constants.Toastr.Information] != null && !string.IsNullOrEmpty(tempData[Constants.Toastr.Information].ToString()))
            {
                model.InformationMessage = tempData[Constants.Toastr.Information].ToString();
            }
            if (tempData[Constants.Toastr.Warning] != null && !string.IsNullOrEmpty(tempData[Constants.Toastr.Warning].ToString()))
            {
                model.WarningMessage = tempData[Constants.Toastr.Warning].ToString();
            }

            return View(model);
        }
    }
}
