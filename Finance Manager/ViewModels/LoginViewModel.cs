using Finance_Manager.Models;
using Finance_Manager.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Finance_Manager.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly UserSession _userSession;
    private readonly AuthService _authService;

    private string _email;
    public string Email
    {
        get => _email;
        set 
        {
            if(value != _email)
            {
                _email = value; 
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set 
        {
            if (value != _password) 
            {
                _password = value; 
                OnPropertyChanged(nameof(Password));
            }
        }
    }

    private bool _isRegisterButtonVisible;
    public bool IsRegisterButtonVisible
    {
        get => _isRegisterButtonVisible;
        set
        {
            if (value != _isRegisterButtonVisible)
            {
                _isRegisterButtonVisible = value;
            }
        }
    }

    public ICommand LoginCommand { get; set; }
    public LoginViewModel(UserSession userSession, AuthService authService)
    {
        _userSession = userSession;
        _authService = authService;

        LoginCommand = new RelayCommand(LoginAsync);

        LoginCheck();
    }

    private void LoginCheck()
    {
        string[] credentials = []; // Взять из компа
        if (credentials != null)
        {
            Email = credentials[0];
            Password = credentials[1];
            IsRegisterButtonVisible = false;
        }
        else IsRegisterButtonVisible = true;       
    }

    private async void LoginAsync(object parameter)
    {
        if(parameter is string type) // Передать в параметрах команды
        {
            if (type == "auth")
            {
                _userSession.CurrentUser = await _authService.AuthenticateUserAsync(Email, Password);
            }
            else if(type == "reg")
            {
                _userSession.CurrentUser = await _authService.RegisterUserAsync(Email, Password);
            }
        }

        if (_userSession.IsAuthenticated)
        {
            Window currentWindow = Application.Current.MainWindow;

            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();

            currentWindow?.Close();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
