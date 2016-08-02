using System.ComponentModel;

namespace Passpad.WPF.BaseViewModel
{
    public interface IViewModel: INotifyPropertyChanged
    {
        bool IsModified { get; set; }

        bool IsLoading { get; set; }
    }
}
