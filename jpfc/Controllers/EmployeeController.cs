using jpfc.Models.EmployeeViewModels;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    [Authorize]
    public class EmployeeController : JpfcController
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List(EmployeeListViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", model);
            var result = await _employeeService.ListAllAsync(model);
            if (result.Success)
            {
                return View(result.Model);
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            _logger.LogInformation(GetLogDetails());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", model);
            if (ModelState.IsValid)
            {
                var result = await _employeeService.CreateAsync(model);
                if (!result.Success)
                {
                    SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                    return View(model);
                }

                SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Employee has been added successfully");
                return RedirectToAction(nameof(List));
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, "Please check all the information and submit again.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", id);
            var result = await _employeeService.FetchEmployeeForEditAsync(id);
            if (result.Success)
            {
                return View(result.Model);
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", model);
            if (ModelState.IsValid)
            {
                var result = await _employeeService.EditAsync(model);
                if (!result.Success)
                {
                    SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                    return View(model);
                }

                SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Employee has been updated successfully");
                return RedirectToAction(nameof(List));
            }

            SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, "Please check all the information and submit again.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation(GetLogDetails() + " - id:{@Id}", id);

            var result = await _employeeService.DeleteAsync(id);
            if (result.Success)
            {
                SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Employee has been removed successfully.");
            }
            else
            {
                SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
            }

            return RedirectToAction(nameof(List));

        }
    }
}
