using System.Diagnostics;
using StREberryTag.Core.Interfaces;

namespace StREberryTag.Core;

public class Bash : IExecuteCommand
{
    public async Task<string> ItAsync(string command)
    {
        var p = new ProcessStartInfo 
        { FileName               = "/bin/bash"
        , Arguments              = command
        , RedirectStandardOutput = true
        , UseShellExecute        = false
        , CreateNoWindow         = true
        };

        using var process = Process.Start(p);
        await process.WaitForExitAsync();
        var output = await process.StandardOutput.ReadToEndAsync();

        return output;
    }
}
