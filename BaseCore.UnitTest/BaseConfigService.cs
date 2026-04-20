using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using BaseCore.Common;
using System.Configuration;
using BaseCore.Repository.EFCore;
using Microsoft.Extensions.DependencyInjection;
using BaseCore.Repository.Authen;
using Microsoft.EntityFrameworkCore;

namespace BaseCore.UnitTest
{
    public class BaseConfigService
    {
        public IOptions<AppSettings> Option;
        public readonly IConfiguration ConfigurationRoot;

        public BaseConfigService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            ConfigurationRoot = builder.Build();
            Option = Options.Create(new AppSettings
            {
                Secret = ""
            });

            IServiceCollection service = new ServiceCollection();
            var connectionString = ConfigurationRoot.GetSection("ConnectionString").Value;
            
            service.AddDbContext<SqlServerDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            service.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
