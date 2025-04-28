using ChatApi.DTO.Options;
using ChatApi.Services.ConvertingData;
using ChatApi.Services.DataBase;
using ChatApi.Services.FileManagment;
using ChatApi.Services.RegisterServices.Interface;
using ChatApi.Services.RegisterServices.Realization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ����������� ��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllClient", policy =>
    {
        policy
          .WithOrigins("https://localhost:7191") // ���� � URL ������ Blazor WASM
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

// ����������� �������� �������������� � ����������� (���� � ��� � ������������ ���� [Authorize])
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// �������� ��������� �������� � ����� �������!

app.UseHttpsRedirection();

app.UseRouting(); // << �� UseCors

app.UseCors("AllowAllClient"); // << ����� ����� UseRouting

app.UseAuthentication(); // << ������ ���� �� Authorization
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// �������� ��
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

app.Run();
