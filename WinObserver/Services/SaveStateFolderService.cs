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
                var test1 = new FolderState();
                test1.Name = itemFolder.FolderName;

                foreach (var itemVM in itemFolder.HostVMCollection)
                {
                    test1.Obj_id.Add(new StateObjectTraceroute { Hostname = itemVM.HostnameView }); ;
                }

                _repository.AddNewHost(test1);
            }


            _logger.LogWarning("Save State folder from Db Success");

            return true;
        }
    }
}
