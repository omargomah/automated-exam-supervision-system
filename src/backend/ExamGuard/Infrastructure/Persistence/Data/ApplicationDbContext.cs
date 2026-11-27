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
        public DbSet<Device> Devices => Set<Device>();
        public DbSet<Camera> Cameras => Set<Camera>();
        public DbSet<TelemetryNode> TelemetryNodes => Set<TelemetryNode>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<ExamSession> ExamSessions => Set<ExamSession>();
        public DbSet<ExamSeatAssignment> ExamSeatAssignments => Set<ExamSeatAssignment>();
        public DbSet<AlertEvent> AlertEvents => Set<AlertEvent>();
        public DbSet<CameraAlert> CameraAlerts => Set<CameraAlert>();
        public DbSet<TelemetryAlert> TelemetryAlerts => Set<TelemetryAlert>();
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
