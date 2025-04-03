using System;
using WeatherApp.Api.Extensions;
using WeatherApp.Api.Services.Hosted;
using WeatherApp.Core.Clients;
using WeatherApp.Core.Configuration;
using WeatherApp.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices();
var connectionString = builder.Configuration.GetConnectionString("WeatherDataDb");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new ArgumentNullException("Connection string is not configured");
}
builder.Services.AddCoreServices(connectionString);

builder.Services.AddOptions<WeatherDataClientSettings>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection("WeatherDataClientSettings").Bind(settings);
        });
builder.Services.AddHttpClient(nameof(WeatherDataClient), client =>
{
    var baseUrl = builder.Configuration.GetSection("WeatherDataClientSettings").GetValue<string>("BaseUrl");
    if(string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new ArgumentNullException("BaseUrl is not configured");
    }
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHostedService<WeatherReportFetchService>();

builder.Services.AddLogging(); //Configure logging to write to db.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowReactApp"); 
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
