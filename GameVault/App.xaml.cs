namespace GameVault;

public partial class App : Application {

    public static SharedViewModel SharedVM { get; } = new SharedViewModel();
	public static SharedViewModel SharedVMBacklog { get; } = new SharedViewModel();
    public static SharedViewModel SharedVMVault { get; } = new SharedViewModel();
    public App()
	{

		InitializeComponent();

		MainPage = new AppShell();
	}

}
