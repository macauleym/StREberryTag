namespace reberry;

public record UpdateRequest(
  string Path
, string Comment
, string Genre
, string Copyright
, int Released
, int TrackTotal
, int DiscTotal
, string Cover
);