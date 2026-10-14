using Domain.Entities;
using Infrastructure.Identity;
using Infrastructure.Persistence.DataSeeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<AppUser, IdentityRole<Guid>,Guid>(options)
    {
        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public DbSet<Classroom> Classrooms => Set<Classroom>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<CameraDevice> CameraDevices => Set<CameraDevice>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<ExamSession> ExamSessions => Set<ExamSession>();
        public DbSet<ExamSeatAssignment> ExamSeatAssignments => Set<ExamSeatAssignment>();
        public DbSet<AlertEvent> AlertEvents => Set<AlertEvent>();
        public DbSet<Evidence> Evidences => Set<Evidence>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.AddSeedingDataOfRoles();
        }
    }
}
