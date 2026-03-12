using Microsoft.Extensions.DependencyInjection;
using SongsProject.Repositories.Interfaces;

namespace SongsProject.Repositories
{
    public static class RepositoryDependencies
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IRecommendationRepository, RecommendationRepository>();
            services.AddScoped<ISongTagRepository, SongTagRepository>();
            services.AddScoped<IListeningHistoryRepository, ListeningHistoryRepository>();
            return services;
        }
    }
}