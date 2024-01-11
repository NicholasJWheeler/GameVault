using System;
namespace GameVault;

public partial class GameView : ContentPage {

    // Define a custom event to notify the Backlog when a new backlog game is added
    public event EventHandler BacklogGameAdded;

    public Game thisGame;
    public string passName, passImageSource, passRelease, passESRBRating, passPlatforms, passGenres;
    public int passMetacritic, passPlaytime;
	public GameView(Game curGame) {
		InitializeComponent();
        LoadGameInfo(curGame);
        thisGame = curGame;
	}

    public void LoadGameInfo(Game game) {
		GameNameHelper(game);
		GameImageHelper(game);
        GameReleaseHelper(game);
        GameESRBHelper(game);
        GamePlatformsHelper(game);
        GamePlaytimeHelper(game);
        GameMetacriticHelper(game);
        GameGenresHelper(game);
    }

    // Helper class to properly display the name of the current game
    private void GameNameHelper(Game game) {
		string? gameName = game.GameName;
        if (gameName != null) {
            int? nameLength = gameName.Length;
            if (nameLength > 18) { // Longer name
                passName = gameName;
                gameNameLabel.Text = gameName;
                gameNameLabel.FontSize = 16;
            } else if (nameLength > 0 && gameName != "" && gameName != null) {
                passName = gameName;
                gameNameLabel.Text = gameName;
            }
            else {
                passName = "Game name currently unavailable";
                gameNameLabel.Text = "Game name currently unavailable";
                gameNameLabel.FontSize = 18;
            }
        } else {
            passName = "Game name currently unavailable";
            gameNameLabel.Text = "Game name currently unavailable";
            gameNameLabel.FontSize = 18;
        }
    }

    // Helper class to properly display the image associated with the current game
    private void GameImageHelper(Game game){
		string? gameImageURL = game.BackgroundImage;
		if (gameImageURL == null || gameImageURL == "") { // Invalid Picture
            passImageSource = "image_unavailable.png";
            currentGameImage.Source = "image_unavailable.png";
        } else {
            passImageSource = gameImageURL;
            currentGameImage.Source = gameImageURL;
        }
    }

    // Helper class to properly display the release date associated with the current game
    private void GameReleaseHelper(Game game) {
        string? gameReleaseDate = game.ReleaseDate;
        if (gameReleaseDate == null || gameReleaseDate == "") { // Invalid Date
            passRelease = "N/A";
            releaseDateLabel.Text = "Release Date: N/A";
        } else {
            string formattedDate = FormatDate(gameReleaseDate);
            passRelease = formattedDate;
            releaseDateLabel.Text = $"Release Date: {formattedDate}";
        }
    }

    // Helper class to properly display the ESRB rating associated with the current game
    private void GameESRBHelper(Game game) {
        if (game.ESRBRating != null) { // Make sure something can be read
            string? gameESRBRating = game.ESRBRating.Name;
            if (gameESRBRating != null && gameESRBRating != "")
            {
                passESRBRating = gameESRBRating;
                esrbLabel.Text = $"ESRB Rating: {gameESRBRating}";
            } else { // Invalid rating
                passESRBRating = "N/A";
                esrbLabel.Text = "ESRB Rating: N/A";
            }
        } else { // Invalid rating
            passESRBRating = "N/A";
            esrbLabel.Text = "ESRB Rating: N/A";
        }
    }

    // Helper class to properly display the platforms the current game is on
    private void GamePlatformsHelper(Game game) {
        string formattedPlatforms = "Platforms: ";
        Platforms[]? platforms = game.Platforms;
        if (platforms != null && platforms.Length > 0) { // Ensure there are suitable platforms
            foreach (Platforms p in platforms) {
                if (p != null) {
                    string platformName = p.PlatformType.Name;
                    formattedPlatforms = formattedPlatforms + platformName + ", ";
                }
            }
            formattedPlatforms = formattedPlatforms.Substring(0, formattedPlatforms.Length - 2);
            passPlatforms = formattedPlatforms;
            if (formattedPlatforms.Length >= 50) { // Large amount of text
                platformsLabel.FontSize = 12;
                platformsLabel.Text = formattedPlatforms;
            } else if (formattedPlatforms.Length >= 34) { // Medium Amount of Text
                platformsLabel.FontSize = 15;
                platformsLabel.Text = formattedPlatforms;
            } else {
                platformsLabel.Text = formattedPlatforms;
            }
        } else { // Invalid platforms
            passPlatforms = "Platforms: N/A";
            platformsLabel.Text = "Platforms: N/A";
        }
    }

    // Helper class to properly display the average playtime associated with the current game
    private void GamePlaytimeHelper(Game game) {
        int? gamePlaytime = game.Playtime;
        if (gamePlaytime != null && gamePlaytime > -1) {
            passPlaytime = (int)gamePlaytime;
            playtimeLabel.Text = $"Playtime (may be inaccurate): {gamePlaytime} hours";
        } else { // Wrong playtime, 0 hours placeholder
            passPlaytime = 0;
            playtimeLabel.Text = $"Playtime (may be inaccurate): 0 hours";
        }
    }

    // Helper class to properly display the Metacritic score awarded to the current game
    private void GameMetacriticHelper(Game game) {
        int? gameMetacriticScore = game.Metacritic;
        if (gameMetacriticScore != null && gameMetacriticScore > -1) {
            passMetacritic = (int)gameMetacriticScore;
            metacriticLabel.Text = $"Metacritic Score: {gameMetacriticScore}/100";
        } else { // Invalid Score
            passMetacritic = 0;
            metacriticLabel.Text = "Metacritic Score: N/A";
        }
    }

    // Helper class to properly display the genres associated with the current game
    private void GameGenresHelper(Game game) {
        string formattedGenres = "Genres: ";
        Genre[]? genres = game.Genres;
        if (genres != null && genres.Length > 0) { // Ensure there are suitable genres
            foreach (Genre g in genres) {
                if (g != null) {
                    string? genreName = g.Name;
                    formattedGenres = formattedGenres + genreName + ", ";
                }
            }
            formattedGenres = formattedGenres.Substring(0, formattedGenres.Length - 2);
            passGenres = formattedGenres;
            if (formattedGenres.Length >= 50) { // Large amount of text
                genresLabel.FontSize = 12; 
            } else if (formattedGenres.Length >= 34) { // Medium Amount of Text
                genresLabel.FontSize = 15;
            }
            genresLabel.Text = formattedGenres;
        } else { // Invalid genres
            passGenres = "Genres: N/A";
            genresLabel.Text = "Genres: N/A";
        }
    }

    static string FormatDate(string inputDate) {
        // Parse the input string to a DateTime object
        if (DateTime.TryParse(inputDate, out DateTime date))
        {
            // Format the DateTime object as "Month Day, Year"
            string formattedDate = date.ToString("MMMM d, yyyy");
            return formattedDate;
        }
        else
        {
            // Handle invalid input date
            return "Invalid date format";
        }
    }

    private async void addToVaultBtn_Clicked(object sender, EventArgs e) {
        RatingPage newRatingPage = new RatingPage(passName,
            passImageSource, passRelease, passESRBRating, passPlatforms, passGenres, passMetacritic, passPlaytime);
        await Navigation.PushModalAsync(newRatingPage, true); // A delay may/may not result here
    }

    private async void addToBacklogBtn_Clicked(object sender, EventArgs e) {
        // Create a Backlog entry
        BacklogGame backlogGame = new BacklogGame { Name = passName, ImageSource = passImageSource,
            Release = passRelease, Metacritic = passMetacritic, ESRBRating = passESRBRating,
            Platforms = passPlatforms, Genres = passGenres, Playtime = passPlaytime };

        // Add to Backlog Database
        DB.backlogConn.Insert(backlogGame);

        // Trigger the BacklogGameAdded event
        BacklogGameAdded?.Invoke(this, EventArgs.Empty);

        // Update the status in SharedViewModel
        App.SharedVMBacklog.Status = "New Status";

        // Trigger the UpdateUI event
        App.SharedVMBacklog.OnSomethingChanged();

        // Exit page
        await Navigation.PopModalAsync();
    }

    private async void backBtn_Clicked(object sender, EventArgs e) {
        await Navigation.PopModalAsync();
    }
}