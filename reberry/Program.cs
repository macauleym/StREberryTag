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

    static RootCommand BuildCommand()
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
                description: "Fixes tags in audio files to allow Strawberry to understand them naturally."))
            .WithArgument(pathArgument)
            .WithOption(commentOption)
            .WithOption(genreOption)
            .WithOption(copyrightOption)
            .WithOption(releasedOption)
            .WithOption(trackTotalOption)
            .WithOption(discTotalOption)
            .WithOption(coverOption)
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
        await BuildCommand()
            .InvokeAsync(args);
}
