using jpfc.Models;
using jpfc.Models.JobPostViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.Interfaces
{
    public interface IJobRepository
    {
        Task<bool> SaveAsync<T>(T entity) where T : class;
        Task<bool> DeleteAsync<T>(T entity) where T : class;
        Task<ICollection<JobPostListViewModel>> ListJobPostAsync(bool activeOnly = false);
        Task<JobPost> FetchByIdAsync(int id);
    }
}
