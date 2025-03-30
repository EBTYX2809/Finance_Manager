using Finance_Manager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Finance_Manager.ViewModels;

public class TransactionsViewModel : INotifyPropertyChanged
{
    private readonly UserSession _userSession;
    public TransactionsViewModel(UserSession userSession)
    {
        _userSession = userSession;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}