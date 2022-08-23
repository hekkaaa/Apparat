using Apparat.Helpers.Interfaces;
using NetObserver.PingUtility;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using WinObserver.Algorithms;
using WinObserver.Model;

namespace Apparat.Helpers
{
    public class UpdateStatisticOfTracerouteElementsHelper : IUpdateStatisticOfTracerouteElementsHelper
    {
        private int _timeout = 1500;
        private IcmpRequestSender _icmpUtility = new IcmpRequestSender();

        public ObservableCollection<TracertModel> Update(ObservableCollection<TracertModel> externalCollection)
        {
            foreach (TracertModel itemCollection in externalCollection.ToList())
            {
                PingReply tmpResult = _icmpUtility.RequestIcmp(itemCollection.Hostname, _timeout);
                TracertModel tempItemCollection = itemCollection; // variable for further work with ref methods

                if (tmpResult.Status == IPStatus.Success)
                {
                    tempItemCollection.LastDelay = (int)tmpResult.RoundtripTime;
                    tempItemCollection.ArhivePingList!.Add((int)tmpResult.RoundtripTime);
                    DataGridStatisticAlgorithm.UpdateMinMaxPing(ref tempItemCollection, (int)tmpResult.RoundtripTime);
                    DataGridStatisticAlgorithm.MiddlePing(ref tempItemCollection);
                    tempItemCollection.CounterPacket++;
                    tempItemCollection.ArhiveStatusRequestPacket.Add(0);
                }
                else
                {
                    tempItemCollection.LastDelay = 0;
                    tempItemCollection.CounterPacket++;
                    tempItemCollection.CounterLossPacket++;
                    tempItemCollection.ArhiveStatusRequestPacket.Add(1);
                }

                itemCollection.PercentLossPacket = DataGridStatisticAlgorithm.CalculationofLossesOnElementsHost(tempItemCollection);
            }

            return externalCollection;
        }

    }
}
