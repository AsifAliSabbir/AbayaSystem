using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbayaSystem.Core
{
    public interface IExternalWorkerService
    {
        Task<List<ExternalWorker>> GetExternalWorkersAsync();
        Task<ExternalWorker> CreateExternalWorkerAsync(ExternalWorker worker);
        Task UpdateExternalWorkerAsync(ExternalWorker worker);
        Task DeleteExternalWorkerAsync(int externalWorkerId);
    }
}




