﻿namespace EmsAmbulanceApp.Mobile.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}