using SQLite;
using System;
using System.Collections.ObjectModel;

namespace GameVault;

public partial class Backlog : ContentPage
{
	public Backlog()
	{
		InitializeComponent();
        DB.OpenConnection();
        updateListView();

        // Subscribe to the UpdateUI event
        App.SharedVMBacklog.UpdateUI += OnUpdateUI;
    }

    // Helper method to let the app know when to update the Backlog page
    private void OnUpdateUI(object sender, EventArgs e) {
        updateListView();
    }

    public void updateListView() {
        List<BacklogItem> backlogGameList = new List<BacklogItem>();
        var dbBacklogGames = DB.backlogConn.Table<BacklogGame>().ToList();
        IEnumerable<Object> result = null;
        if (byPlaytimeRadio.IsChecked == true) {
            // Pull backlog games out of database with LINQ
            string gameLengthPref = Preferences.Get("GameLength", "Shorter");
            if (gameLengthPref == "Longer") { // User likes longer games
                result = from g in dbBacklogGames
                         orderby g.Playtime descending
                         select g;
            } else { // User likes shorter games
                result = from g in dbBacklogGames
                         orderby g.Playtime
                         select g;
            }
            for (int i = 0; i < result.Count(); i++) {
                Color curLabelColor = Color.FromHex("#ffffff");
                int currentGameRanking = i + 1;
                Object o = result.ElementAt(i);
                if (o is BacklogGame) { // Check if cast is valid
                    BacklogGame curBacklogGame = o as BacklogGame;
                    if (curBacklogGame.Name != null && curBacklogGame.ImageSource != null
                        && curBacklogGame.Release != null && curBacklogGame.ESRBRating != null
                        && curBacklogGame.Platforms != null && curBacklogGame.Genres != null) {
                        if (i == 0) { // First Game, Gold Color
                            curLabelColor = Color.FromHex("#FFD700");
                        } else if (i == 1) { // Second Game, Silver Color
                            curLabelColor = Color.FromHex("#C0C0C0");
                        } else if (i == 2) { // Third Game, Bronze Color
                            curLabelColor = Color.FromHex("#CD7F32");
                        }
                        string formattedItemTitle = $"#{currentGameRanking}: {curBacklogGame.Name}";
                        BacklogItem curBacklogItem = new BacklogItem { 
                            BacklogId = curBacklogGame.Id,
                            BacklogListTitle = formattedItemTitle,
                            BacklogGameTitle = curBacklogGame.Name,
                            BacklogListImageSource = curBacklogGame.ImageSource,
                            BacklogListRelease = curBacklogGame.Release,
                            BacklogListMetacritic = curBacklogGame.Metacritic,
                            BacklogListESRBRating = curBacklogGame.ESRBRating,
                            BacklogListPlatforms = curBacklogGame.Platforms,
                            BacklogListGenres = curBacklogGame.Genres,
                            BacklogListPlaytime = curBacklogGame.Playtime,
                            BacklogListFormattedPlaytime = $"Average Playtime: {curBacklogGame.Playtime} hours (may be inaccurate)",
                            BacklogLabelColor = curLabelColor};
                        backlogGameList.Add(curBacklogItem);
                    }
                }
            }
        } else if (byNameRadio.IsChecked == true) {
            result = from g in dbBacklogGames
                     orderby g.Name
                     select g;
            for (int i = 0; i < result.Count(); i++)
            {
                Color curLabelColor = Color.FromHex("#ffffff");
                int currentGameRanking = i + 1;
                Object o = result.ElementAt(i);
                if (o is BacklogGame) { // Check if cast is valid
                    BacklogGame curBacklogGame = o as BacklogGame;
                    if (curBacklogGame.Name != null && curBacklogGame.ImageSource != null
                        && curBacklogGame.Release != null && curBacklogGame.ESRBRating != null
                        && curBacklogGame.Platforms != null && curBacklogGame.Genres != null)
                    {
                        if (i == 0)
                        { // First Game, Gold Color
                            curLabelColor = Color.FromHex("#FFD700");
                        }
                        else if (i == 1)
                        { // Second Game, Silver Color
                            curLabelColor = Color.FromHex("#C0C0C0");
                        }
                        else if (i == 2)
                        { // Third Game, Bronze Color
                            curLabelColor = Color.FromHex("#CD7F32");
                        }
                        string formattedItemTitle = $"#{currentGameRanking}: {curBacklogGame.Name}";
                        BacklogItem curBacklogItem = new BacklogItem
                        {
                            BacklogId = curBacklogGame.Id,
                            BacklogListTitle = formattedItemTitle,
                            BacklogGameTitle = curBacklogGame.Name,
                            BacklogListImageSource = curBacklogGame.ImageSource,
                            BacklogListRelease = curBacklogGame.Release,
                            BacklogListMetacritic = curBacklogGame.Metacritic,
                            BacklogListESRBRating = curBacklogGame.ESRBRating,
                            BacklogListPlatforms = curBacklogGame.Platforms,
                            BacklogListGenres = curBacklogGame.Genres,
                            BacklogListPlaytime = curBacklogGame.Playtime,
                            BacklogListFormattedPlaytime = $"Average Playtime: {curBacklogGame.Playtime} hours (may be inaccurate)",
                            BacklogLabelColor = curLabelColor
                        };
                        backlogGameList.Add(curBacklogItem);
                    }
                }
            }
        }
        backlogListView.ItemsSource = backlogGameList?.ToList();
    }

    private void byPlaytimeRadio_CheckedChanged(object sender, CheckedChangedEventArgs e) {
        updateListView();
    }

    private void byNameRadio_CheckedChanged(object sender, CheckedChangedEventArgs e) {
       updateListView();
    }

    public class BacklogItem {
        //public Game BacklogGame { get; set; }
        public int BacklogId { get; set; }
        public string BacklogListTitle { get; set; }
        public string BacklogGameTitle { get; set; }
        public string BacklogListImageSource { get; set; }
        public string BacklogListRelease { get; set; }
        public int BacklogListMetacritic { get; set; }
        public string BacklogListESRBRating { get; set; }
        public string BacklogListPlatforms { get; set; }
        public string BacklogListGenres { get; set; }
        public int BacklogListPlaytime { get; set; }
        public string BacklogListFormattedPlaytime { get; set; }
        public Color BacklogLabelColor { get; set; }
    }

    private async void backlogListView_ItemTapped(object sender, ItemTappedEventArgs e) {
        if (e.Item is BacklogItem tappedItem) {
            BacklogItem backlogListGame = tappedItem;
            if (backlogListGame != null) {
                string userChoice = await DisplayActionSheet("What Would You Like To Do?", "Cancel", null,
                                    "View Game", "Delete Game");
                if (userChoice == "View Game") { // View The Game
                    BacklogGameView backlogGameView = new BacklogGameView(backlogListGame);
                    await Navigation.PushModalAsync(backlogGameView, true); // A delay may/may not result here
                    /*updatePage.SymptomUpdated += (sender, args) =>
                    {
                        updateObservableCollection();
                    };*/
                }
                else if (userChoice == "Delete Game") { // Delete the record
                    bool result = await DisplayAlert("Warning",
                        "Do you wish to delete this game?", "Yes", "No");
                    if (result) { // Yes to delete record
                        int curID = backlogListGame.BacklogId;
                        var gameToDelete = DB.backlogConn.Table<BacklogGame>().FirstOrDefault(x => x.Id == curID);
                        DB.backlogConn.Delete(gameToDelete);

                        // Update the status in SharedViewModel
                        App.SharedVMBacklog.Status = "New Status";
                        // Trigger the UpdateUI event for Backlog
                        App.SharedVMBacklog.OnSomethingChanged();

                        await DisplayAlert("Success", "Game removed from Backlog", "OK");
                    } else { // Close alert
                    }
                }

            }
        }
    }
}