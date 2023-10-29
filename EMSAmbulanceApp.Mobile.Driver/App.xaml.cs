namespace EMSAmbulanceApp.Mobile.Driver;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}