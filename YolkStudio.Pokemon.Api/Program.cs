using FluentValidation;
using YolkStudio.Pokemon.Infrastructure.Data;
using YolkStudio.Pokemon.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Api.Trainers;
using YolkStudio.Pokemon.Core.Trainers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IValidator<CreateTrainerRequest>, CreateTrainerValidator>();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddFluentValidationAutoValidation();

// Zde referencuji Infrastructure layer, coz bychom mohli obejit vytvorenim projektu, ktery se stara o DI a registraci sluzeb.
builder.Services.AddDbContext<PokemonDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

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