using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Passpad.WPF.BaseViewModel
{
    public abstract class ObservableObject :IViewModel
    {
        private bool _isModified;
        private bool _isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsModified
        {
            get
            {
                return _isModified;
            }
            set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            RaisePropertyChanged(false, property);
        }

        protected virtual void RaisePropertyChanged(bool causesModification, [CallerMemberName] string property = "")
        {
            IsModified = IsModified || causesModification;

	        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
