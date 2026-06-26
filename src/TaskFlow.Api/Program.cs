using Scalar.AspNetCore;
using Serilog;
using TaskFlow.Api.Infrastructure.Common.Extensions;
using TaskFlow.Api.Infrastructure.Common.Middleware;
using TaskFlow.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

var api = app.MapGroup("/api");

app.Run();

public partial class Program { }
