using jpfc.Classes;
using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.JobPostViewModel;
using jpfc.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class JobPostService : IJobPostService
    {
        private readonly IJobRepository _jobRepository;
        public readonly ILogger<JobPostService> _logger;
        private readonly IDateTimeService _dateTimeService;

        public JobPostService(IJobRepository jobRepository,
            ILogger<JobPostService> logger,
            IDateTimeService dateTimeService)
        {
            _jobRepository = jobRepository;
            _logger = logger;
            _dateTimeService = dateTimeService;
        }

        public async Task<(bool Success, string Error, ICollection<JobPostListViewModel> Model)> ListForAdminAsync(bool activeOnly = false)
        {
            var success = false;
            var error = string.Empty;
            var model = new List<JobPostListViewModel>();

            try
            {
                model = await _jobRepository.ListJobPostAsync(activeOnly) as List<JobPostListViewModel>;
                success = true;
            }
            catch (Exception ex)
            {
                error = "An unexpected error has occurred.";
                _logger.LogError("JobPostService.ListForAdminAsync - exception:{@Ex}", ex);
            }

            return (success, error, model);
        }

        public async Task<(bool Success, string Error)> SaveJobPostAsync(CreateJobPostViewModel model, string userId)
        {
            var success = false;
            var error = string.Empty;

            try
            {
                JobPost jobPost = null;
                if (model.JobPostId > 0)
                {
                    jobPost = await _jobRepository.FetchByIdAsync(model.JobPostId.Value);
                }
                if (jobPost == null)
                {
                    jobPost = new JobPost
                    {
                        CreatedUserId = userId,
                        CreatedUtc = DateTime.UtcNow
                    };
                }
                else
                {
                    jobPost.AuditUserId = userId;
                    jobPost.AuditUtc = DateTime.UtcNow;
                }

                // save other values
                jobPost.JobTitle = model.JobTitle;
                jobPost.Description = model.Description;
                jobPost.Requirements = model.Requirements;
                jobPost.JobTypeId = model.JobTypeId;
                jobPost.JobStartDate = model.JobStartDate;
                jobPost.Length = model.Length;
                jobPost.Pay = model.Pay;
                jobPost.JobLocation = model.JobLocation;
                if (model.JobCloseDate.HasValue)
                {
                    jobPost.JobCloseUtc = _dateTimeService.ConvertDateTimeToUtc(model.JobCloseDate.Value);
                }
                jobPost.IsDraft = model.IsDraft;
                jobPost.IsClosed = model.IsClosed;

                await _jobRepository.SaveAsync(jobPost);
                success = true;
            }
            catch (Exception ex)
            {
                error = "An unexpected error has occurred.";
                _logger.LogError("JobPostService.SaveJobPostAsync - exception:{@Ex}", ex);
            }

            return (success, error);
        }

        public async Task<(bool Success, string Error, CreateJobPostViewModel Model)> FetchJobPostForEditAsync(int id)
        {
            var success = false;
            var error = string.Empty;
            var model = new CreateJobPostViewModel();

            try
            {
                var jobPost = await _jobRepository.FetchByIdAsync(model.JobPostId.Value);
                if (jobPost != null)
                {
                    model.JobTitle = jobPost.JobTitle;
                    model.Description = jobPost.Description;
                    model.Requirements = jobPost.Requirements;
                    model.JobTypeId = jobPost.JobTypeId;
                    model.JobStartDate = jobPost.JobStartDate;
                    model.Length = jobPost.Length;
                    model.Pay = jobPost.Pay;
                    model.JobLocation = jobPost.JobLocation;
                    if (jobPost.JobCloseUtc.HasValue)
                    {
                        model.JobCloseDate = _dateTimeService.ConvertUtcToDateTime(jobPost.JobCloseUtc.Value,
                            _dateTimeService.FetchTimeZoneInfo(Constants.System.TimeZone));
                    }
                    model.IsDraft = jobPost.IsDraft;
                    model.IsClosed = jobPost.IsClosed;

                    success = true;
                }
                else
                {
                    error = "Unable to locate job post";
                }
            }
            catch (Exception ex)
            {
                error = "An unexpected error has occurred.";
                _logger.LogError("JobPostService.FetchJobPostForEditAsync - exception:{@Ex}", ex);
            }

            return (success, error, model);
        }
    }
}
