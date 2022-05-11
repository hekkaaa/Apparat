using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WinObserver.Model 
{
    public class ItemPing : INotifyPropertyChanged
    {
        private string _ip;
        private string _delay;


        public string Ip
        {
            get { return _ip; }
            set
            {
                _ip = value;
                OnPropertyChanged("Ip");

            }
        }

        public string Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                OnPropertyChanged("Delay");
            }
        }

        public void Additems()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
