using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinObserver.Model
{
    public class DataGridModel : INotifyPropertyChanged
    {
        private string _headerNameTable = "New";

        public string HeaderNameTable
        {
            get { return _headerNameTable; }
            set
            {
                _headerNameTable = value;
                OnPropertyChanged("HeaderNameTable");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
