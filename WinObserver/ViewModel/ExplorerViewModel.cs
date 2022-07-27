using Apparat.Commands;
using MaterialDesignThemes.Wpf;
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
    public class ExplorerViewModel : INotifyPropertyChanged
    {
        private const string defaultSize = "18";
        private const string defaultBorderBrush = "DeepSkyBlue";
        private const string defaultBorderBrushError = "Red";
        
        public bool IsNewCreateObj { get; set; } = true;

        private string _folderName = String.Empty;
        private ObservableCollection<HostViewModel>? _HostVMCollection;

        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value; OnPropertyChanged("FolderName");
            }
        }

        public ObservableCollection<HostViewModel> HostVMCollection
        {
            get { return _HostVMCollection!; }
            set { _HostVMCollection = value; OnPropertyChanged("FolderCollection"); }
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


        private string _borderBrushColor = defaultBorderBrush;
        public string BorderBrushColor
        {
            get { return _borderBrushColor; }
            set { _borderBrushColor = value; OnPropertyChanged(); }
        }

        private string _sizeElement = defaultSize;
        public string SizeElement
        {
            get { return _sizeElement; }
            set { _sizeElement = value; OnPropertyChanged(); }
        }

        private string _textErrorFolderValidation = string.Empty;
        public string TextErrorFolderValidation
        {
            get { return _textErrorFolderValidation; }
            set { _textErrorFolderValidation = value; OnPropertyChanged(); }
        }
        

        private DelegateCommand _Test552 = null!;
        public DelegateCommand Test552
        {
            get
            {
                return _Test552
                ?? (_Test552 = new DelegateCommand(
                (obj) =>
                {   
                    if(obj is null)
                    {
                        return;
                    }
                    else
                    {
                        ExplorerViewModel folderObj = (ExplorerViewModel)obj;

                        if(String.IsNullOrWhiteSpace(folderObj.FolderName))
                        {
                            
                            TextErrorFolderValidation = "You must specify the folder name.";
                            BorderBrushColor = defaultBorderBrushError;
                            return;
                           
                        }
                      
                        SizeElement = defaultSize;
                        folderObj.VisibleTextBoxNameFolder = "Collapsed";
                        folderObj.VisibleLabelNameFolder = "Visible";
                        IsNewCreateObj = false;
                        OnPropertyChanged();
                    }
                }));
            }
        }

        public void DefaultValuesStyleInTextBoxElement()
        {
            SizeElement = defaultSize;
            BorderBrushColor = defaultBorderBrush;
            TextErrorFolderValidation = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
