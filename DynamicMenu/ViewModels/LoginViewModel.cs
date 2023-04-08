using System.Collections.ObjectModel;
using System.Diagnostics;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DynamicMenu.EventArg;
using DynamicMenu.Models;
using DynamicMenu.Services;

namespace DynamicMenu.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public static event EventHandler<LoginEventArgs> SuccessfulLogin;
        private readonly UserService service;
        public LoginViewModel(UserService service)
        {
            this.service = service;
        }

        //default value can be set to observable property directly as well and it will work correctly.

        [ObservableProperty]
        private bool availableUsersLoading;

        [ObservableProperty]
        private ObservableCollection<User> availableUsers;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private bool unknownUsernameTextVisible;

        [ObservableProperty]
        private bool isRefreshing;

        private void ResetForm()
        {
            AvailableUsers = new ObservableCollection<User>(new List<User>());
            Name = string.Empty;
            UnknownUsernameTextVisible = false;
        }

        private static int ViewModelOnAppearingCounter = 0;

        [RelayCommand]
        async void OnAppearing()
        {
            ResetForm();
            ViewModelOnAppearingCounter++;
            Debug.WriteLine($"{DateTime.Now}: Called LoginViewModel OnAppearing via toolkit:EventToCommandBehavior hooked to LoginPage.OnAppearing event. This also which calls ReloadUsersCommand. Total: {ViewModelOnAppearingCounter} times.");
            await ReloadUsers();
        }

        [RelayCommand]
        void OnNameChanged()
        {
            UnknownUsernameTextVisible = false;
        }

        [RelayCommand]
        async Task Login()
        {
            var inputText = Name;
            var user = await service.GetUserByName(inputText);
            if (user is null)
            {
                UnknownUsernameTextVisible = true;
                return;
            }

            SuccessfulLogin?.Invoke(this, new LoginEventArgs(user));
        }

        private static int ReloadUsersCommandStartCounter = 0;

        [RelayCommand]
        async Task ReloadUsers()
        {
            ReloadUsersCommandStartCounter++;
            Debug.WriteLine($"{DateTime.Now}: Total started command: {ReloadUsersCommandStartCounter} times");
            AvailableUsers.Clear();

            if (IsRefreshing != true)
            {
                IsRefreshing = true;
            }
            AvailableUsersLoading = true;
            var items = await service.GetAll();
            AvailableUsers = new ObservableCollection<User>(items);
            AvailableUsersLoading = false;
            IsRefreshing = false;
        }
    }
}
