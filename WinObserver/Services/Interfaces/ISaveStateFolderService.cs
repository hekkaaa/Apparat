using Apparat.ViewModel;
using System.Collections.ObjectModel;

namespace Apparat.Services.Interfaces
{
    public interface ISaveStateFolderService
    {
        bool SaveStateFolder(ObservableCollection<ExplorerViewModel> collectionFolder);
        ObservableCollection<ExplorerViewModel> LoadStateFolder();
        bool DeleteAllFolder();
    }
}