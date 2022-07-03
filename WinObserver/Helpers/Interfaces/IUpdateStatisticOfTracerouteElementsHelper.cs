using System.Collections.ObjectModel;
using WinObserver.Model;

namespace Apparat.Helpers.Interfaces
{
    public interface IUpdateStatisticOfTracerouteElementsHelper
    {
        ObservableCollection<TracertModel> Update(ObservableCollection<TracertModel> externalCollection);
    }
}