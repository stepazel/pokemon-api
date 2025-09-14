using FluentValidation;
using YolkStudio.Pokemon.Infrastructure.Data;
using YolkStudio.Pokemon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Api.Trainers;
using YolkStudio.Pokemon.Core.Pokemons;
using YolkStudio.Pokemon.Core.Trainers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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