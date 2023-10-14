using MediatR;
using Microsoft.EntityFrameworkCore;
using TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

namespace Microsoft.Extensions.Configuration;

public static class ServiceCollectionExtension
{
    public static void RegisterDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.ConfigureControllers();

        serviceCollection.AddMediatR(typeof(CreateTeamsCommand).Assembly);

        serviceCollection.RegisterDbContexts(configuration);
    }

    private static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddRouting(options => options.LowercaseUrls = true);
    }

    public static void RegisterDbContexts(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ITournamentBracketDbContext, TournamentBracketDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(ConnectionStringKeys.TournamentBracketDb));
        });
    }


}

