using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.Models;

public class Settings : INotifyPropertyChanged
{
    private string _language = string.Empty;
    private string _theme = string.Empty;
    private string _currency = string.Empty;
    private string _account = string.Empty;

    public string Language
    {
        get => _language;
        set
        {
            if (_language != value)
            {
                _language = value;
                OnPropertyChanged();
            }
        }
    }

    public string Theme
    {
        get => _theme;
        set
        {
            if (_theme != value)
            {
                _theme = value;
                OnPropertyChanged();
            }
        }
    }

    public string Currency
    {
        get => _currency;
        set
        {
            if (_currency != value)
            {
                _currency = value;
                OnPropertyChanged();
            }
        }
    }

    public string Account
    {
        get => _account;
        set
        {
            if (_account != value)
            {
                _account = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}