using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.Infrasturcture.Repository
{
    public class RejectedLogRepository : IRejectedLogRepository
    {
        private readonly DatabaseContext _context;

        public RejectedLogRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RejectedLog rejectedLog)
        {
            _context.RejectedLogs.Add(rejectedLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RejectedLog>> GetPendingLogsAsync()
        {
            return await _context.RejectedLogs
                .Where(r => r.RetryCount < 5 && r.Timestamp < DateTime.UtcNow.AddHours(-1))
                .ToListAsync();
        }

        public async Task MarkAsProcessedAsync(int id)
        {
            var log = await _context.RejectedLogs.FindAsync(id);
            if (log != null)
            {
                log.RetryCount += 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementRetryCountAsync(int id)
        {
            var log = await _context.RejectedLogs.FindAsync(id);
            if (log != null)
            {
                log.RetryCount += 1;
                await _context.SaveChangesAsync();
            }
        }
    }
}
