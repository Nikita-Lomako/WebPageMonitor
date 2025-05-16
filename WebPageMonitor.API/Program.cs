using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebPageMonitor.Infrastructure.Data;
using WebPageMonitor.Infrastructure.Parsers;
using WebPageMonitor.Infrastructure.Repositories;
using WebPageMonitor.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// CORS: Allow specific origins
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7227") // Client URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddScoped<IWatchedPageRepository, EfWatchedPageRepository>();
builder.Services.AddScoped<IPageVersionRepository, EfPageVersionRepository>();
builder.Services.AddScoped<IChangeLogRepository, EfChangeLogRepository>();

// Register parsers
builder.Services.AddHttpClient<GismeteoParser>();

builder.Services.AddScoped<GismeteoService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();

app.Run();
