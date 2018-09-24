using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{


    public class PolicyContext : DbContext
    {
        public PolicyContext(DbContextOptions<PolicyContext> options)
            : base(options)
        {
        }

        public DbSet<Policy> PolicyItems { get; set; }
    }
}
