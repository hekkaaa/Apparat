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
        public void AnalystLossIcmpGrid(IApplicationViewModel test, ILogger<IApplicationViewModel> logger)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Task.Delay(5000).Wait();
                    logger.LogWarning("Update View Datagrid");
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
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical($"Error {ex.Message}");
                        continue;
                    }
                }
            });
        }
    }
}
