using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Configurations;

namespace TestProject.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }


        public DbSet<Summary> Summaries { get; set; }
        public DbSet<History> Histories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity(typeof(History)).HasNoKey();
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}