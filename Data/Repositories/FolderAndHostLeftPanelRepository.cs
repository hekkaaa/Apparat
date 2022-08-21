using Data.Connect;
using Data.Entities;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class FolderAndHostLeftPanelRepository : IFolderAndHostLeftPanelRepository
    {
        private ApplicationSettingFolderandHostContext _context;

        public FolderAndHostLeftPanelRepository(ApplicationSettingFolderandHostContext context)
        {
            _context = context;
        }

        public int SaveStateFolder(FolderState newFolder)
        {
            _context.Folders.Add(newFolder);
            _context.SaveChanges();
            return newFolder.FolderId;
        }

        public List<FolderState> LoadStateFolders()
        {
            var result = _context.Folders.ToList();
            return result;
        }

        public bool DeleteFolder(FolderState delefolder)
        {
            var resDelTracert = DeleteHostTracert(delefolder.Host_id);

            _context.Folders.Remove(delefolder);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteHostTracert(ICollection<StateObjectTraceroute> items)
        {
            _context.TracerouteHost.RemoveRange(items);
            _context.SaveChanges();
            return true;
        }
    }

}
