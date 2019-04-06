using jpfc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Data.SeedData
{
    public class MetalSeed
    {
        public static object[] Data =
        {
            new Metal
            {
                MetalId = new Guid("807F52D1-8F65-4F91-8408-3C5A04D830DD"),
                Name = "Gold",
                InActive = false
            },
            new Metal
            {
                MetalId = new Guid("2A883EFE-13FA-4A50-AD2B-AE49B034C8B0"),
                Name = "Silver",
                InActive = false
            },
            new Metal
            {
                MetalId = new Guid("B0FD2523-63A0-4CF3-940C-627617B3196F"),
                Name = "Platinum",
                InActive = false
            }
        };
    }
}
