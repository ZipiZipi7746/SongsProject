using Microsoft.Extensions.DependencyInjection;
using SongsProject.Services.Interfaces;

namespace SongsProject.Services
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISongService, SongServiceImpl>();
            services.AddScoped<IUserService, UserServiceImpl>();
            services.AddScoped<ITagService, TagServiceImpl>();
            services.AddScoped<IRecommendationService, RecommendationServiceImpl>();
            services.AddScoped<ISongTagService, SongTagServiceImpl>();
            services.AddScoped<IListeningHistoryService, ListeningHistoryServiceImpl>();
            services.AddScoped<RecommendationEngineService>();
            return services;
        }
    }
}