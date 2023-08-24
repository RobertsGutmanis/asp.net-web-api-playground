var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors((options=>{
    options.AddPolicy("DevCors", (corsBuilder)=>{
        corsBuilder.WithOrigins("https://localhost:4200")
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });

    options.AddPolicy("ProdCors", (corsBuilder)=>{
        corsBuilder.WithOrigins("https://myproductionsite.com")
        .AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
}));

var app = builder.Build();

app.UseCors("DevCors");
app.UseCors("ProdCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
