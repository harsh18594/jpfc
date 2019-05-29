using jpfc.Models;
using System;

namespace jpfc.Data.SeedData
{
    public class IdentificationDocumentSeed
    {
        public static object[] Data =
        {
            new IdentificationDocument
            {
                IdentificationDocumentId = new Guid("78faffc2-601a-4a06-b7ef-0e3b5a7f8b96"),
                Name = "Driver's License",
                SortOrder = null
            },
            new IdentificationDocument
            {
                IdentificationDocumentId = new Guid("4eed5d73-e2b9-406f-979f-7c124813eef3"),
                Name = "Passport",
                SortOrder = null
            },
            new IdentificationDocument
            {
                IdentificationDocumentId = new Guid("5bb91152-bee1-4afb-8c0b-aa95e6c4a9b5"),
                Name = "PR Card",
                SortOrder = null
            }
        };
    }
}
