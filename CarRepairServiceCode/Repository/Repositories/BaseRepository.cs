using Microsoft.EntityFrameworkCore;
using System;

namespace CarRepairServiceCode.Repository.Repositories
{
    public abstract class BaseRepository<TContext> : IDisposable where TContext : DbContext
    {
        protected readonly TContext _context;

        protected BaseRepository(TContext context)
        {
            _context = context;
        }

        public virtual void Dispose()
        {
            _context?.Dispose();
        }
    }
}
