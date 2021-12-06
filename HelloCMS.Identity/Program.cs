using AutoMapper;
using HelloCMS.Identity.Data;
using HelloCMS.Identity.Data.Models;
using HelloCMS.Identity.Infrastructure.Automapper;
using HelloCMS.Identity.Infrastructure.ServiceRegistration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    //Automapper
    // Auto Mapper Configurations
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MappingProfile());
    });

    IMapper mapper = mapperConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);

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
    builder.Services.AddServices(builder.Configuration);
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
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    app.UseHttpLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    //Seed the database
    AppDbInitializer.SeedRolesToDb(app).Wait();

    app.Run();

}