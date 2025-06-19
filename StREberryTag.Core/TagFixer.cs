using ATL;
using StREberryTag.Core.Models;

namespace StREberryTag.Core;

public static class TagFixer
{
    public static Track UpdateArtist(Track forTrack)
    {
        if (string.IsNullOrWhiteSpace(forTrack.Artist)
            && !string.IsNullOrWhiteSpace(forTrack.AlbumArtist))
            forTrack.AlbumArtist = forTrack.Artist;

        return forTrack;
    }
    
    public static Track UpdateTitle(Track forTrack, string title)
    {
        if(!string.IsNullOrWhiteSpace(forTrack.Title))
            forTrack.Title = title;

        return forTrack;
    }

    public static Track UpdateTrackNum(Track forTrack, string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return forTrack;
        
        if (!int.TryParse(number, out var trackNum))
            return forTrack;
        
        forTrack.TrackNumber    = trackNum;
        forTrack.TrackNumberStr = number;

        return forTrack;
    }

    public static Track UpdateAlbumLevelInfo(Track forTrack, AlbumInfo info)
    {
        forTrack.Comment    = info.Comment;
        forTrack.Genre      = info.Genre;
        forTrack.Copyright  = info.Copyright;
        forTrack.Date       = info.ReleaseDate;
        forTrack.Year       = info.ReleaseDate.Year;
        forTrack.TrackTotal = info.TrackTotal;
        forTrack.DiscTotal  = info.DiscTotal;

        return forTrack;
    }

    public static Track UpdateCoverEmbed(Track forTrack, string coverPath)
    {
        if (string.IsNullOrWhiteSpace(coverPath))
            return forTrack;
        
        var picInfo = PictureInfo.fromBinaryData(File.ReadAllBytes(coverPath), PictureInfo.PIC_TYPE.CD);
        forTrack.EmbeddedPictures.Add(picInfo);

        return forTrack;
    }
}
