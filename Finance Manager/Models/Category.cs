using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.Models;

public class Category : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _icon = string.Empty;
    private string _colorForBackground = string.Empty;
    private int? _parentCategoryId;
    private List<Category> _innerCategories = new();

    public int Id;

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

    public string Icon
    {
        get => _icon;
        set
        {
            if (_icon != value)
            {
                _icon = value;
                OnPropertyChanged();
            }
        }
    }

    public string ColorForBackground
    {
        get => _colorForBackground;
        set
        {
            if (_colorForBackground != value)
            {
                _colorForBackground = value;
                OnPropertyChanged();
            }
        }
    }

    public int? ParentCategoryId
    {
        get => _parentCategoryId;
        set
        {
            if (_parentCategoryId != value)
            {
                _parentCategoryId = value;
                OnPropertyChanged();
            }
        }
    }

    public List<Category> InnerCategories
    {
        get => _innerCategories;
        set
        {
            if (_innerCategories != value)
            {
                _innerCategories = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
