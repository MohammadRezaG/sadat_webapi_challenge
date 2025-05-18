using System.Threading.RateLimiting;
using Sadas_test.Data;
using Sadas_test.Middleware;
using Sadas_test.Models.DTOs;
using Sadas_test.Repositories.Implementations;
using Sadas_test.Repositories.Interfaces;
using Sadas_test.Services.Implementations;
using Sadas_test.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);





Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddTransient<LoggingHttpMessageHandler>();


builder.Services.AddHttpClient<IPriceSourcesService, PriceSourcesService>()
    .AddHttpMessageHandler<LoggingHttpMessageHandler>();




builder.Services.Configure<List<ProductSourceDto>>(
    builder.Configuration.GetSection("ProductSources"));





builder.Services.AddSingleton<DbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IPriceSourcesService, PriceSourcesService>();


builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(1),
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        });
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.Headers["Retry-After"] = "60";
        context.HttpContext.Response.ContentType = "text/plain";
        await context.HttpContext.Response.WriteAsync("? Too many requests. Please wait and try again.", token);
    };

});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();

app.Run();
