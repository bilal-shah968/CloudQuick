using CloudQuick.Configurations;
using CloudQuick.Data;
using CloudQuick.Data.Repository;
using CloudQuick.MyLogging;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Clear default logging providers
builder.Logging.ClearProviders();

// Configure log4net
XmlConfigurator.Configure(new FileInfo(Path.Combine(AppContext.BaseDirectory, "log4net.config")));
builder.Logging.AddLog4Net();

// Optional: Add Serilog
#region Serilog Settings
// Uncomment to enable Serilog
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Information()
//     .WriteTo.File("Log/Log.txt", rollingInterval: RollingInterval.Day)
//     .CreateLogger();
// builder.Host.UseSerilog();
#endregion

builder.Services.AddDbContext<CloudDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CloudDBConnection"));
});

// Add services to the container
builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));


// Register logging service as singleton
builder.Services.AddSingleton<IMyLogger, LogToServerMemory>();

builder.Services.AddTransient<IMyLogger, LogToServerMemory>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();


var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
