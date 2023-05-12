using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = 
                               Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddDbContext<Bme.Aut.Logistics.Dal.LogisticsDbContext>(
    options => options.UseSqlite("Data Source=logistics.db"));

var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { } // so you can reference it from tests
