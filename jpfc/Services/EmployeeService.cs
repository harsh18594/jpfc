using jpfc.Models;
using jpfc.Models.EmployeeViewModels;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public EmployeeService(ILogger<EmployeeService> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool Success, string Error, EmployeeListViewModel Model)> ListAllAsync(EmployeeListViewModel iModel)
        {
            var success = false;
            string error = "";
            EmployeeListViewModel model = new EmployeeListViewModel();

            try
            {
                var users = _userManager.Users
                    ?.Where(u => (string.IsNullOrEmpty(iModel.FirstName) || u.FirstName == iModel.FirstName)
                    && (string.IsNullOrEmpty(iModel.LastName) || u.LastName == iModel.LastName))
                    ?.ToList();

                if (users?.Any() == true)
                {
                    foreach (var user in users)
                    {
                        model.Employees.Add(new EmployeeViewModel
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Id = user.Id
                        });
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("EmployeeService.ListAllAsync - exception:{@Ex}", ex);
            }

            return (success, error, model);
        }

        public async Task<(bool Success, string Error)> CreateAsync(CreateEmployeeViewModel model)
        {
            var success = false;
            var error = "";

            try
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.EmailAddress,
                    UserName = model.EmailAddress
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // todo: save role

                    success = true;
                }
                else if (result.Errors?.Any() == true)
                {
                    foreach (var err in result.Errors)
                    {
                        error += $"{err.Description}; ";
                    }
                }
                else
                {
                    error = "Unexpected error occurred while processing your request";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("EmployeeService.CreateAsync - exception:{@Ex}", ex);
            }

            return (success, error);
        }

        public async Task<(bool Success, string Error)> DeleteAsync(string id)
        {
            var success = false;
            var error = "";

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        success = true;
                    }
                    else if (result.Errors?.Any() == true)
                    {
                        foreach (var err in result.Errors)
                        {
                            error += $"{err.Description}; ";
                        }
                    }
                    else
                    {
                        error = "Unexpected error occurred while processing your request";
                    }
                }
                else
                {
                    error = "Unable to locate information";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("EmployeeService.DeleteAsync - exception:{@Ex}", ex);
            }

            return (success, error);
        }

        public async Task<(bool Success, string Error, EditEmployeeViewModel Model)> FetchEmployeeForEditAsync(string id)
        {
            var success = false;
            var error = "";
            var model = new EditEmployeeViewModel();

            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user != null)
                    {
                        model.FirstName = user.FirstName;
                        model.LastName = user.LastName;
                        model.EmailAddress = user.Email;
                        model.LockoutEnd = user.LockoutEnd;

                        success = true;
                    }
                    else
                    {
                        error = "Unable to locate employee information";
                    }
                }
                else
                {
                    error = "Invalid Id";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("EmployeeService.FetchEmployeeForEditAsync - exception:{@Ex}", ex);
            }

            return (success, error, model);
        }

        public async Task<(bool Success, string Error)> EditAsync(EditEmployeeViewModel model)
        {
            var success = false;
            var error = "";

            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    // update values
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.EmailAddress;
                    user.UserName = model.EmailAddress;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        success = true;

                        // update password if required
                        if (!string.IsNullOrEmpty(model.Password))
                        {
                            // reset password
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var changePasswordResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                            if (changePasswordResult.Succeeded)
                            {
                                await _signInManager.RefreshSignInAsync(user);
                                success = true;
                            }
                            else
                            {
                                success = false;
                                error = string.Join("; ", changePasswordResult.Errors.Select(e => e.Description).ToList());
                            }
                        }
                    }
                    else if (result.Errors?.Any() == true)
                    {
                        foreach (var err in result.Errors)
                        {
                            error += $"{err.Description}; ";
                        }
                    }
                    else
                    {
                        error = "Unexpected error occurred while processing your request";
                    }
                }
                else
                {
                    error = "Unable to locate employee information";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request";
                _logger.LogError("EmployeeService.EditAsync - exception:{@Ex}", ex);
            }

            return (success, error);
        }
    }
}