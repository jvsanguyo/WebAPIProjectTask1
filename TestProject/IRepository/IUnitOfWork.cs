using TestProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Summary> Summaries { get; }
        IGenericRepository<History> Histories { get; }
        Task Save();
        bool HasSummaryData();
        bool HasHistoryData();
    }
}