using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using Plugin.Maui.Audio;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Timers;
using System.Runtime.CompilerServices;

namespace GameVault;

public partial class Trending : ContentPage {

    // Audio Connection
    private readonly IAudioManager audioManager;
    private IAudioPlayer player;

    // API Connection
    private static readonly string RAWGApiEndpoint = "https://api.rawg.io/api/";
    private static readonly string RAWGApiKEY = "XXXXXXXXXXXXXXX"; // Hidden for privacy
    private readonly HttpClient client = new HttpClient();

    public Trending(IAudioManager audioManager)
    {
        InitializeComponent();
        this.audioManager = audioManager;
        DB.OpenConnection();
        LoadPopularGamesAsync();
        bestGenreGamesLabel.Text = $"Best {Preferences.Get("GenrePref", "Action")} Games";
        LoadGenreGamesAsync();
        StartBackgroundMusic();

        // Subscribe to the UpdateUI event
        App.SharedVM.UpdateUI += OnUpdateUI;
    }

    ~Trending() {
        player?.Dispose();
    }

    /*protected override void OnDisappearing() {
        base.OnDisappearing();
        if (player != null ) {
            player.Pause();
        }
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        if (player != null) {
            player.Play();
        }
    }*/

    private async void StartBackgroundMusic() {
        if (player == null) {
            player = audioManager.CreatePlayer(await Microsoft.Maui.Storage.FileSystem.OpenAppPackageFileAsync("resonant_victory_bgm.wav"));
        }
        player.Loop = true;
        player.Play();
    }

    // Helper method to let the app know when to update the Trending page
    private void OnUpdateUI(object sender, EventArgs e)
    {
        bestGenreGamesLabel.Text = $"Best {Preferences.Get("GenrePref", "Action")} Games";
        LoadGenreGamesAsync();
    }

    private async void LoadPopularGamesAsync() {
        try
        {
            string response = await GetRAWGYearlyQueryResultAsync();
            if (response != null)
            {
                GameQuery gameData = JsonConvert.DeserializeObject<GameQuery>(response);

                if (gameData != null && gameData.GamesCount > 0)
                {
                    PopulateCarouselView(gameData.results);
                }
                else
                {
                    DisplayError("No game data found.");
                }
            }
            else
            {
                DisplayError("Failed to fetch game data.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            DisplayError("An error occurred while fetching data.");
        }
    }

    private async void LoadGenreGamesAsync() {
        try {
            string? response = await GetRAWGGenreQueryResultAsync();
            Debug.WriteLine($"JSON Response: {response}");
            if (response != null)
            {
                GameQuery gameData = JsonConvert.DeserializeObject<GameQuery>(response);

                if (gameData != null && gameData.GamesCount > 0)
                {
                    PopulateListView(gameData.results);
                }
                else
                {
                    DisplayError("No game data found.");
                }
            }
            else
            {
                DisplayError("Failed to fetch game data.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            DisplayError("An error occurred while fetching data.");
        }
    }

    private void PopulateCarouselView(Game[] games)
    {
        CarouselItem[] carouselItemCollection = new CarouselItem[Math.Min(games.Length, 10)];

        for (int i = 0; i < carouselItemCollection.Length; i++)
        {
            Game curGame = games[i];
            string curGameName = curGame.GameName;
            string curGameImage = curGame.BackgroundImage;

            if (!string.IsNullOrEmpty(curGameName) && !string.IsNullOrEmpty(curGameImage))
            {
                CarouselItem curCarouselItem = new CarouselItem { TheGame = curGame, Title = curGameName, ImageSource = curGameImage };
                carouselItemCollection[i] = curCarouselItem;
            }
        }

        carouselView.ItemsSource = carouselItemCollection;
    }

    private void PopulateListView(Game[] games)
    {
        List<ListItem> listItemCollection = new List<ListItem>();

        for (int i = 0; i < Math.Min(games.Length, 15); i++) {
            Game curGame = games[i];
            string curGameName = curGame.GameName;
            string curGameImage = curGame.BackgroundImage;

            if (!string.IsNullOrEmpty(curGameName) && !string.IsNullOrEmpty(curGameImage))
            {
                ListItem curListItem = new ListItem { ListGame = curGame, ListTitle = curGameName, ListImageSource = curGameImage };
                listItemCollection.Add(curListItem);
            }
        }

        // Set the list as the ItemsSource of the ListView
        listView.ItemsSource = listItemCollection;
    }

    private string CreateTopYearlyGamesQuery()
    {
        DateTime currentDate = DateTime.Now;
        // Format the date correctly for the API string
        string curYear = currentDate.Year.ToString("D4");
        string curMonth = currentDate.Month.ToString("D2");
        string curDay = currentDate.Day.ToString("D2");
        // Build full request
        string requestUri = $"{RAWGApiEndpoint}games?key={RAWGApiKEY}&dates={curYear}-01-01,{curYear}-{curMonth}-{curDay}&ordering=-metacritic";
        return requestUri;
    }

    private string CreateGenreQuery() {
        // Get the formatted genre
        string formattedGenre = FormatGenre();
        // Build full request
        string requestUri = $"{RAWGApiEndpoint}games?key={RAWGApiKEY}&genres={formattedGenre}&ordering=-metacritic";
        return requestUri;
    }

    private string FormatGenre() {
        string currentGenrePreference = Preferences.Get("GenrePref", "Action");
        switch (currentGenrePreference) {
            case "Adventure": return "adventure";
            case "Puzzle": return "puzzle";
            case "Indie": return "indie";
            case "RPG": return "role-playing-games-rpg";
            case "Shooter": return "shooter";
            case "Casual": return "casual";
            case "Simulation": return "simulation";
            case "Arcade": return "arcade";
            case "Fighting": return "fighting";
            case "Platformer": return "platformer";
            case "Racing": return "racing";
            case "Sports": return "sports";
            case "Family": return "family";
            case "Strategy": return "strategy";
            case "Educational": return "educational";
            case "Card": return "card";
            default: return "action";
        }

    }

    private async Task<string?> GetRAWGYearlyQueryResultAsync()
    {
        string query = CreateTopYearlyGamesQuery();
        string? result = null;

        try
        {
            var response = await client.GetAsync(query);
            result = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }

        return result;
    }

    private async Task<string?> GetRAWGGenreQueryResultAsync()
    {
        string query = CreateGenreQuery();
        string? result = null;

        try {
            var response = await client.GetAsync(query);
            result = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }

        return result;
    }

    private void DisplayError(string errorMessage) {
        // Implement error handling, e.g., display an error message to the user.
        // You can update UI elements or show a dialog to inform the user about the error.
    }

    public class CarouselItem {
        public Game TheGame { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }
    }

    public class ListItem {
        public Game ListGame { get; set; }
        public string ListTitle { get; set; }
        public string ListImageSource { get; set; }
    }

    private async void gameImageBtn_Clicked(object sender, EventArgs e) {
        CarouselItem currentItem = carouselView.CurrentItem as CarouselItem;
        GameView gameViewPage = new GameView(currentItem.TheGame);
        await Navigation.PushModalAsync(gameViewPage, true);
    }

    private async void listView_ItemTapped(object sender, ItemTappedEventArgs e) {
        if (e.Item is ListItem tappedItem)
        {
            Game listViewGame = tappedItem.ListGame;

            if (listViewGame != null)
            {
                GameView gameViewPage = new GameView(listViewGame);
                await Navigation.PushModalAsync(gameViewPage, true);
            }
        }
    }
}