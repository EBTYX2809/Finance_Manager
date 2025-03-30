using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Finance_Manager.Models
{
    public class UserSession : INotifyPropertyChanged
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if(value != _currentUser)
                {
                    _currentUser = value;
                }
            }
        }

        public bool IsAuthenticated => CurrentUser != null;

        public void Logout()
        {
            CurrentUser = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
