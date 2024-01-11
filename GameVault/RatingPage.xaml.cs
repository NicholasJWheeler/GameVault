namespace GameVault;

public partial class RatingPage : ContentPage
{
    private string pName, pImageSource, pRelease, pESRBRating, pPlatforms, pGenres;
    private int pMetacritic, pPlaytime;
	public RatingPage(string passName, string passImageSource, string passRelease,
        string passESRBRating, string passPlatforms, string passGenres, int passMetacritic, int passPlaytime) {
		InitializeComponent();
        LoadGameInfo(passName, passImageSource);
        gameReplayPicker.SelectedIndex = 0;
        gameReplayPicker.SelectedItem = "Yes";
        ratingSlider.Value = 2.50;
        pName = passName;
        pImageSource = passImageSource;
        pRelease = passRelease;
        pESRBRating = passESRBRating;
        pPlatforms = passPlatforms;
        pGenres = passGenres;
        pMetacritic = passMetacritic;
        pPlaytime = passPlaytime;
    }

    public void LoadGameInfo(string curGameName, string curGameImage) {
        // Fill in Game Name
        if (!string.IsNullOrEmpty(curGameName)) {
            int nameLength = curGameName.Length;
            if (nameLength > 18) {
                gameNameLabel.FontSize = 16;
            } else if (nameLength > 14) {
                gameNameLabel.FontSize = 20;
            }
            gameNameLabel.Text = curGameName;
        } else {
            gameNameLabel.Text = "Game name currently unavailable";
            gameNameLabel.FontSize = 18;
        }

        // Fill in Game Image
        if (!string.IsNullOrEmpty(curGameImage)) {
            currentGameImage.Source = curGameImage;
        } else {
            currentGameImage.Source = "image_unavailable.png";
        }
    }

    private async void addToVaultBtn_Clicked(object sender, EventArgs e) {
        string? hoursEntry = playedHoursEntry.Text;
        if (!string.IsNullOrEmpty(hoursEntry)) {
            if (IsNumeric(hoursEntry)) { // Check for numerical hours input
                double userEnjoymentRating = ratingSlider.Value;
                int personalPlaytime = int.Parse(hoursEntry);
                bool willReplayGame;
                if (gameReplayPicker.SelectedIndex == 0) {
                    willReplayGame = true;
                } else {
                    willReplayGame = false;
                }

                // Create new VaultGame Object to be stored
                VaultGame newVaultGame = new VaultGame { Name = pName, ImageSource = pImageSource,
                    Release = pRelease, Metacritic = pMetacritic, ESRBRating = pESRBRating, Platforms = pPlatforms,
                    Genres = pGenres, Playtime = pPlaytime, PersonalPlaytime = personalPlaytime, Rating = userEnjoymentRating, ReplayIt = willReplayGame};

                // Add to Vault Database
                DB.vaultConn.Insert(newVaultGame);

                // Update the status in SharedViewModel (Vault Page)
                App.SharedVMVault.Status = "New Status";

                // Trigger the UpdateUI event for user's Vault
                App.SharedVMVault.OnSomethingChanged();

                // Exit page
                await Navigation.PopModalAsync();

            } else { // Not numeric
                await DisplayAlert("Incorrect Response", "Hours played needs to be a whole number!", "OK");
            }
        } else { // Not valid input
            await DisplayAlert("Incorrect Response", "Hours played needs to be a whole number!", "OK");
        }
    }

    private async void cancelBtn_Clicked(object sender, EventArgs e) {
        await Navigation.PopModalAsync();
    }

    private void ratingSlider_ValueChanged(object sender, ValueChangedEventArgs e) {
        double sliderVal = (double)e.NewValue;
        gamePersonalRatingLabel.Text = $"Your Game Rating: {sliderVal:F2}";
    }

    static bool IsNumeric(string input)
    {
        // Check if the string contains only numeric digits
        foreach (char c in input)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }

        return true;
    }
}