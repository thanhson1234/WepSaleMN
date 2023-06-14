using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Entities
{
    public partial class BaseCoreDataContext : IdentityDbContext<AspNetUsers, AspNetRoles, int>
    {
        public BaseCoreDataContext() { }
        public BaseCoreDataContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspModule> AspModules { get; set; }
        public virtual DbSet<AspNetRoleModules> AspNetRoleModules { get; set; }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }    
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<GroupAcount> GroupAcounts { get; set; }
        public virtual DbSet<ImgProduct> ImgProducts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Units> Units { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("ThanhSonDB"));

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("AspNetUserClaims").HasKey(x => new { x.Id });
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("AspNetUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("AspNetRoleClaims").HasKey(x => new { x.Id });
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("AspUserTokens").HasKey(x => x.UserId);
        }

    }
}
