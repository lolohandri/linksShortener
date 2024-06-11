using LinkAPI.Context;
using LinkAPI.Interfaces;
using LinkAPI.Models;
using LinkAPI.Repository;
using LinkAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.
                GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition(
    "token",
    new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization
    });
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "token"
                    },
                },
                Array.Empty<string>()
            }
        }
    );
});
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("LinksDb")));
builder.Services.AddScoped<IRepository<Link>, LinkRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IUrlShortener, UrlShortener>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("default", (options) =>
    {
        options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("https://localhost:3000");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapFallback((DataContext context, HttpContext ctx) =>
{
    var path = $"{ctx.Request.Scheme}://{ctx.Request.Host}/{ctx.Request.Path.ToUriComponent().Trim('/')}";
    var urlMatch = context.Links.FirstOrDefault(link =>
        link.ShortLink!.Trim() == path.Trim());
    
    return urlMatch == null ? Results.BadRequest("Invalid url") : Results.Redirect(urlMatch.OriginLink);
});

app.Run();