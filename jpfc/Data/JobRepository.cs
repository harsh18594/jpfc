using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.DropdownViewModels;
using jpfc.Models.JobPostViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync<T>(T entity) where T : class
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Add(entity);
            }
            else if (_context.Entry(entity).State == EntityState.Modified)
            {
                _context.Update(entity);
            }

            if (_context.Entry(entity).State == EntityState.Unchanged)
            {
                return true;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            _context.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<JobPostListViewModel>> ListJobPostAsync(bool activeOnly = false)
        {
            return await _context.JobPost
                  .Where(j => !activeOnly || (!j.IsClosed && (!j.JobCloseUtc.HasValue || j.JobCloseUtc > DateTime.UtcNow)))
                  .OrderByDescending(j => j.JobStartDate)
                  .ThenByDescending(j => j.CreatedUtc)
                  .Select(j => new JobPostListViewModel
                  {
                      JobPostId = j.JobPostId,
                      Title = j.JobTitle,
                      Location = j.JobLocation,
                      StartDate = j.JobStartDate,
                      Type = j.JobType.Type,
                      IsDraft = j.IsDraft,
                      IsClosed = j.IsClosed || (j.JobCloseUtc.HasValue && j.JobCloseUtc < DateTime.UtcNow)
                  })
                  .ToListAsync();
        }

        public async Task<JobPost> FetchByIdAsync(int id)
        {
            return await _context.JobPost
                .Where(j => j.JobPostId == id)
                .Include(j => j.JobType)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<DropdownItemViewModel>> ListJobTypesForDropdownAsync()
        {
            return await _context.JobType
                .OrderBy(t => t.Type)
                .Select(j => new DropdownItemViewModel
                {
                    Value = j.JobTypeId.ToString(),
                    Text = j.Type
                })
                .ToListAsync();
        }

        public async Task<ICollection<JobPostDetailViewModel>> ListJobPostForPublicAsync()
        {
            return await _context.JobPost
                  .Where(j => !j.IsDraft && !j.IsClosed && (!j.JobCloseUtc.HasValue || j.JobCloseUtc > DateTime.UtcNow))
                  .OrderBy(j => j.JobStartDate)
                  .ThenBy(j => j.CreatedUtc)
                  .ThenBy(j => j.JobTitle)
                  .Select(j => new JobPostDetailViewModel
                  {
                      Id = j.JobPostId,
                      Title = j.JobTitle,
                      Location = j.JobLocation,
                      StartDate = j.JobStartDate,
                      Type = j.JobType.Type,
                      Description = j.Description,
                      Requirements = j.Requirements,
                      Length = j.Length,
                      Pay = j.Pay
                  })
                  .ToListAsync();
        }
    }
}
