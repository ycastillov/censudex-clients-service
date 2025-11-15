using ClientsService.Src.Data;
using ClientsService.Src.Interfaces;
using ClientsService.Src.Repositories;
using ClientsService.Src.Validators;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cargar archivo .env (si existe)
Env.Load();

// Construir el connection string desde variables de entorno
var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
var dbPass = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(
        7181,
        o =>
        {
            o.UseHttps(); // necesario para gRPC
            o.Protocols = HttpProtocols.Http2;
        }
    );
});

var connectionString =
    $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass}";

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();

// builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ClientCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClientUpdateValidator>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Implementar el Data Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DataSeeder.Initialize(services);
}

app.UseRouting();
app.UseGrpcWeb();

app.MapGrpcService<ClientsGrpcService>().EnableGrpcWeb();

app.MapGrpcReflectionService();

app.MapGet("/", () => "ClientsService running with gRPC");

app.Run();
