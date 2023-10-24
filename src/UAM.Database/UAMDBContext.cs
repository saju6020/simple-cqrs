using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace SimpleCQRS.UAM.Database
{
    public class UAMDBContext : IdentityDbContext<User,Role,Guid,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {              

        public UAMDBContext() { }

        public UAMDBContext(DbContextOptions<UAMDBContext> options) : base (options) 
        { 

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-KTI6J0L\\SQLEXPRESS;Initial Catalog=UAM;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
             {
                 // Each User can have many UserClaims
                 b.HasMany(e => e.Claims)
                     .WithOne(e => e.User)
                     .HasForeignKey(uc => uc.UserId)
                     .IsRequired();

                 // Each User can have many UserLogins
                 b.HasMany(e => e.Logins)
                     .WithOne(e => e.User)
                     .HasForeignKey(ul => ul.UserId)
                     .IsRequired();

                 // Each User can have many UserTokens
                 b.HasMany(e => e.Tokens)
                     .WithOne(e => e.User)
                     .HasForeignKey(ut => ut.UserId)
                     .IsRequired();

                 // Each User can have many entries in the UserRole join table
                 b.HasMany(e => e.UserRoles)
                     .WithOne(e => e.User)
                     .HasForeignKey(ur => ur.UserId)
                     .IsRequired();
             });
            

            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e=> e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            AddCustomTableName(modelBuilder);
        }

        private void AddCustomTableName(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
            });

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<UserLogin>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<UserToken>(b =>
            {
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRoles");
            });
        }
    }
}
