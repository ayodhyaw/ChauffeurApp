using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChauffeurApp.Core.Entities
{
    public class ApplicationUserRole : IdentityRole<long>
    {
        public ApplicationUserRole() : base()
        {

        }
        public ApplicationUserRole(string roleName) : base(roleName)
        {
        }
    }
}
