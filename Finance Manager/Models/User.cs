using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.Models;

public class User : INotifyPropertyChanged
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private decimal _balance;
    private List<Transaction> _spendings = new();
    private List<Saving> _savings = new();

    public int Id;

    public string Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged();
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (_password != value)
            {
                _password = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal Balance
    {
        get => _balance;
        set
        {
            if (_balance != value)
            {
                _balance = value;
                OnPropertyChanged();
            }
        }
    }

    public List<Transaction> Spendings
    {
        get => _spendings;
        set
        {
            if (_spendings != value)
            {
                _spendings = value;
                OnPropertyChanged();
            }
        }
    }

    public List<Saving> Savings
    {
        get => _savings;
        set
        {
            if (_savings != value)
            {
                _savings = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}