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

        public int AddNewHost(FolderState newFolder)
        {
            _context.Folders.Add(newFolder);
            _context.SaveChanges();
            return newFolder.FolderId;
        }

        //public List<HistoryHost> GetLastFiveHostname()
        //{
        //    return _context.History.OrderByDescending(x => x).Take(5).ToList();
        //}

        //public bool ClearAllTable()
        //{
        //    _context.History.RemoveRange(_context.History);
        //    _context.SaveChanges();
        //    return true;
        //}

        //public bool DeleteHostname(HistoryHost removeHostname)
        //{
        //    _context.History.Remove(removeHostname);
        //    int t = _context.SaveChanges();
        //    if (t > 0) return true;
        //    else return false;
        //}
    }

}
