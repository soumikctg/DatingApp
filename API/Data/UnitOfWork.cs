using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public async Task<long> SaveChangesAsync()
        {
            if (!_context.ChangeTracker.HasChanges())
            {
                return 0;
            }

            return await _context.SaveChangesAsync();
        }
    }
}
