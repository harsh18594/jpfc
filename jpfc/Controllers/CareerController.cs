using jpfc.Models;
using jpfc.Models.JobPostViewModel;
using jpfc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Controllers
{
    [Authorize]
    public class CareerController : JpfcController
    {
        private readonly ILogger<CareerController> _logger;
        private readonly IJobPostService _jobPostService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CareerController(ILogger<CareerController> logger,
            IJobPostService jobPostService,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _jobPostService = jobPostService;
            _userManager = userManager;
        }

        public IActionResult List()
        {
            _logger.LogInformation(GetLogDetails());

            return View();
        }

        public async Task<IActionResult> GetListData(bool activeOnly = false)
        {
            _logger.LogInformation(GetLogDetails() + " - activeOnly:{ActiveOnly}", activeOnly);

            var result = await _jobPostService.ListForAdminAsync(activeOnly);
            return Json(new
            {
                success = result.Success,
                error = result.Error,
                data = result.Model
            });
        }

        public async Task<IActionResult> Create()
        {
            _logger.LogInformation(GetLogDetails());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobPostViewModel model)
        {
            _logger.LogInformation(GetLogDetails() + " - model:{@Model}", model);

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var result = await _jobPostService.SaveJobPostAsync(model, userId);
                if (result.Success)
                {
                    SetSiteMessage(MessageType.Success, DisplayFor.FullRequest, "Job post has been saved successfully");
                    return RedirectToAction("List");
                }
                else
                {
                    SetSiteMessage(MessageType.Error, DisplayFor.FullRequest, result.Error);
                }
            }

            return View(model);
        }
    }
}
