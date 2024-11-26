using Demo2CoreAPICrud.Data;
using Demo2CoreAPICrud.Interface;
using Demo2CoreAPICrud.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// log
builder.Services.AddHttpLogging(httplog =>
{
    httplog.LoggingFields = HttpLoggingFields.All;
    httplog.RequestHeaders.Add("sec-ch-ua");
    httplog.ResponseHeaders.Add("MyResponseHeader");
    httplog.MediaTypeOptions.AddText("application/javascript");
    httplog.RequestBodyLogLimit = 4096;
    httplog.ResponseBodyLogLimit = 4096;
    httplog.CombineLogs = true;
});

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());  // For mapping data to Dto
builder.Services.AddScoped<IUsersRepo, UsersRepo>();
builder.Services.AddScoped<ILogsRepo, LogsRepo>();
builder.Services.AddScoped<ILocationRepo, LocationRepo>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => opt.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Attendance Log",
    Description = "Web API to log attendance of users into a SQL Server database.",
    Version = "v1"     
}));

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// http logging
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
