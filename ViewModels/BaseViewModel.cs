using System.ComponentModel;


namespace DHOG_WPF.ViewModels
{
    public class BaseViewModel : IEditableObject, INotifyPropertyChanged
    {
        public event ItemEndEditEventHandler ItemEndEdit;

        public BaseViewModel() { }

        public void BeginEdit()
        {
        }

        public void CancelEdit()
        {
        }

        public void EndEdit()
        {
            ItemEndEdit?.Invoke(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
