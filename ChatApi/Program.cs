using ChatApi.Services.DataBase;
using ChatApi.Services.RegisterServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddScoped<IRegistrServices, RegistrServices>();
builder.Services.AddSingleton<JwtService>();

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
    app.MapOpenApi();
}



app.UseHttpsRedirection();


app.Run();

