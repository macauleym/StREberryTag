using System.Text.RegularExpressions;
using StREberryTag.Core.Models;

namespace StREberryTag.Core.Data;

public static class AudioData
{
    public static Maybe<(string track, string title)> ParseExistingTitle(string title)
    {
        var titlePattern = new Regex(@"^(?<number>\d{1,3})\s-\s(?<name>.*)$");
        if (!titlePattern.IsMatch(title))
            return new None<(string track, string title)>();
        
        var titleMatch   = titlePattern.Match(title);
        var number = titleMatch.Groups["number"].Value;
        var name         = titleMatch.Groups["name"].Value;

        return new Just<(string track, string title)>((number, name));
    }
}
