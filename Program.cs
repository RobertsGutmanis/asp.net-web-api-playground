using System.Text;
using API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddCors((options=>{
    options.AddPolicy("DevCors", (corsBuilder)=>{
        corsBuilder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });

    options.AddPolicy("ProdCors", (corsBuilder)=>{
        corsBuilder.WithOrigins("https://myproductionsite.com")
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options=>{
        options.TokenValidationParameters = new TokenValidationParameters(){
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:TokenKey").Value
            )),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

var app = builder.Build();

app.UseCors("DevCors");
app.UseCors("ProdCors");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
