using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Apparat.ViewModel
{
    public class ExplorerViewModel
    {
        private string _folderName = "NewFolder";
        private ObservableCollection<HostViewModel> _folderCollection;

        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value; OnPropertyChanged("FolderName");
            }
        }

        public ObservableCollection<HostViewModel> FolderCollection
        {
            get { return _folderCollection; }
            set { _folderCollection = value; OnPropertyChanged("FolderCollection"); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
