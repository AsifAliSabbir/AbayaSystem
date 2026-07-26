using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbayaSystem.Core;
using Microsoft.EntityFrameworkCore; // Ensures EF Core Async extensions are used

namespace AbayaSystem.Infrastructure
{
    public class ExternalWorkerService : IExternalWorkerService
    {
        private readonly BoutiqueDbContext _context;

        public ExternalWorkerService(BoutiqueDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExternalWorker>> GetExternalWorkersAsync()
        {
            return await _context.ExternalWorkers
                .Where(w => w.IsActive)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<ExternalWorker> CreateExternalWorkerAsync(ExternalWorker worker)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));

            worker.IsActive = true;
            _context.ExternalWorkers.Add(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        public async Task UpdateExternalWorkerAsync(ExternalWorker worker)
        {
            var existing = await _context.ExternalWorkers
                .FirstOrDefaultAsync(w => w.ExternalWorkerId == worker.ExternalWorkerId);

            if (existing == null)
            {
                throw new KeyNotFoundException($"External worker with ID {worker.ExternalWorkerId} not found.");
            }

            existing.Name = worker.Name;
            existing.Phone = worker.Phone;
            existing.SupportedType = worker.SupportedType;
            existing.IsActive = worker.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteExternalWorkerAsync(int externalWorkerId)
        {
            var worker = await _context.ExternalWorkers
                .FirstOrDefaultAsync(w => w.ExternalWorkerId == externalWorkerId);

            if (worker != null)
            {
                _context.ExternalWorkers.Remove(worker);
                await _context.SaveChangesAsync();
            }
        }
    }
}