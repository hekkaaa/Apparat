using Apparat.ViewModel.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Apparat.Services.Interfaces
{
    public interface ITracertService
    {
        event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string prop = "");
        void StartTraceroute(string hostname, IHostViewModel error);
        void StopTraceroute();
    }
}