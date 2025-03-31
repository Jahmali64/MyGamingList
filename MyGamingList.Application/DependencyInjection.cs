using Microsoft.Extensions.DependencyInjection;
using MyGamingList.Application.Services.VideoGames;

namespace MyGamingList.Application;

public static class DependencyInjection {
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddScoped<IVideoGameService, VideoGameService>();
        
        return services;
    }
}
