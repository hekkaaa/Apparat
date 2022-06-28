using System.ComponentModel;
using System.Runtime.CompilerServices;
using WinObserver.ViewModel;

namespace Apparat.Services.Interface
{
    public interface ITracertService
    {
        event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string prop = "");
        void StartTraceroute(string hostname);
        void StopTraceroute();
    }
}