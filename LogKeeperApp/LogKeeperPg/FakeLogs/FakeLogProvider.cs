using Bogus;
using LogKeeperPg.Models;

namespace LogKeeperPg.FakeLogs;

public class FakeLogProvider
{
    public List<LogInformation> GenerateLog (int count)
    {
        var faker = new Faker<LogInformation>()
            .RuleFor(o => o.Uuid, f => f.Random.Guid())
            .RuleFor(o => o.Author, f => f.Name.FullName())
            .RuleFor(o => o.Title, f => f.Lorem.Word())
            .RuleFor(o => o.Project, f => f.Lorem.Word())
            .RuleFor(o => o.Time, f => f.Date.Past().ToUniversalTime())
            .RuleFor(o => o.Contents, f => f.Lorem.Paragraphs(30));

        return faker.Generate(count);
    }
}