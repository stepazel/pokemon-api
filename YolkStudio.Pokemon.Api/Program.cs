using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.Sqlite;
using YolkStudio.Pokemon.Infrastructure.Data;
using YolkStudio.Pokemon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Api.Shared;
using YolkStudio.Pokemon.Api.Trainers;
using YolkStudio.Pokemon.Core.Pokemons;
using YolkStudio.Pokemon.Core.Trainers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Here I reference the Infrastructure layer, which could be avoided by having a separate project for DI.
builder.Services.AddDbContext<PokemonDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Trainer services
builder.Services.AddScoped<IValidator<CreateTrainerRequest>, CreateTrainerValidator>();
builder.Services.AddScoped<IValidator<UpdateTrainerRequest>, UpdateTrainerValidator>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

// Pokemon services
builder.Services.AddScoped<IPokemonService, PokemonService>();
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();

var app = builder.Build();

// !! This is just a hack for using SQLite in Heroku dyno, which is ephemeral and resets the file system once a day.
using (app.Services.CreateScope())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var scriptPath = Path.Combine(app.Environment.ContentRootPath, "pokemon_database_data_init.sql");
    var script = File.ReadAllText(scriptPath);

    using var command = connection.CreateCommand();
    command.CommandText = script;
    command.ExecuteNonQuery();
}

app.UseExceptionHandler(_ => {});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();