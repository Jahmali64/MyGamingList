using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyGamingList.Infrastructure.DbContexts;

namespace MyGamingList.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContextFactory<MyGamingListDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
