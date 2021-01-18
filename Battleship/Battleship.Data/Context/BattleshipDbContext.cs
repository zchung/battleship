using Battleship.Data.Context.Interfaces;
using Battleship.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Battleship.Data.Context
{
    public class BattleshipDbContext : DbContext, IBattleshipDbContext
    {
        public BattleshipDbContext(DbContextOptions<BattleshipDbContext> options): base(options)
        {
            
        }

        public DbSet<Game> Games { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().ToTable("Game");
        }
    }
}
