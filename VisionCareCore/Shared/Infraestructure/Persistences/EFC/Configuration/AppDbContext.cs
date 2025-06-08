using System.Collections.Immutable;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using VisionCareCore.HealthCare.Domain.Model.Aggregates;
using VisionCareCore.User.Domain.Model.Aggregates;


namespace VisionCareCore.Shared.Infraestructure.Persistences.EFC.Configuration
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

     
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            builder.EnableSensitiveDataLogging();
            builder.AddCreatedUpdatedInterceptor();

            base.OnConfiguring(builder);
        }
        
        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<AuthUserRefreshToken> AuthUsersRefreshTokens { get; set; }
        public DbSet<Medicine> Medicines { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<AuthUser>(authUser =>
            {
                authUser.HasKey(u => u.Id);
                authUser.Property(u => u.Id).IsRequired();
                authUser.Property(u => u.Email).IsRequired().HasMaxLength(255);
                authUser.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_AuthUser_Email");
                authUser.Property(u => u.PasswordHash).IsRequired();
            });
            
            builder.Entity<AuthUserRefreshToken>(refreshToken =>
            {
                refreshToken.HasKey(rt => rt.Id);

                refreshToken.HasOne(rt => rt.AuthUser)
                    .WithMany()
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Medicine>(medicine =>
            {
                medicine.HasKey(m => m.Id);
                medicine.Property(m => m.Nombre).IsRequired().HasMaxLength(100);
                medicine.Property(m => m.Description).IsRequired().HasMaxLength(500);
                medicine.Property(m => m.SideEffects).HasMaxLength(500);
                medicine.Property(m => m.Warnings).HasMaxLength(500);
                medicine.Property(m => m.IsDeleted).IsRequired();
                medicine.Property(m => m.UserId).IsRequired();
            });

            
            
        }
            
            
}
}
