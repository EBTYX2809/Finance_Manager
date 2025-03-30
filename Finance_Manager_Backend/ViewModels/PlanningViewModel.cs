using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.ViewModels;

public class PlanningViewModel : INotifyPropertyChanged
{


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}