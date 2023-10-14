var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = builder.Configuration.ConfigureSettings();
var configuration = configurationBuilder.Build();

var serviceCollection = builder.Services;
serviceCollection.RegisterDependencies(configuration);

var app = builder.Build();
app.Initialize();