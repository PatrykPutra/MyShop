
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AutoMapper;
using MyShop.Data;
using MyShop.Services;
using MyShop.Client;
using Microsoft.AspNetCore.Identity;
using MyShop.Middleware;
using static MyShop.AuthentitactionSettings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using NLog.Web;
using MyShop.Entities;
using MyShop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseNLog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyShop",
        Version = "v1"
    });

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Provide JWT token."
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var authentitactionSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authentitactionSettings);
builder.Services.AddSingleton(authentitactionSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authentitactionSettings.JwtIssuer,
        ValidAudience = authentitactionSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authentitactionSettings.JwtKey)),
    };
});

builder.Services.AddDbContext<MyShopDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("localdb")));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IItemCategoryServices, ItemCategoryServices>();
builder.Services.AddScoped<IExchangeRatesServices, ExchangeRatesServices>();
builder.Services.AddScoped<IShopItemServices, ShopItemServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<ISummaryServices, SummaryServices>();
builder.Services.AddScoped<ICurrencyExchangeRatesClient, CurrencyExchangeRatesClient>();
builder.Services.AddScoped<IShoppingCartServices, ShoppingCartServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<MyShopSeeder>();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<MyShopSeeder>(); 
seeder.Seed();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


app.Run();



