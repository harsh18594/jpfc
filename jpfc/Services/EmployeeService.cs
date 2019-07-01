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

        public EmployeeService(ILogger<EmployeeService> logger,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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
    }
}
