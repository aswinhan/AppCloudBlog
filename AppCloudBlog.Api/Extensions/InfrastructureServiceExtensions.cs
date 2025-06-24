using AppCloudBlog.Infrastructure.Options;

namespace AppCloudBlog.Api.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register JwtSettings
        services.Configure<JwtSettings>(configuration.GetSection("ApiSettings"));

        // Register DB Connection
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register Repositories
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();


        return services;
    }
}
