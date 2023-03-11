using MegaSenaWeb.Interfaces;
using MegaSenaWeb.Services;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();


builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IGeraNumeroMegaSena, GeraNumeroMegaSena>()
    .AddHttpClient("geraNumeroMegaSena", config =>
    {
        config.BaseAddress = new Uri(builder.Configuration["ExternalServices:GeraNumeroAPI:url"]);
    })
    .AddPolicyHandler(GetRetryPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(
        retryCount: 6,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
        onRetry: (ex, ts, retryCount, context) =>
        {
            Console.WriteLine($"Retry Policy - Attempt {retryCount} - Error {ex.Exception?.Message}");
        });
}
