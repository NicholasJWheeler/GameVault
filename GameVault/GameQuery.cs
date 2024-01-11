using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameVault;
public class GameQuery
{
    [JsonProperty("count")]
    public int? GamesCount { get; set; }
    [JsonProperty("next")]
    public string? NextGamesPageURL { get; set; }
    [JsonProperty("previous")]
    public string? PreviousGamesPageURL { get; set; }
    public Game[]? results { get; set; }


}

public class Game {

    // Human-readable unique identifier
    public string? Slug { get; set; }
    [JsonProperty("name")]
    public string? GameName { get; set; }
    //In-accurate playtime count (hours)
    public int? Playtime { get; set; }
    public Platforms[]? Platforms { get; set; }
    [JsonProperty("released")]
    public string? ReleaseDate { get; set; }
    [JsonProperty("background_image")]
    public string? BackgroundImage { get; set; }
    public int? Metacritic { get; set; }
    [JsonProperty("esrb_rating")]
    public ESRBRating? ESRBRating { get; set; }
    public Genre[] Genres { get; set; }


}

public class Platforms {
    [JsonProperty("platform")]
    public PlatformType? PlatformType { get; set; }
}

public class PlatformType {
    // ID correlating to the type of platform
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
}

public class ESRBRating {
    // ID correlating to the type of rating
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Slug { get; set; }
}

public class Genre {
    // ID correlating to the type of genre
    public int? Id { get; set; }
    // Name correlating to the Genere (Ex: "Puzzle", "Action", etc.) 
    public string? Name { get; set; }
    public string? Slug { get; set; }
}
