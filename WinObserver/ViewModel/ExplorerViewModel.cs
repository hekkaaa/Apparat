using Apparat.Commands;
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

        private string _visibleTextBoxNameFolder = "Collapsed";
        public string VisibleTextBoxNameFolder
        {
            get { return _visibleTextBoxNameFolder; }
            set { _visibleTextBoxNameFolder = value; OnPropertyChanged(); }
        }

        private string _visibleLabelNameFolder = "Visible";
        public string VisibleLabelNameFolder
        {
            get { return _visibleLabelNameFolder; }
            set { _visibleLabelNameFolder = value; OnPropertyChanged(); }
        }

        private DelegateCommand _Test55 = null!;
        public DelegateCommand Test55
        {
            get
            {
                return _Test55
                ?? (_Test55 = new DelegateCommand(
                (obj) =>
                {
                    var yy = obj as ExplorerViewModel;
                    yy.FolderName = "Витя Покров";
                    yy.VisibleTextBoxNameFolder = "Collapsed";
                    yy.VisibleLabelNameFolder = "Visible";
                }));
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
