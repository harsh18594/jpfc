using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.AccountViewModels;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace jpfc.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IAccessCodeRepository _accessCodeRepository;

        public LoginService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginService> logger,
            IAccessCodeRepository accessCodeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accessCodeRepository = accessCodeRepository;
        }

        public async Task<(bool Success, string Error)> LogUserIn(LoginViewModel model)
        {
            bool success = false;
            string error = string.Empty;

            // verify access code first
            var accessCodeVerified = await VerifyAccessCodeAsync(model.AccessCode);
            if (true /*accessCodeVerified*/)
            {
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    success = true;
                }
                else if (result.IsLockedOut)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var lockoutEndTime = user.LockoutEnd;
                    var remainingTime = (lockoutEndTime.Value - DateTime.UtcNow).TotalMinutes;
                    error = $"Your account has been locked for {Math.Ceiling(remainingTime)} minutes due to invalid login attempts.";
                }
                else
                {
                    error = "Invalid login attempt.";
                }
            }
            else
            {
                error = "Invalid login attempt.";
            }


            return (success, error);
        }

        public async Task<bool> VerifyAccessCodeAsync(string enteredAccessCode)
        {
            bool success = false;
            try
            {
                var accessCode = await _accessCodeRepository.GetAccessCodeAsync();
                if (accessCode != null)
                {
                    var resultManager = new Helper.EncryptionHelper.EncryptionManager();
                    var hash = resultManager.GeneratePasswordHash(enteredAccessCode, out string salt);
                    success = resultManager.IsStringMatch(enteredAccessCode, accessCode.Salt, accessCode.Hash);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LoginService/VerifyAccessCodeAsync - exception:{@Ex}", new object[]{
                    ex
                });
            }


            return success;
        }

        public async Task<bool> SetAccessCodeAsync(AccessCodeViewModel model)
        {
            bool success = false;

            try
            {
                // make sure there is only one access code
                var accessCode = await _accessCodeRepository.GetAccessCodeAsync();
                if (accessCode == null)
                {
                    accessCode = new AccessCode()
                    {
                        CreateDate = DateTime.UtcNow
                    };
                }
                // encrypt code
                var encryptionManager = new Helper.EncryptionHelper.EncryptionManager();
                var hash = encryptionManager.GeneratePasswordHash(model.AccessCode, out string salt);
                // save to db                
                accessCode.Salt = salt;
                accessCode.Hash = hash;

                success = await _accessCodeRepository.SaveAccessCodeAsync(accessCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("LoginService/SetAccessCodeAsync - exception:{@Ex}", new object[]{
                    ex
                });
            }
            return success;
        }

        public async Task<(bool Success, string Error)> ResetAccessCodeAsync(Models.ManageViewModels.AccessCodeViewModel model)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                // make sure there is only one access code
                var accessCode = await _accessCodeRepository.GetAccessCodeAsync();
                if (accessCode != null)
                {
                    // verify current access code
                    var resultManager = new Helper.EncryptionHelper.EncryptionManager();
                    var hash = resultManager.GeneratePasswordHash(model.CurrentAccessCode, out string salt);
                    var currentAccessCodeMatched = resultManager.IsStringMatch(model.CurrentAccessCode, accessCode.Salt, accessCode.Hash);

                    if (currentAccessCodeMatched)
                    {
                        // encrypt code
                        var encryptionManager = new Helper.EncryptionHelper.EncryptionManager();
                        var hash1 = encryptionManager.GeneratePasswordHash(model.NewAccessCode, out string salt1);
                        // save to db                
                        accessCode.Salt = salt1;
                        accessCode.Hash = hash1;

                        success = await _accessCodeRepository.SaveAccessCodeAsync(accessCode);
                    }
                    else
                    {
                        error = "Error: Current access code is not valid";
                    }
                }
                else
                {
                    error = "Error: No access code found. If this error happens frequently, please contact IT support.";
                }
            }
            catch (Exception ex)
            {
                error = "Error: Unexpected error occurred while processing your request.";

                _logger.LogError("LoginService/ResetAccessCodeAsync - exception:{@Ex}", new object[]{
                    ex
                });
            }

            return (Success: success, Error: error);
        }
    }
}
