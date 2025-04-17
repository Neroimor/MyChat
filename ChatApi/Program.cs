using ChatApi.Services.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddLogging();

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

