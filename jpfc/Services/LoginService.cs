using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jpfc.Classes;
using jpfc.ConfigOptions;
using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.AccountViewModels;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace jpfc.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IAccessCodeRepository _accessCodeRepository;
        private readonly GlobalOptions _globalOptions;

        public LoginService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginService> logger,
            IAccessCodeRepository accessCodeRepository,
            IOptions<GlobalOptions> globalOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accessCodeRepository = accessCodeRepository;
            _globalOptions = globalOptions.Value;
        }

        public async Task<(bool Success, string Error)> LogUserIn(LoginViewModel model)
        {
            bool success = false;
            string error = string.Empty;

            // verify access code first
            var accessCodeVerified = await VerifyAccessCodeAsync(model.AccessCode);
            if (accessCodeVerified)
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
                    var savedValue = Encryption.Decrypt(accessCode.EncryptedValue, accessCode.UniqueKey);
                    if (string.Equals(savedValue, enteredAccessCode))
                    {
                        success = true;
                    }
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
                else
                {
                    accessCode.AuditUtc = DateTime.UtcNow;
                }

                var encryptionResult = Encryption.Encrypt(model.AccessCode);
                accessCode.EncryptedValue = encryptionResult.EncryptedString;
                accessCode.UniqueKey = encryptionResult.UniqueKey;

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

        public async Task<(bool Success, string Error)> ResetAccessCodeAsync(Models.ManageViewModels.AccessCodeViewModel model, string userId)
        {
            bool success = false;
            string error = string.Empty;

            try
            {
                // make sure there is only one access code
                var accessCode = await _accessCodeRepository.GetAccessCodeAsync();
                if (accessCode != null)
                {
                    var savedValue = Encryption.Decrypt(accessCode.EncryptedValue, accessCode.UniqueKey);

                    if (string.Equals(savedValue, model.CurrentAccessCode))
                    {
                        // encrypt code
                        //var encryptionManager = new Helper.EncryptionHelper.EncryptionManager();
                        //var hash1 = encryptionManager.GeneratePasswordHash(model.NewAccessCode, out string salt1);
                        //// save to db                
                        //accessCode.Salt = salt1;
                        //accessCode.Hash = hash1;

                        var encryptionResult = Encryption.Encrypt(model.NewAccessCode);
                        accessCode.EncryptedValue = encryptionResult.EncryptedString;
                        accessCode.UniqueKey = encryptionResult.UniqueKey;
                        accessCode.AuditUtc = DateTime.UtcNow;

                        success = await _accessCodeRepository.SaveAccessCodeAsync(accessCode);
                    }
                    else
                    {
                        error = "Current access code is not valid";
                    }
                }
                else
                {
                    error = "No access code found. If this error happens frequently, please contact IT support.";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";

                _logger.LogError("LoginService.ResetAccessCodeAsync - exception:{@Ex}", new object[] { ex });
            }

            return (Success: success, Error: error);
        }

        public async Task<(bool Success, string Error)> ChangePasswordAsync(ChangePasswordViewModel model, string userId)
        {
            var success = false;
            var error = "";

            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        if (changePasswordResult.Succeeded)
                        {
                            await _signInManager.RefreshSignInAsync(user);
                            success = true;
                        }
                        else
                        {
                            error = string.Join("; ", changePasswordResult.Errors.Select(e => e.Description).ToList());
                        }
                    }
                    else
                    {
                        error = "Unable to load user";
                    }
                }
                else
                {
                    error = "User Id is invalid";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("LoginService.ChangePasswordAsync - exception:{@Ex}", new object[] { ex });
            }

            return (Success: success, Error: error);
        }

        public async Task<bool> ResetAccount(string verificationCode)
        {
            var success = false;
            try
            {
                if (string.Equals(verificationCode, _globalOptions.ResetVerificationCode))
                {
                    await SetAccessCodeAsync(new AccessCodeViewModel { AccessCode = _globalOptions.AccessCodeToReset });
                    var adminUser = await _userManager.FindByIdAsync(_globalOptions.AdminUserId);
                    if (adminUser != null)
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(adminUser);
                        await _userManager.ResetPasswordAsync(adminUser, token, _globalOptions.PasswordToReset);
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("LoginService.ResetAccount - exception:{@Ex}", new object[] { ex });
            }
            return success;
        }
    }
}
