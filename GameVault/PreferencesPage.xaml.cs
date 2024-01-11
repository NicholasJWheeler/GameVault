namespace GameVault;

public partial class PreferencesPage : ContentPage
{
    // Notify the profile page when an event is changed
    public event EventHandler PreferenceChanged;
	public PreferencesPage() {
		InitializeComponent();

        // Handle the Username preference
        string defaultUsername = "User";
        if (!Preferences.ContainsKey("Username"))
            Preferences.Set("Username", defaultUsername);
        usernameEntry.Text = Preferences.Get("Username", defaultUsername);

        // Handle the Profile Picture preference
        string defaultProfilePicture = "standard.png";
        if (!Preferences.ContainsKey("ProfilePicture"))
            Preferences.Set("ProfilePicture", defaultProfilePicture);
        string currentPfp = Preferences.Get("ProfilePicture", defaultProfilePicture);
        switch (currentPfp) {
            case "standard.png":
                profilePicturePicker.SelectedIndex = 0;
                // profilePicturePicker.SelectedItem = "Default";
                break;
            case "astronaut.png":
                profilePicturePicker.SelectedIndex = 1;
                // profilePicturePicker.SelectedItem = "Astronaut";
                break;
            case "dinosaur.png":
                profilePicturePicker.SelectedIndex = 2;
                // profilePicturePicker.SelectedItem = "Dinosaur";
                break;
            case "diver.png":
                profilePicturePicker.SelectedIndex = 3;
                // profilePicturePicker.SelectedItem = "Diver";
                break;
            case "wizard.png":
                profilePicturePicker.SelectedIndex = 4;
                // profilePicturePicker.SelectedItem = "Wizard";
                break;
        }

        // Handle the Game Length preference
        string defaultGameLengthPreference = "Shorter";
        if (!Preferences.ContainsKey("GameLength"))
            Preferences.Set("GameLength", defaultGameLengthPreference);
        string currentGameLength = Preferences.Get("GameLength", defaultGameLengthPreference);
        switch (currentGameLength)
        {
            case "Shorter":
                gameLengthPicker.SelectedIndex = 0;
                gameLengthPicker.SelectedItem = "Shorter";
                break;
            case "Longer":
                gameLengthPicker.SelectedIndex = 1;
                gameLengthPicker.SelectedItem = "Longer";
                break;
        }

        // Handle the Game Genre preference
        string defaultGameGenre = "Action";
        if (!Preferences.ContainsKey("GenrePref"))
            Preferences.Set("GenrePref", defaultGameGenre);
        string currentGenre = Preferences.Get("GenrePref", defaultGameGenre);
        switch (currentGenre)
        {
            case "Action":
                favoriteGenrePicker.SelectedIndex = 0;
                break;
            case "Adventure":
                favoriteGenrePicker.SelectedIndex = 1;
                break;
            case "Indie":
                favoriteGenrePicker.SelectedIndex = 2;
                break;
            case "RPG":
                favoriteGenrePicker.SelectedIndex = 3;
                break;
            case "Strategy":
                favoriteGenrePicker.SelectedIndex = 4;
                break;
            case "Shooter":
                favoriteGenrePicker.SelectedIndex = 5;
                break;
            case "Casual":
                favoriteGenrePicker.SelectedIndex = 6;
                break;
            case "Simulation":
                favoriteGenrePicker.SelectedIndex = 7;
                break;
            case "Puzzle":
                favoriteGenrePicker.SelectedIndex = 8;
                break;
            case "Arcade":
                favoriteGenrePicker.SelectedIndex = 9;
                break;
            case "Platformer":
                favoriteGenrePicker.SelectedIndex = 10;
                break;
            case "Racing":
                favoriteGenrePicker.SelectedIndex = 11;
                break;
            case "Sports":
                favoriteGenrePicker.SelectedIndex = 12;
                break;
            case "Fighting":
                favoriteGenrePicker.SelectedIndex = 13;
                break;
            case "Family":
                favoriteGenrePicker.SelectedIndex = 14;
                break;
            case "Educational":
                favoriteGenrePicker.SelectedIndex = 15;
                break;
            case "Card":
                favoriteGenrePicker.SelectedIndex = 16;
                break;
        }
    }

    private async void DonePrefBtn_Clicked(object sender, EventArgs e) {
        // Set Username
        Preferences.Set("Username", usernameEntry.Text);

        // Set Profile Picture
        int curProfilePicture = profilePicturePicker.SelectedIndex;
        switch (curProfilePicture) {
            case 0:
                Preferences.Set("ProfilePicture", "standard.png");
                break;
            case 1:
                Preferences.Set("ProfilePicture", "astronaut.png");
                break;
            case 2:
                Preferences.Set("ProfilePicture", "dinosaur.png");
                break;
            case 3:
                Preferences.Set("ProfilePicture", "diver.png");
                break;
            case 4:
                Preferences.Set("ProfilePicture", "wizard.png");
                break;
        }

        // Set Game Length Preference
        int curGameLength = favoriteGenrePicker.SelectedIndex;
        switch (curGameLength) {
            case 0:
                Preferences.Set("GenrePref", "Action");
                break;
            case 1:
                Preferences.Set("GenrePref", "Adventure");
                break;
            case 2:
                Preferences.Set("GenrePref", "Indie");
                break;
            case 3:
                Preferences.Set("GenrePref", "RPG");
                break;
            case 4:
                Preferences.Set("GenrePref", "Strategy");
                break;
            case 5:
                Preferences.Set("GenrePref", "Shooter");
                break;
            case 6:
                Preferences.Set("GenrePref", "Casual");
                break;
            case 7:
                Preferences.Set("GenrePref", "Simulation");
                break;
            case 8:
                Preferences.Set("GenrePref", "Puzzle");
                break;
            case 9:
                Preferences.Set("GenrePref", "Arcade");
                break;
            case 10:
                Preferences.Set("GenrePref", "Platformer");
                break;
            case 11:
                Preferences.Set("GenrePref", "Racing");
                break;
            case 12:
                Preferences.Set("GenrePref", "Sports");
                break;
            case 13:
                Preferences.Set("GenrePref", "Fighting");
                break;
            case 14:
                Preferences.Set("GenrePref", "Family");
                break;
            case 15:
                Preferences.Set("GenrePref", "Educational");
                break;
            case 16:
                Preferences.Set("GenrePref", "Card");
                break;
        }

        // Set Game Genre Preference
        int curGameGenre = gameLengthPicker.SelectedIndex;
        switch (curGameGenre)
        {
            case 0:
                Preferences.Set("GameLength", "Shorter");
                break;
            case 1:
                Preferences.Set("GameLength", "Longer");
                break;
        }

        // Invoke an update for the profile page
        PreferenceChanged?.Invoke(this, EventArgs.Empty);

        // Update the status in SharedViewModel for Trending (genre)
        App.SharedVM.Status = "New Status";

        // Trigger the UpdateUI event for Trending
        App.SharedVM.OnSomethingChanged();

        // Update the status in SharedViewModel for the backlog (playtime)
        App.SharedVMBacklog.Status = "New Status";

        // Trigger the UpdateUI event for Backlog
        App.SharedVMBacklog.OnSomethingChanged();

        // Exit Page
        await Navigation.PopModalAsync();
    }
}