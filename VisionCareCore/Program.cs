using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using VisionCareCore.OpenAI.Application.Internal.CommandServices;
using VisionCareCore.OpenAI.Domain.Services;
using VisionCareCore.OpenAI.Infrastructure.ExternalAPIs.OpenAI;
using VisionCareCore.Shared.Domain.Repositories;
using VisionCareCore.Shared.Infraestructure.Interfaces.ASP.Configuration;
using VisionCareCore.Shared.Infraestructure.Persistences.EFC.Configuration;
using VisionCareCore.Shared.Infraestructure.Persistences.EFC.Repositories;
using VisionCareCore.User.Application.Internal.CommandServices;
using VisionCareCore.User.Application.Internal.OutboundServices;
using VisionCareCore.User.Application.Internal.QueryServices;
using VisionCareCore.User.Domain.Repositories;
using VisionCareCore.User.Domain.Services;
using VisionCareCore.User.Infraestructure.Hashing.BCrypt.Services;
using VisionCareCore.User.Infraestructure.Persistence.EFC.Repositories;
using VisionCareCore.User.Infraestructure.Tokens.JWT.Configurations;
using VisionCareCore.User.Infraestructure.Tokens.JWT.Services;
using VisionCareCore.User.Interfaces.ACL;
using VisionCareCore.User.Interfaces.ACL.Services;
using VisionCareCore.Vision.Application.Internal.CommandServices;
using VisionCareCore.Vision.Domain.Services;
using VisionCareCore.Vision.Infrastructure.ExternalAPIs.Azure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Bearer";
        options.DefaultChallengeScheme = "Bearer";
    })
    .AddJwtBearer("Bearer", options =>
    {
        var secret = builder.Configuration["TokenSettings:Secret"];
        if (string.IsNullOrEmpty(secret))
        {
            throw new ArgumentNullException(nameof(secret), "JWT Secret is not configured in appsettings.json.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };

        // 🔥 Permitir el uso de cookies para enviar el token
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("AuthToken"))
                {
                    context.Token = context.Request.Cookies["AuthToken"];
                }
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,                         
            maxRetryDelay: TimeSpan.FromSeconds(10),  
            errorNumbersToAdd: null                   
        )
    )
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
    .EnableDetailedErrors(builder.Environment.IsDevelopment());
});


builder.Services.AddControllers(options =>
{
        options.Conventions.Add(new KebabCaseRouteNamingConvention());
});

// Configurar opciones de enrutamiento
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",policy =>
    {
        policy.WithOrigins("http://localhost:8080","http://localhost:80","http://localhost:8081","http://localhost:5162","http://epam.amsac.pe","http://198.150.0.223","https://localhost", "https://epam.amsac.pe","https://198.150.0.223","https://epam.amsac.pe:80","https://epam.amsac.pe:8080","https://epam.amsac.pe:443")  
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();  
    });
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true; // 🔹 Evita acceso JS
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // 🔹 Solo HTTPS en producción
    options.Cookie.SameSite = SameSiteMode.None; // 🔹 Necesario para Swagger y CORS
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VisionCareCore API",
        Version = "v1",
        Description = "VisionCareCore API",
        TermsOfService = new Uri("http://localhost:5000/swagger/index.html"),
        Contact = new OpenApiContact
        {
            Name = "Bruno",
            Email = "brunomoisespalomino@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });

    // 🔹 Definir el esquema de autenticación JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT con el prefijo 'Bearer '",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IAuthUserRepository, AuthUserRepository>();
builder.Services.AddScoped<IAuthUserRefreshTokenRepository, AuthUserRefreshTokenRepository>();

builder.Services.AddScoped<IAuthUserCommandService, AuthUserCommandService>();
builder.Services.AddScoped<IAuthUserQueryService, AuthUserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

builder.Services.AddScoped<IGptClient, GptClient>();
builder.Services.AddScoped<IGptService, GptCommandService>();

builder.Services.AddScoped<IVisionService, VisionCommandService>();
builder.Services.AddScoped<IVisionClient, VisionClient>();

var app = builder.Build();

// Log Server Addresses
var serverAddresses = app.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();


foreach (var address in serverAddresses.Addresses)
{
    Console.WriteLine($"Listening on {address}");
}

// Ensure Database is Created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SupportedSubmitMethods(new[] { SubmitMethod.Get, SubmitMethod.Post });
        c.ConfigObject.AdditionalItems["withCredentials"] = true; // 🔹 Habilita envío de cookies
    });}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
