namespace TalentSchool.Infrastructure.Seeds;

public interface ISeed
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}