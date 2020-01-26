using jpfc.Classes;
using jpfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.SeedData
{
    public class JobTypeSeed
    {
        public static object[] Data =
        {
            new JobType
            {
                JobTypeId = new Guid(Constants.JobPost.JobTypeId.FtPermanent),
                Type = "Full-time Permanent"
            },
            new JobType
            {
                JobTypeId = new Guid(Constants.JobPost.JobTypeId.PtPermanent),
                Type = "Part-time Permanent"
            },
            new JobType
            {
                JobTypeId = new Guid(Constants.JobPost.JobTypeId.FtTemporary),
                Type = "Full-time Temporary"
            },
            new JobType
            {
                JobTypeId = new Guid(Constants.JobPost.JobTypeId.PtTemporary),
                Type = "Part-time Temporary"
            },
            new JobType
            {
                JobTypeId = new Guid(Constants.JobPost.JobTypeId.FtContract),
                Type = "Full-time Contract"
            },
            new JobType
            {
                JobTypeId = new Guid(Constants.JobPost.JobTypeId.PtContract),
                Type = "Part-time Contract"
            }
        };
    }
}
