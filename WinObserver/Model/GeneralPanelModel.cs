using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinObserver.Model
{
    public class GeneralPanelModel : INotifyPropertyChanged
    {
        private string _nameStartBtn = ViewStatusStringBtnEnum.Start.ToString();

        public string NameControlBtn
        {
            get { return _nameStartBtn; }
            set
            {
                _nameStartBtn = value;
                OnPropertyChanged("NameStartBtn");
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
