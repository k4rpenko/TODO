using Applications;
using Applications.Validators;
using Core.Card;
using Core.Hash;
using Core.Hesh;
using Core.Models.Request;
using Core.Redis;
using Core.User;
using DALPostgresSQL;
using DALRedis;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hash;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetSection("Npgsql:ConnectionString").Value));

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserLoginValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationValidator>();

builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<ICards, CardService>();
builder.Services.AddScoped<ICardItems, CardItemService>();
builder.Services.AddScoped<IArgon2Hasher, Argon2Hasher>();
builder.Services.AddScoped<IJWT, JwtService>();
builder.Services.AddSingleton<AppRedisContext>();
builder.Services.AddScoped<IRedisService, RedisService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["access_token"];
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
