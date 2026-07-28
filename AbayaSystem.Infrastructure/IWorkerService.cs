using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbayaSystem.Core
{
    public interface IWorkerService
    {
        Task<List<Worker>> GetWorkersAsync();
        Task<List<Branch>> GetBranchesAsync();
        Task<Worker> CreateWorkerAsync(Worker worker);
        Task UpdateWorkerAsync(Worker worker);
        Task DeleteWorkerAsync(int workerId);
    }
}