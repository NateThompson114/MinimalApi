namespace MinimalApi.Service;

public record Person(string FullName);

public class PeopleService
{
    private readonly List<Person> _people = new()
    {
        new("Nate Thompson"), new("Misty Smith"), new ("Justin Thompson")
    };

    public IEnumerable<Person> Search(string searchTerm) =>
        _people.Where(p => p.FullName.Contains(searchTerm, StringComparison.Ordinal));
}