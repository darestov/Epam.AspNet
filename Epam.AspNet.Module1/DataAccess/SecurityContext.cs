using Epam.AspNet.Module1.Models.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.DataAccess
{
    public class SecurityContext: IdentityDbContext<User>
    {
        public SecurityContext(DbContextOptions<SecurityContext> options)
            :base(options)
        {
            
        }
    }
}
