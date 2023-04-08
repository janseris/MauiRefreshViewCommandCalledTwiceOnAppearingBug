using System.Diagnostics;

using DynamicMenu.ViewModels;

namespace DynamicMenu.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }

    private static int OnAppearingCounter = 0;
    protected override void OnAppearing()
    {
        base.OnAppearing();
        OnAppearingCounter++;
        Debug.WriteLine($"{DateTime.Now}: Total called LoginPage.OnAppearing: {OnAppearingCounter} times");
    }
}