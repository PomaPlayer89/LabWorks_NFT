using Microsoft.EntityFrameworkCore;
using nat.Storage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nat.Storage.Migrations
{
    public class CenterDataContext : DbContext
    {
        public CenterDataContext(DbContextOptions<CenterDataContext> options) : base(options)
        {

        }
        public DbSet<Center> Center { get; set; }
        public DbSet<Trainer> Trainer { get; set; }
        public DbSet<Customer> Customer { get; set; }
    }
}
