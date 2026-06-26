using Scalar.AspNetCore;
using Serilog;
using TaskFlow.Api.Features.Users.RegisterUser;
using TaskFlow.Api.Features.Users.GetUsers;
using TaskFlow.Api.Features.Projects.CreateProject;
using TaskFlow.Api.Features.Projects.GetProjects;
using TaskFlow.Api.Features.Projects.GetProjectById;
using TaskFlow.Api.Features.Projects.UpdateProject;
using TaskFlow.Api.Features.Projects.DeleteProject;
using TaskFlow.Api.Features.Issues.CreateIssue;
using TaskFlow.Api.Features.Issues.GetIssues;
using TaskFlow.Api.Features.Issues.GetIssueById;
using TaskFlow.Api.Features.Issues.UpdateIssue;
using TaskFlow.Api.Features.Issues.TransitionIssueStatus;
using TaskFlow.Api.Features.Issues.AssignIssue;
using TaskFlow.Api.Features.Issues.DeleteIssue;
using TaskFlow.Api.Features.Comments.AddComment;
using TaskFlow.Api.Features.Comments.GetComments;
using TaskFlow.Api.Features.Comments.DeleteComment;
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

api.MapGroup("/projects/{projectId:guid}/issues")
   .MapCreateIssue()
   .MapGetIssues();

api.MapGroup("/issues")
   .MapGetIssueById()
   .MapUpdateIssue()
   .MapTransitionIssueStatus()
   .MapAssignIssue()
   .MapDeleteIssue();

api.MapGroup("/issues/{issueId:guid}/comments")
   .MapAddComment()
   .MapGetComments();

api.MapGroup("/comments")
   .MapDeleteComment();

app.Run();

public partial class Program { }
