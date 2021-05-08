using api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data {
    public class DataContext : IdentityDbContext<AppUser, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>> {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        // public DbSet<AppUser> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<UserRole>(userRole => {
                userRole.HasKey(u => new { u.UserId, u.RoleId });
                userRole.HasOne(r => r.Role)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(r => r.RoleId)
                    .IsRequired();
                userRole.HasOne(u => u.User)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(u => u.UserId)
                    .IsRequired();
            });
        }
    }
}