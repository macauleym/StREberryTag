using StREberryTag.Core.Models;

namespace reberry;

public static class UpdateRequestExtensions
{
    public static AlbumInfo ToAlbumInfo(this UpdateRequest source) =>
        new (
          source.Comment
        , source.Genre
        , source.Copyright
        , new DateTime(source.Released, 1, 1)
        , source.TrackTotal
        , source.DiscTotal
        , source.Cover
        );
}
