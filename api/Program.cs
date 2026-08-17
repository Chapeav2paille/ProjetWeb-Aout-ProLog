using System.Text;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProManLog.Core.Interfaces;
using ProManLog.Core.Services;
using ProManLog.Infrastructure.Data;
using ProManLog.Infrastructure.Repositories;
using ProManLog.Infrastructure.Security;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// --- Injection de dépendances ---
builder.Services.AddSingleton<IDbConnectionFactory>(
    new SqliteConnectionFactory(builder.Configuration.GetConnectionString("Default")!));
//Client
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
//Vehicule
builder.Services.AddScoped<IVehiculeRepository, VehiculeRepository>();
builder.Services.AddScoped<IVehiculeService, VehiculeService>();
//Employe
builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IEmployeService, EmployeService>();
//Prestation
builder.Services.AddScoped<IPrestationRepository, PrestationRepository>();
builder.Services.AddScoped<IPrestationService, PrestationService>();
//Historique
builder.Services.AddScoped<IHistoriqueRepository, HistoriqueRepository>();
builder.Services.AddScoped<IHistoriqueService, HistoriqueService>();
//TableauBord
builder.Services.AddScoped<ITableauBordService, TableauBordService>();
//Chiffres
builder.Services.AddScoped<IChargeRepository, ChargeRepository>();
builder.Services.AddScoped<IChiffresService, ChiffresService>();
//Authentification
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<IHacheurMotDePasse, HacheurMotDePasse>();
builder.Services.AddScoped<IGenerateurToken, GenerateurToken>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --- Authentification JWT ---
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Emetteur"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Cle"]!)),
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAngular");

// --- Création de la base au démarrage ---
using (var scope = app.Services.CreateScope())
{
    var connectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    using var connection = connectionFactory.CreateConnection();

    var schemaSql = File.ReadAllText(Path.Combine("..", "Database", "01_schema.sql"));
    connection.Execute(schemaSql);

    var nombreUtilisateurs = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Utilisateur");
    if (nombreUtilisateurs == 0)
    {
        var seedSql = File.ReadAllText(Path.Combine("..", "Database", "02_seed.sql"));
        connection.Execute(seedSql);
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
