using GameVault.Resources.Styles;

namespace GameVault;

public partial class Profile : ContentPage {
	public Profile() {
		InitializeComponent();
        DB.OpenConnection();

        // Load user's profile from Preferences
        UsernameLabel.Text = $"Welcome, {Preferences.Get("Username", "User")}";
        ProfileImg.Source = $"{Preferences.Get("ProfilePicture", "standard.png")}";
        gamePreferenceLabel.Text = $"Prefers {Preferences.Get("GameLength", "Shorter")} {Preferences.Get("GenrePref", "Action")} Games";

        // Update Backlog Game Statistics
        UpdateBacklogGameCount();
        App.SharedVMBacklog.UpdateUI += OnUpdateBacklogUI;

        // Update Vault Game Statistics
        UpdateVaultGameCount();
        UpdateVaultGameRatingAvg();
        
        App.SharedVMVault.UpdateUI += OnUpdateVaultUI;
    }

    // Helper method to let the app know when to update the Backlog game count
    private void OnUpdateBacklogUI(object sender, EventArgs e) {
        UpdateBacklogGameCount();
    }

    // Helper method to let the app know when to update the Vault game stats
    private void OnUpdateVaultUI(object sender, EventArgs e)
    {
        UpdateVaultGameCount();
        UpdateVaultGameRatingAvg();
    }

    private void UpdateBacklogGameCount() {
        IEnumerable<Object> result = null;
        var dbBacklogGames = DB.backlogConn.Table<BacklogGame>().ToList();
        result = from g in dbBacklogGames
                 select g;
        int? backlogGameCount = result.Count();
        if (backlogGameCount.HasValue && backlogGameCount > -1) {
            backlogGameCountLabel.Text = $"Games In Backlog: {backlogGameCount}";
        } else {
            backlogGameCountLabel.Text = $"Games In Backlog: 0";
        }
    }

    private void UpdateVaultGameCount() {
        IEnumerable<Object> result = null;
        var dbVaultGames = DB.vaultConn.Table<VaultGame>().ToList();
        result = from g in dbVaultGames
                 select g;
        int? vaultGameCount = result.Count();
        if (vaultGameCount.HasValue && vaultGameCount > -1)
        {
            vaultGameCountLabel.Text = $"Games Finished: {vaultGameCount}";
        }
        else
        {
            vaultGameCountLabel.Text = "Games Finished: 0";
        }
    }

    private void UpdateVaultGameRatingAvg() {
        double finalAvg = 0.00;
        IEnumerable<Object> result = null;
        var dbVaultGames = DB.vaultConn.Table<VaultGame>().ToList();
        result = from g in dbVaultGames
                 select g;
        int? vaultGameCount = result.Count();
        if (vaultGameCount.HasValue && vaultGameCount > 0) {
            for (int i = 0; i < vaultGameCount; i++) {
                Object o = result.ElementAt(i);
                if (o is VaultGame) { // Check if cast is valid
                    VaultGame curVaultGame = o as VaultGame;
                    finalAvg = finalAvg + curVaultGame.Rating;
                }
            }
            finalAvg = (double)(finalAvg / vaultGameCount);
            vaultGameRatingAvgLabel.Text = $"Average Rating Given: {finalAvg:F2}";
        } else {
            vaultGameRatingAvgLabel.Text = "Average Rating Given: 0.0";
        }
    }

    // Go to user's completed game vault
    private async void myVaultBtn_Clicked(object sender, EventArgs e) {
        MyVault myVaultPage = new MyVault();
        await Navigation.PushModalAsync(myVaultPage, true);
    }

    // Go to user's preferences
    private async void myPreferencesBtn_Clicked(object sender, EventArgs e) {
        PreferencesPage preferencesPage = new PreferencesPage();
        await Navigation.PushModalAsync(preferencesPage, true);

        // Refresh profile page
        preferencesPage.PreferenceChanged += (sender, args) =>
        {
            // Load user's profile from Preferences
            UsernameLabel.Text = $"Welcome, {Preferences.Get("Username", "User")}";
            ProfileImg.Source = $"{Preferences.Get("ProfilePicture", "standard.png")}";
            gamePreferenceLabel.Text = $"Prefers {Preferences.Get("GameLength", "Shorter")}" +
                $" {Preferences.Get("GenrePref", "Action")} Games";
        };

    }
}