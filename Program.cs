using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add Context
builder.Services.AddDbContext<TicketsDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("TicketsSysem"),
    sqlServerOptionsAction: sqloption =>
    {
        sqloption.EnableRetryOnFailure();
        sqloption.MigrationsAssembly("Infrastructure");
    });
});


// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

#region Cross origins
builder.Services.AddCors(options =>
{
    string origins = configuration.GetSection("AllowedCors").Value;
    if (string.IsNullOrEmpty(origins) || origins == "*")
    {
        options.AddPolicy("AllowSpecificOrigin", builder => builder.AllowAnyOrigin()
                                                                   .AllowAnyMethod()
                                                                   .AllowAnyHeader());
    }
    else
    {
        options.AddPolicy("AllowSpecificOrigin", builder =>
        builder.AllowAnyHeader().AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(origins.Split(',', StringSplitOptions.RemoveEmptyEntries))
                                                                );
    }
});

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
