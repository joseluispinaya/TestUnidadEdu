using Capa.Backend.Data;
using Capa.Backend.Helpers;
using Capa.Backend.Repositories.Implementations;
using Capa.Backend.Repositories.Intefaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));
builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IFileStorage, FileStorage>();

builder.Services.AddHttpClient<IIARecommendationService, IARecommendationService>();

builder.Services.AddScoped<IRespuestasRepository, RespuestasRepository>();
builder.Services.AddScoped<IPreguntasRepository, PreguntasRepository>();
builder.Services.AddScoped<IEstudiantesRepository, EstudiantesRepository>();
//builder.Services.AddScoped<IEstudiantesRepository, EstudiantesRepository>();

var app = builder.Build();

SeedData(app);

void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using var scope = scopedFactory!.CreateScope();
    var service = scope.ServiceProvider.GetService<SeedDb>();
    service!.SeedAsync().Wait();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
