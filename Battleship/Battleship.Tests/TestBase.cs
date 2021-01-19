using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;

namespace Battleship.Tests
{
    public class TestBase
    {
        public DbSet<T> SetupDbContextList<T>(IQueryable<T> mockList) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(mockList.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(mockList.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(mockList.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => mockList.GetEnumerator());

            return dbSet.Object;
        }
    }
}
