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
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsBusy
        {
            get { return _isBusy; }
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
