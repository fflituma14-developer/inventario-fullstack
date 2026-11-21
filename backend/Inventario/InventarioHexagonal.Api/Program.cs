using Serilog;
using System.Text;
using Microsoft.EntityFrameworkCore;
using InventarioHexagonal.Infrastructure.Data;
using InventarioHexagonal.Application.Interfaces;
using InventarioHexagonal.Infrastructure.Repositories;
using InventarioHexagonal.Application.Services;
using InventarioHexagonal.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Configuración de SQL Server
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se configuró la conexión a SQL Server.");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(connectionString)
);

// 3. Registro de repositorios y servicios

// Productos
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ProductoService>();

// Proveedores
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<ProveedorService>();

// Lotes
builder.Services.AddScoped<ILoteProductoRepository, LoteProductoRepository>();
builder.Services.AddScoped<LoteProductoService>();

// Autenticación
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<AuthService>();

// 4. Configuración JWT
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException(
        "Falta configurar Jwt:Secret.");

if (Encoding.UTF8.GetByteCount(jwtSecret) < 32)
{
    throw new InvalidOperationException(
        "La clave JWT debe tener al menos 32 bytes.");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "Falta configurar Jwt:Issuer.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "Falta configurar Jwt:Audience.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSecret)),

                ValidAlgorithms = new[]
                {
                    SecurityAlgorithms.HmacSha256
                }
            };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();