using Backend.WebApi.Configuration;
using Backend.WebApi.MappingProfiles;
using Backend.WebApi.Infrastructure;
using Backend.WebApi.Infrastructure.Hubs;
using Backend.WebApi.Infrastructure.Persistence;
using Backend.WebApi.Repositories;
using Backend.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddAutoMapper(typeof(SensorInfoMapping).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SensorServiceContext>();
builder.Services.AddDbContext<IdentityServiceContext>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ISensorInfoRepository, SensorInfoRepository>();

builder.Services.AddMqttHostedServiceOptions(builder.Configuration);
builder.Services.AddConnectionStrings(builder.Configuration);

builder.Services.AddHostedService<MqttHostedService>();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader()
            .AllowAnyMethod().AllowCredentials();
    });
});

// JWT Authentication
JwtSettings jwtSettings = new JwtSettings(configuration.GetRequiredSection("Jwt:Key").Value ?? throw new InvalidOperationException("Jwt:Key not configured"));
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.KeyBytes),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        x.Events= new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/scaleSensorHub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityServiceContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;    
        return Task.CompletedTask;
    };
});

WebApplication? app = builder.Build();

InitializeDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ScaleSensorHub>("/scaleSensorHub");

app.Run();

void InitializeDatabase(IHost app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var sensorContext = services.GetRequiredService<SensorServiceContext>();
    sensorContext.Database.Migrate();

    var identityContext = services.GetRequiredService<IdentityServiceContext>();
    identityContext.Database.Migrate();
}