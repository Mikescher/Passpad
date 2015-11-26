using System.ComponentModel;

namespace Passpad.BaseViewModel
{
    public interface IViewModel: INotifyPropertyChanged
    {
        bool IsModified { get; set; }

        bool IsLoading { get; set; }
    }
}
