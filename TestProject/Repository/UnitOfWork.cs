using TestProject.Data;
using TestProject.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Summary> _summaries;
        private IGenericRepository<History> _histories;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }
        public IGenericRepository<Summary> Summaries => _summaries ??= new GenericRepository<Summary>(_context);

        public IGenericRepository<History> Histories => _histories ??= new GenericRepository<History>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool HasHistoryData()
        {
            return _context.Histories.Any();
        }

        public bool HasSummaryData()
        {
            return _context.Summaries.Any();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}