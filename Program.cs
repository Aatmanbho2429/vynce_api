using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using vynce_api.DataProvider;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
ApplicationConfigurations.ConnectionString = builder.Configuration.GetSection("ApiOptions:SqlConnection").Value;

#region dependency injection
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IDataProviderHelper, DataProviderHelper>();
builder.Services.AddTransient<IMemberDataProvider, MemberDataProvider>();
builder.Services.AddTransient<ILoginDataProvider, LoginDataProvider>();
builder.Services.AddTransient<IJwtTokenDataProvider, JwtTokenDataProvider>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
#endregion

var hosts = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<List<string>>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders", builder => builder
     .AllowAnyMethod()
     .AllowAnyHeader()
     .WithOrigins(hosts.ToArray())
     .AllowCredentials());
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "C-Pesa API",
        Description = "C-Pesa Windows Portal API to support C-Pesa Windows portal"
    });

    //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    //{
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.ApiKey,
    //    Scheme = "Bearer",
    //    BearerFormat = "JWT",
    //    In = ParameterLocation.Header,
    //    Description = "JWT Authorization header using the Bearer scheme."

    //});
    //options.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[] {}
    //    }
    //});
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("defaultToken", options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = false,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>(),
//        ValidAudience = builder.Configuration.GetSection("Jwt:Issuer").Get<string>(),
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Get<string>()))
//    };
//});

//builder.Services.AddAuthorization(options =>
//{
//    options.DefaultPolicy = new AuthorizationPolicyBuilder()
//            .RequireAuthenticatedUser()
//            .AddAuthenticationSchemes("defaultToken")
//            .Build();

//    options.AddPolicy("registerTokenPolicy", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.AddAuthenticationSchemes("registrationToken");
//        policy.Build();
//        policy.RequireClaim(ClaimTypes.NameIdentifier, "true");
//    });
//});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllHeaders");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
