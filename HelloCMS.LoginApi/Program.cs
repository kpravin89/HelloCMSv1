using HelloCMS.LoginApi.Data;
using HelloCMS.LoginApi.Data.Models;
using HelloCMS.LoginApi.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

Configure();

void ConfigureServices()
{

    //Setup Database
    var AppConnstr = builder.Configuration.GetConnectionString("CMSConnection");

    if (builder.Environment.IsDevelopment())
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase(AppConnstr));
    else
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(AppConnstr));

    //Add Identity
    builder.Services.AddIdentity<AppIdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    //Token Validation Parameter

    var tokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    builder.Services.AddSingleton(tokenValidationParameters);

    //Add Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    //Add JWT Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = tokenValidationParameters;
    });


    //Add Managers and Controllers
    builder.Services.AddManagers(builder.Configuration);
    builder.Services.AddControllers();

    //Add Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

}

void Configure()
{

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    //Seed the database
    AppDbInitializer.SeedRolesToDb(app).Wait();

    app.Run();

}