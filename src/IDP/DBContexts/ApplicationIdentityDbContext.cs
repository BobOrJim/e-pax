using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using IDP.Entities;

namespace IDP.DBContexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHostEnvironment _environment;

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options, IHostEnvironment environment) : base(options)
        {
            _environment = environment;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (_environment.IsDevelopment())
            {
                //Seeding AspNetRoles
                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "1", Name = "User", NormalizedName = "USER" });
                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" });
                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "3", Name = "Root", NormalizedName = "ROOT" });
                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "4", Name = "Spare1", NormalizedName = "SPARE1" });
                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "5", Name = "Spare2", NormalizedName = "SPARE2" });
                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "7", Name = "Masters_Degree_In_Forestry", NormalizedName = "Masters_Degree_In_Forestry" });
                modelBuilder.Entity<ApplicationRole>().HasData(new ApplicationRole { Id = "8", Name = "Masters_Degree_In_Forestry", NormalizedName = "Masters_Degree_In_Mining" });


                

                //Seeding AspNetUsers
                var hasher = new PasswordHasher<ApplicationRole>(); //a hasher to hash the password before seeding the user to the db
                string tmp = "ADMIN@ADMIN.com";
                modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = "1", UserName = tmp, NormalizedUserName = tmp, Email = tmp, NormalizedEmail = tmp, EmailConfirmed = true, PasswordHash = hasher.HashPassword(null, "admin") });
                tmp = "ROOT@ROOT.com";
                modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = "2", UserName = tmp, NormalizedUserName = tmp, Email = tmp, NormalizedEmail = tmp, EmailConfirmed = true, PasswordHash = hasher.HashPassword(null, "root") });
                tmp = "USER@USER.com";
                modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser { Id = "3", UserName = tmp, NormalizedUserName = tmp, Email = tmp, NormalizedEmail = tmp, EmailConfirmed = true, PasswordHash = hasher.HashPassword(null, "user") });

                //Seeding the relation between user and role to AspNetUserRoles
                modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "2", UserId = "1" }); //Give admin admin role
                modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "1", UserId = "1" }); //Give admin user role
                modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "3", UserId = "2" }); //Give root root role
                modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "1", UserId = "2" }); //Give root user role
                modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> { RoleId = "1", UserId = "3" }); //Give user user role
            }
        }
    }
}
