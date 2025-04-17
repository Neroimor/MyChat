using ChatApi.DTO.UserDTO;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Services.DataBase
{
    public class AppDBContext : DbContext
    {

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.UserData)
                .HasForeignKey(m => m.UserDataId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<UserData> Users { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

    }
}
