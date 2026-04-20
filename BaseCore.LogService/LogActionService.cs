using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using BaseCore.Repository.EFCore;
using BaseCore.LogService.Entities;

namespace BaseCore.LogService
{
    public interface ILogActionService : IRepository<LogAction>
    {
        Task<ICollection<LogAction>> GetAllListAsync();

        Task CreateLog(LogAction logAction);
    }

    public class LogActionService : Repository<LogAction>, ILogActionService
    {
        private readonly SqlServerDbContext _context;
        public LogActionService(SqlServerDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<ICollection<LogAction>> GetAllListAsync()
        {
            return await GetAllAsync();
        }

        public async Task CreateLog(LogAction logAction)
        {
            await AddAsync(logAction);
        }
    }
}
