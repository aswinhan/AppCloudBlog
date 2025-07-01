var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddCorsPolicies(builder.Configuration);           // CORS
builder.Services.AddApplicationServices();                         // MediatR, Validators, Behaviors
builder.Services.AddInfrastructureServices(builder.Configuration); // DB, Repositories
builder.Services.AddJwtAuthentication(builder.Configuration);      // JWT Auth Setup
builder.Services.AddSwaggerWithJwt();                              // Swagger + JWT Auth

// Build App
var app = builder.Build();

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.UseCorsPolicies();                    // CORS middleware
app.UseMiddleware<ExceptionMiddleware>(); // Global exception Middleware
app.UseAuthentication();
app.UseAuthorization();

// Endpoint Mapping
app.MapGroup("/api/auth").MapAuthEndpoints();
app.MapGroup("/api/posts").MapBlogPostEndpoints();
app.MapGroup("/api").MapTagEndpoints();
app.MapGroup("/api").MapCommentEndpoints();
app.MapGroup("/api").MapUserEndpoints();
app.MapGroup("/api").MapNotificationEndpoints();


app.Run();
