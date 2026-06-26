using Scalar.AspNetCore;
using Serilog;
using TaskFlow.Api.Features.Users.RegisterUser;
using TaskFlow.Api.Features.Users.GetUsers;
using TaskFlow.Api.Features.Projects.CreateProject;
using TaskFlow.Api.Features.Projects.GetProjects;
using TaskFlow.Api.Features.Projects.GetProjectById;
using TaskFlow.Api.Features.Projects.UpdateProject;
using TaskFlow.Api.Features.Projects.DeleteProject;
using TaskFlow.Api.Infrastructure.Common.Extensions;
using TaskFlow.Api.Infrastructure.Common.Middleware;

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

api.MapGroup("/users")
   .MapRegisterUser()
   .MapGetUsers();

api.MapGroup("/projects")
   .MapCreateProject()
   .MapGetProjects()
   .MapGetProjectById()
   .MapUpdateProject()
   .MapDeleteProject();

app.Run();

public partial class Program { }
