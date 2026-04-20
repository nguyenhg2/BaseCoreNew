using Microsoft.AspNetCore.Http;
using BaseCore.Repository.EFCore;
using BaseCore.LogService.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BaseCore.LogService
{
    public interface ILogErrorService : IRepository<LogError>
    {
        Task<ICollection<LogError>> GetAllListAsync();
        Task CreateLog(HttpContext httpContext, string message);
    }

    public class LogErrorService : Repository<LogError>, ILogErrorService
    {
        private readonly SqlServerDbContext _context;
        public LogErrorService(SqlServerDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task CreateLog(HttpContext httpContext, string message)
        {
            var requestBody = string.Empty;
            httpContext.Request.EnableBuffering();
            using (var reader = new StreamReader(httpContext.Request.Body))
            {
                requestBody = reader.ReadToEnd();
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                requestBody = reader.ReadToEnd();
            }

            var pathUrl = string.Format("{0}://{1}{2}", httpContext.Request.Scheme, httpContext.Request.Host, httpContext.Request.Path);
            var logError = new LogError
            {
                Header = $"REQUEST HttpMethod: {httpContext.Request.Method}, Path: {pathUrl}, Content-Type: {httpContext.Request.ContentType}",
                Body = requestBody,
                CreatedUser = httpContext.User.Identity.Name, 
                Message = message
            };

           await AddAsync(logError);
        }

        public async Task<ICollection<LogError>> GetAllListAsync()
        {
            return await GetAllAsync();
        }
    }
}
