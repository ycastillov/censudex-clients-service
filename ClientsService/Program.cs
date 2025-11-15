using ClientsService.Src.Data;
using ClientsService.Src.Interfaces;
using ClientsService.Src.Repositories;
using ClientsService.Src.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

//builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))
);

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ClientCreateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClientUpdateValidator>();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(
//         "AllowAll",
//         policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
//     );
// });

var app = builder.Build();

// Implementar el Data Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DataSeeder.Initialize(services);
}

app.UseRouting();
app.UseGrpcWeb();

// app.UseCors("AllowAll");

app.MapGrpcService<ClientsGrpcService>().EnableGrpcWeb();

app.MapGrpcReflectionService();

app.MapGet("/", () => "ClientsService running with gRPC");

app.Run();
