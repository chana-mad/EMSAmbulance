namespace EmsAmbulanceApp.Mobile.Client;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        var webView = new WebView
        {
            Source = "https://10.0.2.2:7269/AmbulanceRequest/Index"
        };

        Content = new Grid
        {
            Children = { webView }
        };
    }
}