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
            // gold
            new Karat
            {
                KaratId = new Guid("F47E14CB-9FC9-4AAF-9F2D-A64FC6A15C2F"),
                Name = "9K",
                InActive = false,
                MetalId = new Guid("807F52D1-8F65-4F91-8408-3C5A04D830DD")
            },
            new Karat
            {
                KaratId = new Guid("0BAB0D22-F831-4B6C-B177-C623BA4BF5B9"),
                Name = "10K",
                InActive = false,
                MetalId = new Guid("807F52D1-8F65-4F91-8408-3C5A04D830DD")
            },
            new Karat
            {
                KaratId = new Guid("775D10C4-A955-4039-AAF6-16A80B0759F7"),
                Name = "14K",
                InActive = false,
                MetalId = new Guid("807F52D1-8F65-4F91-8408-3C5A04D830DD")
            },
            new Karat
            {
                KaratId = new Guid("4AAF69A9-7BB0-4F70-956D-4F55CD98FE1E"),
                Name = "18K",
                InActive = false,
                MetalId = new Guid("807F52D1-8F65-4F91-8408-3C5A04D830DD")
            },
            new Karat
            {
                KaratId = new Guid("4DA2D061-E089-4C8D-BFA4-534A301E0C87"),
                Name = "22K",
                InActive = false,
                MetalId = new Guid("807F52D1-8F65-4F91-8408-3C5A04D830DD")
            },
            new Karat
            {
                KaratId = new Guid("D9FB756F-933D-4CFA-9DC3-76714B84B256"),
                Name = "24K",
                InActive = false,
                MetalId = new Guid("807F52D1-8F65-4F91-8408-3C5A04D830DD")
            },
            // platinum
            new Karat
            {
                KaratId = new Guid("06FF5E11-00CB-4CAB-AE61-8237A13AE60F"),
                Name = "950",
                InActive = false,
                MetalId = new Guid("B0FD2523-63A0-4CF3-940C-627617B3196F")
            },
            // silver
            new Karat
            {
                KaratId = new Guid("EF76A7FE-0D8D-4814-83DF-47A4A035D703"),
                Name = "925",
                InActive = false,
                MetalId = new Guid("2A883EFE-13FA-4A50-AD2B-AE49B034C8B0")
            }
        };
    }
}
