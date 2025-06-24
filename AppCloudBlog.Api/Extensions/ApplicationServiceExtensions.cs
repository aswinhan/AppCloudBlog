namespace AppCloudBlog.Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        // Register MediatR Handlers (CQRS)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(currentAssembly));

        // Register FluentValidation Validators
        services.AddValidatorsFromAssembly(currentAssembly);

        // MediatR Pipeline Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        // HTTP Context Accessor
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}
