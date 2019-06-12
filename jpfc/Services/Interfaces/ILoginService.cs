using jpfc.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface ILoginService
    {
        Task<(bool Success, string Error)> LogUserIn(LoginViewModel model);
        Task<bool> VerifyAccessCodeAsync(string accessCode);
        Task<bool> SetAccessCodeAsync(AccessCodeViewModel model);
        Task<(bool Success, string Error)> ResetAccessCodeAsync(Models.ManageViewModels.AccessCodeViewModel model, string userId);
        Task<(bool Success, string Error)> ChangePasswordAsync(ChangePasswordViewModel model, string userId);
    }
}
