using BookStore.Common.Utilities;
using BookStore.Persistence.Extensions;
using BookStoreApp.Configuration;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
// Add services to the container.

try
{
    builder.Services.AddControllers();
    builder.Services.ConfigureAuthentication(configuration);
    builder.Services.AddDependencies(configuration);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        await Seeder.SeedRolesAndAdmin(serviceProvider);
    }

     

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{

    logger.Error(ex, "Something is not right here");
}
finally
{
    NLog.LogManager.Shutdown();
}
