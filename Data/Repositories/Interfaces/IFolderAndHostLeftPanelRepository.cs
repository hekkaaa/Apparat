using Data.Entities;

namespace Data.Repositories.Interfaces
{
    public interface IFolderAndHostLeftPanelRepository
    {
        int AddNewHost(FolderState newFolder);
    }
}