namespace AppCloudBlog.Api.Extensions;

public static class CorsExtensions
{
    //private const string DevCorsPolicy = "DevPolicy";
    private const string DefaultCorsPolicy = "DefaultPolicy";

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

            options.AddPolicy(name: DefaultCorsPolicy, policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });

            //options.AddPolicy(name: DevCorsPolicy, policy =>
            //{
            //    policy.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //});
        });

        return services;
    }

    public static IApplicationBuilder UseCorsPolicies(this IApplicationBuilder app)
    {
        app.UseCors(DefaultCorsPolicy);
        //app.UseCors(DevCorsPolicy);
        return app;
    }
}
