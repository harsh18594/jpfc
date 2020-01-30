using jpfc.Models.JobPostViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services.Interfaces
{
    public interface IJobPostService
    {
        Task<(bool Success, string Error, ICollection<JobPostListViewModel> Model)> ListForAdminAsync(bool activeOnly = false);
        Task<(bool Success, string Error)> SaveJobPostAsync(CreateJobPostViewModel model, string userId);
        Task<(bool Success, string Error, CreateJobPostViewModel Model)> FetchJobPostForEditAsync(int? id);
        Task<(bool Success, string Error)> DeleteJobPostAsync(int id);
        Task<(bool Success, string Error, ICollection<JobPostDetailViewModel> Model)> ListForPublicAsync();
    }
}
