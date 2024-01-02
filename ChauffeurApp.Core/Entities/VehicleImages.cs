using ChauffeurApp.Core.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChauffeurApp.Core.Entities
{
    public class VehicleImages : BaseEntity, IAuditedEntity
    {
        //public string? Customers { get; set; }
        public string ImageUrl { get; set; }

        //relationship

        public long VehicleID { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime? CreatedAt { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedById { get; set; }
    }
}
