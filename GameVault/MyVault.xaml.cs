using SQLite;
using System;
using System.Collections.ObjectModel;

namespace GameVault;

public partial class MyVault : ContentPage {
	public MyVault() {
		InitializeComponent();
        DB.OpenConnection();
        UpdateListView();

        // Subscribe to the UpdateUI event
        App.SharedVMVault.UpdateUI += OnUpdateUI;
    }

    // Helper method to let the app know when to update the Vault page
    private void OnUpdateUI(object sender, EventArgs e) {
        UpdateListView();
    }

    public void UpdateListView() {
        List<VaultItem> vaultGameList = new List<VaultItem>();
        var dbVaultGames = DB.vaultConn.Table<VaultGame>().ToList();
        IEnumerable<Object> result = null;
        if (byRatingRadio.IsChecked == true) {
            // Pull backlog games out of database with LINQ (Sort by highest user rating)
            result = from g in dbVaultGames
                     orderby g.Rating descending
                     select g;
            // Populate vaultGameList
            for (int i = 0; i < result.Count(); i++)
            {
                Color curRatingColor = Color.FromHex("#710000");
                Object o = result.ElementAt(i);
                if (o is VaultGame)
                { // Check if cast is valid
                    VaultGame curVaultGame = o as VaultGame;
                    if (curVaultGame.Name != null && curVaultGame.ImageSource != null &&
                        curVaultGame.Release != null && curVaultGame.Metacritic != null &&
                        curVaultGame.ESRBRating != null && curVaultGame.Platforms != null &&
                        curVaultGame.Genres != null && curVaultGame.Playtime != null &&
                        curVaultGame.PersonalPlaytime != null && curVaultGame.Rating != null &&
                        curVaultGame.ReplayIt != null)
                    {
                        curRatingColor = RatingColorAssignment(curVaultGame.Rating);
                        string formattedUserPlaytime = $"Your Playtime: {curVaultGame.PersonalPlaytime}";
                        string formattedUserRating = $"{curVaultGame.Rating:F2}";
                        string formattedReplayStr = formattedReplayString(curVaultGame.ReplayIt);
                        Color formattedReplayClr = formattedReplayColor(curVaultGame.ReplayIt);

                        VaultItem newVaultItem = new VaultItem
                        {
                            VaultId = curVaultGame.Id,
                            VaultGameTitle = curVaultGame.Name,
                            VaultListImageSource = curVaultGame.ImageSource,
                            VaultListRelease = curVaultGame.Release,
                            VaultListMetacritic = curVaultGame.Metacritic,
                            VaultListESRBRating = curVaultGame.ESRBRating,
                            VaultListPlatforms = curVaultGame.Platforms,
                            VaultListGenres = curVaultGame.Genres,
                            VaultListPlaytime = curVaultGame.Playtime,
                            VaultListFormattedUserPlaytime = formattedUserPlaytime,
                            VaultListUserRating = curVaultGame.Rating,
                            VaultListFormattedUserRating = formattedUserRating,
                            VaultListReplayIt = curVaultGame.ReplayIt,
                            VaultFormattedReplayIt = formattedReplayStr,
                            VaultReplayLabelColor = formattedReplayClr,
                            VaultRatingLabelColor = curRatingColor
                        };
                        vaultGameList.Add(newVaultItem);
                    }
                }
            }
        } else if (byNameRadio.IsChecked == true) {
            // Pull backlog games out of database with LINQ (Alphabetical Order)
            result = from g in dbVaultGames
                     orderby g.Name
                     select g;
            // Populate vaultGameList
            for (int i = 0; i < result.Count(); i++)
            {
                Color curRatingColor = Color.FromHex("#710000");
                Object o = result.ElementAt(i);
                if (o is VaultGame)
                { // Check if cast is valid
                    VaultGame curVaultGame = o as VaultGame;
                    if (curVaultGame.Name != null && curVaultGame.ImageSource != null &&
                        curVaultGame.Release != null && curVaultGame.Metacritic != null &&
                        curVaultGame.ESRBRating != null && curVaultGame.Platforms != null &&
                        curVaultGame.Genres != null && curVaultGame.Playtime != null &&
                        curVaultGame.PersonalPlaytime != null && curVaultGame.Rating != null &&
                        curVaultGame.ReplayIt != null)
                    {
                        curRatingColor = RatingColorAssignment(curVaultGame.Rating);
                        string formattedUserPlaytime = $"Your Playtime: {curVaultGame.PersonalPlaytime}";
                        string formattedUserRating = $"{curVaultGame.Rating:F2}";
                        string formattedReplayStr = formattedReplayString(curVaultGame.ReplayIt);
                        Color formattedReplayClr = formattedReplayColor(curVaultGame.ReplayIt);

                        VaultItem newVaultItem = new VaultItem
                        {
                            VaultGameTitle = curVaultGame.Name,
                            VaultListImageSource = curVaultGame.ImageSource,
                            VaultListRelease = curVaultGame.Release,
                            VaultListMetacritic = curVaultGame.Metacritic,
                            VaultListESRBRating = curVaultGame.ESRBRating,
                            VaultListPlatforms = curVaultGame.Platforms,
                            VaultListGenres = curVaultGame.Genres,
                            VaultListPlaytime = curVaultGame.Playtime,
                            VaultListFormattedUserPlaytime = formattedUserPlaytime,
                            VaultListUserRating = curVaultGame.Rating,
                            VaultListFormattedUserRating = formattedUserRating,
                            VaultListReplayIt = curVaultGame.ReplayIt,
                            VaultFormattedReplayIt = formattedReplayStr,
                            VaultReplayLabelColor = formattedReplayClr,
                            VaultRatingLabelColor = curRatingColor
                        };
                        vaultGameList.Add(newVaultItem);
                    }
                }
            }
        }

        
        vaultListView.ItemsSource = vaultGameList?.ToList();
    }

    private Color RatingColorAssignment(double rating) {
        Color curRatingColor;
        if (rating >= 4.5) {
            curRatingColor = Color.FromHex("#FFD700");
        } else if (rating >= 4.0) {
            curRatingColor = Color.FromHex("#EFBF00");
        } else if (rating >= 3.5) {
            curRatingColor = Color.FromHex("#DFA700");
        } else if (rating >= 3.0) {
            curRatingColor = Color.FromHex("#D08F00");
        } else if (rating >= 2.5) {
            curRatingColor = Color.FromHex("#C07700");
        } else if (rating >= 2.0) {
            curRatingColor = Color.FromHex("#B06000");
        } else if (rating >= 1.5) {
            curRatingColor = Color.FromHex("#A04800");
        } else if (rating >= 1.0) {
            curRatingColor = Color.FromHex("#913000");
        } else if (rating >= 0.5) {
            curRatingColor = Color.FromHex("#811800");
        } else {
            curRatingColor = Color.FromHex("#710000");
        }
        return curRatingColor;
    }

    private Color formattedReplayColor(bool replayIt) {
        Color curReplayColor;
        if (replayIt) {
            curReplayColor = Color.FromHex("#3f8f29");
        } else {
            curReplayColor = Color.FromHex("#de1a24");
        }
        return curReplayColor;
    }

    private string formattedReplayString(bool replayIt) {
        string curReplayResponse;
        if (replayIt) {
            curReplayResponse = "Would Replay";
        } else {
            curReplayResponse = "Wouldn't Replay";
        }
        return curReplayResponse;
    }

    public class VaultItem {
        public int VaultId { get; set; }
        public string VaultGameTitle { get; set; }
        public string VaultListImageSource { get; set; }
        public string VaultListRelease { get; set; }
        public int VaultListMetacritic { get; set; }
        public string VaultListESRBRating { get; set; }
        public string VaultListPlatforms { get; set; }
        public string VaultListGenres { get; set; }
        public int VaultListPlaytime { get; set; }
        public string VaultListFormattedUserPlaytime { get; set; }
        public double VaultListUserRating { get; set; }
        public string VaultListFormattedUserRating { get; set; }
        public bool VaultListReplayIt { get; set; }
        public string VaultFormattedReplayIt { get; set; }
        public Color VaultReplayLabelColor { get; set; }
        public Color VaultRatingLabelColor { get; set; }
    }

    private void byRatingRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        UpdateListView();
    }

    private void byNameRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        UpdateListView();
    }

    private async void vaultListView_ItemTapped(object sender, ItemTappedEventArgs e) {
        if (e.Item is VaultItem tappedItem)
        {
            VaultItem vaultListGame = tappedItem;
            if (vaultListGame != null) {
                // Ask the user if they'd like to delete
                bool result = await DisplayAlert("Warning",
                        "Do you wish to delete this game?", "Yes", "No");
                if (result) { // Yes to delete record
                    int curID = vaultListGame.VaultId;
                    var gameToDelete = DB.vaultConn.Table<VaultGame>().FirstOrDefault(x => x.Id == curID);
                    DB.vaultConn.Delete(gameToDelete);
                    // Update the status in SharedViewModel (Vault Page)
                    App.SharedVMVault.Status = "New Status";
                    // Trigger the UpdateUI event for user's Vault
                    App.SharedVMVault.OnSomethingChanged();

                    await DisplayAlert("Success", "Game removed from Vault", "OK");
                }
                else
                { // Close alert
                }
            }
        }
    }

    private async void backBtn_Clicked(object sender, EventArgs e) {
        await Navigation.PopModalAsync();
    }
}