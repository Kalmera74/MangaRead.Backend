using Microsoft.Extensions.DependencyInjection;

namespace MangaRead.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services.AddScoped<>();

        services.AddScoped<IBucketService, HetznerBucketService>();
        services.AddScoped<FileService>();
        return services;
    }
}
