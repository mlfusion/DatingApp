using DatingApp.API.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Common.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            // Added IAuthRepository Service
            //services.AddScoped<IAuthRepository, AuthRepository>();

            // Added IDatingRepository Service
            services.AddScoped<IDatingRepository, DatingRepository>();

            // Added IPhotoRepository Service
            services.AddScoped<IPhotoRepository, PhotoRepository>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}