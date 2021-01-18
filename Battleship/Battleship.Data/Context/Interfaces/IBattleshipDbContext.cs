
using Battleship.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship.Data.Context.Interfaces
{
    public interface IBattleshipDbContext
    {
        DbSet<Game> Games { get; set; }
        public DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync();
        ValueTask<EntityEntry> AddAsync([NotNullAttribute] object entity, CancellationToken cancellationToken = default);
    }
}
