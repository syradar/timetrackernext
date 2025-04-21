using System.Text.RegularExpressions;

namespace TimeTracking.Application.Models;

public partial class Client
{
    public required Guid Id { get; init; }

    public required string Name { get; set; }

    public string Slug => GenerateSlug();

    private string GenerateSlug()
    {
        var sluggedName = SlugNameRegex().Replace(Name, string.Empty)
            .ToLower()
            .Replace(" ", "-")
            .Trim();
        return sluggedName;
    }

    [GeneratedRegex("[^a-zA-Z0-9 _-]+", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugNameRegex();
}