using jpfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.SeedData
{
    public class KaratSeed
    {
        public static object[] Data =
        {
            new Karat
            {
                KaratId = new Guid("0BAB0D22-F831-4B6C-B177-C623BA4BF5B9"),
                Name = "10K",
                InActive = false
            },
            new Karat
            {
                KaratId = new Guid("775D10C4-A955-4039-AAF6-16A80B0759F7"),
                Name = "14K",
                InActive = false
            },
            new Karat
            {
                KaratId = new Guid("4AAF69A9-7BB0-4F70-956D-4F55CD98FE1E"),
                Name = "18K",
                InActive = false
            },
            new Karat
            {
                KaratId = new Guid("4DA2D061-E089-4C8D-BFA4-534A301E0C87"),
                Name = "22K",
                InActive = false
            },
            new Karat
            {
                KaratId = new Guid("D9FB756F-933D-4CFA-9DC3-76714B84B256"),
                Name = "24K",
                InActive = false
            }
        };
    }
}
