using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbayaSystem.Core;
using Microsoft.EntityFrameworkCore;

namespace AbayaSystem.Infrastructure
{
    public class WorkerService : IWorkerService
    {
        private readonly BoutiqueDbContext _context;

        public WorkerService(BoutiqueDbContext context)
        {
            _context = context;
        }

        public async Task<List<Worker>> GetWorkersAsync()
        {
            return await _context.Workers
                .Include(w => w.Branch)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<List<Branch>> GetBranchesAsync()
        {
            return await _context.Branches
                .OrderBy(b => b.BranchName)
                .ToListAsync();
        }

        public async Task<Worker> CreateWorkerAsync(Worker worker)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));

            // 🔑 Check if password/PIN is already assigned to another worker
            var isDuplicatePassword = await _context.Workers
                .AnyAsync(w => w.PasswordHash == worker.PasswordHash.Trim());

            if (isDuplicatePassword)
            {
                throw new InvalidOperationException($"The PIN/Password '{worker.PasswordHash}' is already in use by another worker.");
            }

            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        public async Task UpdateWorkerAsync(Worker worker)
        {
            var existing = await _context.Workers
                .FirstOrDefaultAsync(w => w.WorkerId == worker.WorkerId);

            if (existing == null)
            {
                throw new KeyNotFoundException($"Worker with ID {worker.WorkerId} not found.");
            }

            // 🔑 If a new password is provided, check that no OTHER worker has it
            if (!string.IsNullOrWhiteSpace(worker.PasswordHash))
            {
                var cleanPassword = worker.PasswordHash.Trim();
                var isDuplicatePassword = await _context.Workers
                    .AnyAsync(w => w.WorkerId != worker.WorkerId && w.PasswordHash == cleanPassword);

                if (isDuplicatePassword)
                {
                    throw new InvalidOperationException($"The PIN/Password '{cleanPassword}' is already in use by another worker.");
                }

                existing.PasswordHash = cleanPassword;
            }

            existing.Name = worker.Name;
            existing.Username = worker.Username;
            existing.AssignedRoles = worker.AssignedRoles;
            existing.BranchId = worker.BranchId;

            if (!string.IsNullOrWhiteSpace(worker.PasswordHash))
            {
                existing.PasswordHash = worker.PasswordHash;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkerAsync(int workerId)
        {
            var worker = await _context.Workers
                .FirstOrDefaultAsync(w => w.WorkerId == workerId);

            if (worker != null)
            {
                _context.Workers.Remove(worker);
                await _context.SaveChangesAsync();
            }
        }
    }
}