using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.Models;

public class Saving : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private decimal _goal;
    private decimal? _currentAmount;

    public int Id { get; set; }
    public int UserId { get; set; }
    public User user { get; set; } = new();

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal Goal
    {
        get => _goal;
        set
        {
            if (_goal != value)
            {
                _goal = value;
                OnPropertyChanged();
            }
        }
    }

    public decimal? CurrentAmount
    {
        get => _currentAmount;
        set
        {
            if (_currentAmount != value)
            {
                _currentAmount = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
