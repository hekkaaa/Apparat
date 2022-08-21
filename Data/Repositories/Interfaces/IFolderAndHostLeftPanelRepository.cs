using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IFolderAndHostLeftPanelRepository
    {
        int SaveStateFolder(FolderState newFolder);
        List<FolderState> LoadStateFolders();
        bool DeleteFolder(FolderState delefolder);
        bool DeleteHostTracert(ICollection<StateObjectTraceroute> items);
    }
}