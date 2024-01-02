using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChauffeurApp.Application.Services.IServices
{
    public interface IBaseEntity
    {
        
        public long Id { get; set; }
    }
}
