using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pages.Models;

namespace Pages.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Page>(entity =>
            {
                entity.Property(e => e.AddedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }

        public DbSet<Page> Page { get; set; }

        public DbSet<NavLink> NavLink { get; set; }

        public DbSet<RelatedPages> RelatedPages { get; set; }
    }
}
