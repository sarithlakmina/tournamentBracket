namespace Microsoft.Extensions.Configuration;

public static class ConfigurationBuilderExtension
{
    public static IConfigurationBuilder ConfigureSettings(this ConfigurationManager configurationBuilder)
        => configurationBuilder
            .AddJsonFile("appsettings.json", false, true)
            .AddUserSecrets<Program>(true)
            .AddEnvironmentVariables();
}
