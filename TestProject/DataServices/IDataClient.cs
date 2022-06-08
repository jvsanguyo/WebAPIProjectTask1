using System.Collections.Generic;
using System.Threading.Tasks;
using TestProject.Models;

namespace TestProject.DataServices
{
    public interface IDataClient
    {
        Task<CovidSummary> GetSummaries();
        Task<IEnumerable<CovidHistory>> GetHistory(string country);
    }
}