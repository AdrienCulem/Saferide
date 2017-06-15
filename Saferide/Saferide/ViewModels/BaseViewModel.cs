using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Saferide.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected bool _isBusy;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
