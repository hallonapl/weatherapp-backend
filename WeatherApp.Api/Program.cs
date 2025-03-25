using WeatherApp.Api.Extensions;
using WeatherApp.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices();
builder.Services.AddCoreServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(); //Configure logging to write to db.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection(); //Swagger over https...

app.UseAuthorization();

app.MapControllers();

app.Run();
