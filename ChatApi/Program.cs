using ChatApi.DTO.Options;
using ChatApi.Services.ConvertingData;
using ChatApi.Services.DataBase;
using ChatApi.Services.FileManagment;
using ChatApi.Services.RegisterServices.Interface;
using ChatApi.Services.RegisterServices.Realization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7191")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.Configure<AvatarOptions>(builder.Configuration.GetSection("AvatarOptions"));

builder.Services.AddScoped<IRegistrServices, RegistrServices>();
builder.Services.AddScoped<IFileManagement, FileManagement>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddTransient<Argon2PasswordHasher>();
builder.Services.AddTransient<IConvertingImage, ConvertingImage>();
builder.Services.AddTransient<IGenerateCode, GeneratorCode>();




var connectingStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(connectingStr));


var app = builder.Build();

var logging = app.Services.GetRequiredService<ILogger<Program>>();
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
        dbContext.Database.Migrate();
        logging.LogInformation("Added migration");
    }
    catch (Exception e)
    {
        logging.LogError(e, "\n\nAn error occurred while migrating the database");
    }

}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}



app.UseHttpsRedirection();


app.Run();

