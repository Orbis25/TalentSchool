using System.Reflection;

namespace TalentSchool.Infrastructure.Seeds;

public static class SeedConfiguration
{
    public static IApplicationBuilder ApplySeedConfiguration(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var seedTypes = Assembly.GetAssembly(typeof(ISeed))?
            .GetTypes()
            .Where(t => typeof(ISeed).IsAssignableFrom(t) && t.IsInterface && !string.Equals(t.Name, nameof(ISeed), StringComparison.CurrentCultureIgnoreCase)) ?? [];

        try
        {
            foreach (var service in seedTypes)
            {
                var serviceInstance = serviceProvider.GetRequiredService(service) as ISeed;
                serviceInstance?.SeedAsync().GetAwaiter().GetResult();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while seeding the database : " + e?.Message);
        }

        return applicationBuilder;
    }
}