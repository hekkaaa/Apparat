using Apparat.Helpers.Interfaces;
using NetObserver.TracerouteUtility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver;
using WinObserver.Model;

namespace Apparat.Helpers
{
    public class HostRouteHelper : INotifyPropertyChanged, IHostRouteHelper
    {
        public IEnumerable<string> CreateNewRouteCollection(string hostname)
        {
            IEnumerable<string> newCollectionRoute = new Traceroute().GetIpTraceRoute(hostname);
            return newCollectionRoute;
        }

        public void FillingNewRoute(ref ObservableCollection<TracertModel> hostnameCollection, IEnumerable<string> collectionIpAddres)
        {
            int countHostname = 1;
            ObservableCollection<TracertModel> resultOfFillingCollectionRoute = new ObservableCollection<TracertModel>();
            ObservableCollection<TracertModel> tmphostnameCollection = hostnameCollection; // variable to use in ref

            foreach (string addres in collectionIpAddres)
            {
                App.Current.Dispatcher.BeginInvoke((System.Action)delegate
                {
                    tmphostnameCollection.Add(new TracertModel { NumberHostname = countHostname, Hostname = addres });
                    countHostname++;
                    OnPropertyChanged();
                });
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
