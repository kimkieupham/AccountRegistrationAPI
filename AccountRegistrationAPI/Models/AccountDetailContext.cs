using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountRegistrationAPI.Models
{
    public class AccountDetailContext : DbContext
    {
        public AccountDetailContext(DbContextOptions<AccountDetailContext> options):base(options)
        {

        }

        public DbSet<AccountDetail> AccountDetails { get; set; }
    }
}
