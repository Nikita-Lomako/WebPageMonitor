using Microsoft.EntityFrameworkCore;
using WebPageMonitor.Core.Parsers;
using WebPageMonitor.Infrastructure.Data;
using WebPageMonitor.Infrastructure.Parsers;
using WebPageMonitor.Infrastructure.Repositories;
using WebPageMonitor.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IWatchedPageRepository, EfWatchedPageRepository>();
builder.Services.AddScoped<IPageVersionRepository, EfPageVersionRepository>();
builder.Services.AddScoped<IChangeLogRepository, EfChangeLogRepository>();

builder.Services.AddHttpClient<RwByParser>();
builder.Services.AddHttpClient<GismeteoParser>();

builder.Services.AddScoped<RwByService>();
builder.Services.AddScoped<GismeteoService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
