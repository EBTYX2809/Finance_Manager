using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.Models;

public class User : INotifyPropertyChanged
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private decimal _balance;
    private ObservableCollection<Transaction> _transactions = new();
    private ObservableCollection<Saving> _savings = new();

    public int Id { get; set; }

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

    public ObservableCollection<Transaction> Transactions
    {
        get => _transactions;
        set
        {
            if (_transactions != value)
            {
                _transactions = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Saving> Savings
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