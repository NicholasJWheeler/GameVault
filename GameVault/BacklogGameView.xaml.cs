using System;
namespace GameVault;

public partial class BacklogGameView : ContentPage {

    public Backlog.BacklogItem thisItem;
    public string passName, passImageSource, passRelease, passESRBRating, passPlatforms, passGenres;
    public int passMetacritic, passPlaytime;
    public BacklogGameView(Backlog.BacklogItem backlogItem) {
		InitializeComponent();
        LoadGameInfo(backlogItem);
        thisItem = backlogItem;
    }

    public void LoadGameInfo(Backlog.BacklogItem backlogItem) {
        GameNameHelper(backlogItem);
        GameImageHelper(backlogItem);
        GameReleaseHelper(backlogItem);
        GameESRBHelper(backlogItem);
        GamePlatformsHelper(backlogItem);
        GamePlaytimeHelper(backlogItem);
        GameMetacriticHelper(backlogItem);
        GameGenresHelper(backlogItem);
    }

    // Helper class to properly display the name of the current game
    private void GameNameHelper(Backlog.BacklogItem backlogItem) {
        string? gameName = backlogItem.BacklogGameTitle;
        if (gameName != null)
        {
            int? nameLength = gameName.Length;
            if (nameLength > 18)
            { // Longer name
                passName = gameName;
                gameNameLabel.Text = gameName;
                gameNameLabel.FontSize = 16;
            }
            else if (nameLength > 0 && gameName != "" && gameName != null)
            {
                passName = gameName;
                gameNameLabel.Text = gameName;
            }
            else
            {
                passName = "Game name currently unavailable";
                gameNameLabel.Text = "Game name currently unavailable";
                gameNameLabel.FontSize = 18;
            }
        }
        else
        {
            passName = "Game name currently unavailable";
            gameNameLabel.Text = "Game name currently unavailable";
            gameNameLabel.FontSize = 18;
        }
    }

    // Helper class to properly display the image associated with the current game
    private void GameImageHelper(Backlog.BacklogItem backlogItem) {
        string? gameImageURL = backlogItem.BacklogListImageSource;
        if (gameImageURL == null || gameImageURL == "") { // Invalid Picture
            passImageSource = "image_unavailable.png";
            currentGameImage.Source = "image_unavailable.png";
        } else {
            passImageSource = gameImageURL;
            currentGameImage.Source = gameImageURL;
        }
    }

    // Helper class to properly display the release date associated with the current game
    private void GameReleaseHelper(Backlog.BacklogItem backlogItem)
    {
        string? gameReleaseDate = backlogItem.BacklogListRelease;
        if (gameReleaseDate == null || gameReleaseDate == "") { // Invalid Date
            passRelease = "N/A";
            releaseDateLabel.Text = "Release Date: N/A";
        } else {
            passRelease = gameReleaseDate;
            releaseDateLabel.Text = $"Release Date: {gameReleaseDate}";
        }
    }

    // Helper class to properly display the ESRB rating associated with the current game
    private void GameESRBHelper(Backlog.BacklogItem backlogItem) {
        if (backlogItem.BacklogListESRBRating != null) { // Make sure something can be read
            string? gameESRBRating = backlogItem.BacklogListESRBRating;
            if (gameESRBRating != null && gameESRBRating != "")
            {
                passESRBRating = gameESRBRating;
                esrbLabel.Text = $"ESRB Rating: {gameESRBRating}";
            }
            else
            { // Invalid rating
                passESRBRating = "N/A";
                esrbLabel.Text = "ESRB Rating: N/A";
            }
        }
        else
        { // Invalid rating
            passESRBRating = "N/A";
            esrbLabel.Text = "ESRB Rating: N/A";
        }
    }

    // Helper class to properly display the platforms the current game is on
    private void GamePlatformsHelper(Backlog.BacklogItem backlogItem) {
        string? formattedPlatforms = backlogItem.BacklogListPlatforms;
        if (formattedPlatforms != null && formattedPlatforms.Length > 0) { // Ensure there are suitable platforms
            passPlatforms = formattedPlatforms;
            if (formattedPlatforms.Length >= 50)
            { // Large amount of text
                platformsLabel.FontSize = 12;
                platformsLabel.Text = formattedPlatforms;
            }
            else if (formattedPlatforms.Length >= 34)
            { // Medium Amount of Text
                platformsLabel.FontSize = 15;
                platformsLabel.Text = formattedPlatforms;
            }
            else
            {
                platformsLabel.Text = formattedPlatforms;
            }
        }
        else
        { // Invalid platforms
            passPlatforms = "Platforms: N/A";
            platformsLabel.Text = "Platforms: N/A";
        }
    }

    // Helper class to properly display the average playtime associated with the current game
    private void GamePlaytimeHelper(Backlog.BacklogItem backlogItem) {
        int? gamePlaytime = backlogItem.BacklogListPlaytime;
        if (gamePlaytime != null && gamePlaytime > -1) {
            passPlaytime = (int)gamePlaytime;
            playtimeLabel.Text = $"Playtime (may be inaccurate): {gamePlaytime} hours";
        } else { // Wrong playtime, 0 hours placeholder
            passPlaytime = 0;
            playtimeLabel.Text = $"Playtime (may be inaccurate): 0 hours";
        }
    }

    // Helper class to properly display the Metacritic score awarded to the current game
    private void GameMetacriticHelper(Backlog.BacklogItem backlogItem) {
        int? gameMetacriticScore = backlogItem.BacklogListMetacritic;
        if (gameMetacriticScore != null && gameMetacriticScore > -1) {
            passMetacritic = (int)gameMetacriticScore;
            metacriticLabel.Text = $"Metacritic Score: {gameMetacriticScore}/100";
        } else { // Invalid Score
            passMetacritic = 0;
            metacriticLabel.Text = "Metacritic Score: N/A";
        }
    }

    // Helper class to properly display the genres associated with the current game
    private void GameGenresHelper(Backlog.BacklogItem backlogItem) {
        string? formattedGenres = backlogItem.BacklogListGenres;
        if (formattedGenres != null) { // Ensure there are suitable genres
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

    private async void addToVaultBtn_Clicked(object sender, EventArgs e) {
        RatingPage newRatingPage = new RatingPage(passName, 
            passImageSource, passRelease, passESRBRating, passPlatforms, passGenres, passMetacritic, passPlaytime);
        await Navigation.PushModalAsync(newRatingPage, true); // A delay may/may not result here
    }

    private async void backBtn_Clicked(object sender, EventArgs e) {
        await Navigation.PopModalAsync();
    }
}