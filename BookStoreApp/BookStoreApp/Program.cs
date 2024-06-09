using BookStore.Common.Utilities;
using BookStoreApp.Configuration;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
// Add services to the container.

try
{
    logger.Debug("Init main");

    builder.Services.AddControllers();
    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();
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
        app.UseDeveloperExceptionPage();
    }

    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        await Seeder.SeedRoles(serviceProvider);
    }

     

    app.UseHttpsRedirection();
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{

    logger.Error(ex.Message, "Something is not right here");
}
finally
{
    NLog.LogManager.Shutdown();
}
