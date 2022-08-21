using Apparat.Services.Interfaces;
using Apparat.ViewModel;
using Castle.Core.Logging;
using Data.Entities;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat.Services
{
    public class SaveStateFolderService : ISaveStateFolderService
    {
        private readonly ILogger<SaveStateFolderService> _logger;
        private IFolderAndHostLeftPanelRepository _repository;

        public SaveStateFolderService(IFolderAndHostLeftPanelRepository repository, ILogger<SaveStateFolderService> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public bool SaveStateFolder(ObservableCollection<ExplorerViewModel> collectionFolder)
        {
            foreach (ExplorerViewModel itemFolder in collectionFolder)
            {
                FolderState tmpFolderState = new FolderState();
                List<StateObjectTraceroute> tmpCollectionObjHost = new();

                tmpFolderState.Name = itemFolder.FolderName;

                if(itemFolder.HostVMCollection != null) { 

                    foreach (var itemVM in itemFolder.HostVMCollection)
                    {
                        tmpCollectionObjHost.Add(new StateObjectTraceroute() { Hostname = itemVM.HostnameView });
                    }

                    tmpFolderState.Host_id = tmpCollectionObjHost;

                    _repository.SaveStateFolder(tmpFolderState);
                }
                else
                {
                    _repository.SaveStateFolder(tmpFolderState);
                }
            }

            _logger.LogWarning("Save State folder from Db Success");

            return true;
        }

        public ObservableCollection<ExplorerViewModel> LoadStateFolder()
        {
            var loadResult = _repository.LoadStateFolders();
            var tmpCollectionFolder = new ObservableCollection<ExplorerViewModel>();
            int count = 0;

            foreach (FolderState itemHostId in loadResult)
            {   
                if(itemHostId.Name == "Default")
                {
                    tmpCollectionFolder.Add(new ExplorerViewModel(true)
                    {
                        FolderName = itemHostId.Name,
                        IsNewCreateObj = false,
                    });
                }
                else
                {
                    tmpCollectionFolder.Add(new ExplorerViewModel(false)
                    {
                        FolderName = itemHostId.Name,
                        IsNewCreateObj = false,
                        VisibleIconMoreAction = "Visible",
                    });
                }

                ObservableCollection<HostViewModel> tmpCollectionHostname = new ObservableCollection<HostViewModel>();
                
                if(itemHostId.Host_id != null) 
                {
                    foreach (StateObjectTraceroute item in itemHostId.Host_id)
                    {
                        var ttts = item;
                        tmpCollectionHostname.Add(new HostViewModel(_logger) { HostnameView = item.Hostname });
                    }
                    tmpCollectionFolder[count].HostVMCollection = tmpCollectionHostname;
                }

                count++;
            }
            return tmpCollectionFolder;
        }

        public bool DeleteAllFolder()
        {
            List<FolderState> loadCollectionInDb = _repository.LoadStateFolders();

            foreach (var item in loadCollectionInDb)
            {
                _repository.DeleteHostTracert(item.Host_id);
                _repository.DeleteFolder(item);
            }

            return true;
        }
    }
}
