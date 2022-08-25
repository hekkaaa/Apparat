using Apparat.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinObserver.Model;
using WinObserver.ViewModel;

namespace Apparat.Algorithms
{
    public class LossColorAnl
    {
        private const int _delayTimeoutDeamon = 5000;

        public void AnalystLossIcmpGrid(IApplicationViewModel test, ILogger logger)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(_delayTimeoutDeamon).Wait();
                    logger.LogWarning("Start update View Datagrid");
                    try
                    {
                        ObservableCollection<ExplorerViewModel> FolderCollection = test.CollectionFoldersInExplorer;
                        foreach (ExplorerViewModel folder in FolderCollection)
                        {
                            ObservableCollection<HostViewModel> HostViewModelCollection = folder.HostVMCollection;

                            if (HostViewModelCollection is null)
                            {
                                break;
                            }

                            foreach (var host in HostViewModelCollection)
                            {
                                // Ignore not working tracerts.
                                if (host.StatusWorkDataGrid == false)
                                {
                                    break;
                                }

                                ReadOnlyObservableCollection<TracertModel> hostEementRowInDataGrid = host.TracertObject!;
                                if (hostEementRowInDataGrid is null)
                                {
                                    break;
                                }
                                else
                                {
                                    foreach (TracertModel item in hostEementRowInDataGrid)
                                    {
                                        if (item.PercentLossPacket <= 1)
                                        {
                                            item.ColorLossView = "Black";
                                        }
                                        if (item.PercentLossPacket >= 2)
                                        {
                                            item.ColorLossView = "Peru";
                                        }
                                        if (item.PercentLossPacket >= 5)
                                        {
                                            item.ColorLossView = "Red";
                                        }
                                    }
                                }
                            }
                        }
                        logger.LogWarning("Successful end update View Datagrid");
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical($"Error updating daemon data {ex.Message}");
                        continue;
                    }
                }
            });
        }
    }
}
