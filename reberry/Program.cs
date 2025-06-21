using System.CommandLine;
using ATL;
using reberry;
using StREberryTag.Core;
using StREberryTag.Core.Data;
using StREberryTag.Core.Models;

class Program
{
    static async Task UpdateAlbumTags(UpdateRequest request)
    {
        if (!Path.Exists(request.Path))
            return;
        
        var tracks = Directory
            .GetFiles(request.Path)
            .Where(f => f.EndsWith(".wav"))
            .Select(p => new Track(p));

        var albumInfo = request.ToAlbumInfo();
        await Parallel.ForEachAsync(tracks, (track, token) =>
        {
            if (token.IsCancellationRequested)
                return ValueTask.FromCanceled(token);
            
            var updated = TagFixer.UpdateArtist(track);
            var possiblyParsed = AudioData.ParseExistingTitle(updated.Title);
            if (possiblyParsed is Just<(string, string)> just)
            {
                updated = TagFixer.UpdateTrackNum(updated, just.Value.Item1);
                updated = TagFixer.UpdateTitle(updated, just.Value.Item2);
            }

            updated = TagFixer.UpdateAlbumLevelInfo(updated, albumInfo);
            updated = TagFixer.UpdateCoverEmbed(updated, albumInfo.CoverPath);

            updated.Save();
            
            return ValueTask.CompletedTask;
        });
    }

    static async Task CopyTags(CopyRequest request)
    {
        if (!Path.Exists(request.FolderPath)
        || !Directory.Exists(request.FolderPath))
            return;

        var directoryName = new DirectoryInfo(request.FolderPath).Name;
        var wavFiles = Directory
            .GetFiles(request.FolderPath)
            .Where(f => f.EndsWith(".wav"));

        var zshing = new Zsh();
        await Parallel.ForEachAsync(wavFiles, (wavFile, cancellationToken) =>
        {
            if (cancellationToken.IsCancellationRequested)
                return ValueTask.FromCanceled(cancellationToken);

            var flacFile = wavFile.Replace("wav", "flac");
            if (!Path.Exists(flacFile))
                return ValueTask.CompletedTask;

            var wavTrack = new Track(wavFile);
            var flacTrack = new Track(flacFile);
            wavTrack.CopyMetadataTo(flacTrack);
            
            var desiredCoverPath = string.IsNullOrWhiteSpace(request.CoverPath)
                ? Path.Combine(request.FolderPath, directoryName + ".jpeg")
                : request.CoverPath;
            TagFixer.UpdateCoverEmbed(flacTrack, desiredCoverPath)
                .Save();
            
            Console.WriteLine($"Copied {wavFile} to {flacFile}.");
            
            return ValueTask.CompletedTask;
        });
    }
    
    static Command BuildWavToFlacCommand()
    {
        var pathArgument = new Argument<string>(
            "FolderPath"
            , () => "."
            , "Path to look for wav & flac files"
            );
        var coverOption = new Option<string>(
              aliases: ["--cover", "-c"]
            , description: "Explicit path to use for the album cover."
            );

        return new CommandBuilder(new Command(
                "wav-to-flac"
                ))
            .WithArgument(pathArgument)
            .WithAsyncHandler(CopyTags, new CopyRequestBinder(pathArgument, coverOption))
            .Build();
    }
    
    static Command BuildRootCommand()
    {
        var pathArgument = new Argument<string>(
              "AlbumPath"
            , () => "."
            , "Path to the album with audio files to fix."
            );
        var commentOption = new Option<string>(
              aliases: ["--comment", "-m"]
            , description: "Optional comment to add."
            );
        var genreOption = new Option<string>(
              aliases: ["--genre", "-g"]
            , "Set the genre of the track(s)."
            );
        var copyrightOption = new Option<string>(
              aliases: ["--copyright", "-p"]
            , "Copyright information to add."
            );
        var releasedOption = new Option<int>(
              aliases: ["--released", "-y"]
            , "The year the album was released."
            );
        var trackTotalOption = new Option<int>(
              aliases: ["--tracks", "-t"]
            , "Total count of tracks on the album."
            );
        var discTotalOption = new Option<int>(
              aliases: ["--discs", "-d"]
            , "Total count of disks for the album."
            );
        var coverOption = new Option<string>(
              aliases: ["--cover", "-c"]
            , "Path to the album/cover art."
            );
        
        return new CommandBuilder(new RootCommand(
                description: "Fixes tags in audio files to allow Strawberry to understand them naturally."
                ))
            .WithArgument(pathArgument)
            .WithOption(commentOption)
            .WithOption(genreOption)
            .WithOption(copyrightOption)
            .WithOption(releasedOption)
            .WithOption(trackTotalOption)
            .WithOption(discTotalOption)
            .WithOption(coverOption)
            .WithSubCommand(BuildWavToFlacCommand())
            .WithAsyncHandler(UpdateAlbumTags, new UpdateRequestBinder(
                  pathArgument
                , commentOption
                , genreOption
                , copyrightOption
                , releasedOption
                , trackTotalOption
                , discTotalOption
                , coverOption
                ))
            .Build();
    }

    public static async Task<int> Main(params string[] args) =>
        await BuildRootCommand()
            .InvokeAsync(args);
}
