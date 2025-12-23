using Microsoft.EntityFrameworkCore;
using projec.Models;

namespace projec.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamPlayer> TeamPlayers { get; set; }
        public DbSet<MatchRequest> MatchRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Field>()
                .Property(f => f.HourlyRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    
                    Name = "enes",
                    Username = "varım",
                    BirthYear = 2002,
                    Password = "asdqwe",
                    Position = Position.Ortasaha,
                    MatchCount = 39,
                    GoalCount = 998,
                    AssistCount = 528,
                    Role = UserRole.Admin
                }
            );
        }
    }
}