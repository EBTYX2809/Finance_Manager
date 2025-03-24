using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.Models;

public class Transaction : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private decimal _price;
    private Category _category = new();
    private Category _innerCategory = new();
    private DateTime _date = DateTime.Now;
    private string _photo = string.Empty;

    public int Id;
    public int UserId;

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

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                OnPropertyChanged();
            }
        }
    }

    public Category Category
    {
        get => _category;
        set
        {
            if (_category != value)
            {
                _category = value;
                OnPropertyChanged();
            }
        }
    }

    public Category InnerCategory
    {
        get => _innerCategory;
        set
        {
            if (_innerCategory != value)
            {
                _innerCategory = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date
    {
        get => _date;
        set
        {
            if (_date != value)
            {
                _date = value;
                OnPropertyChanged();
            }
        }
    }

    public string Photo
    {
        get => _photo;
        set
        {
            if (_photo != value)
            {
                _photo = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
