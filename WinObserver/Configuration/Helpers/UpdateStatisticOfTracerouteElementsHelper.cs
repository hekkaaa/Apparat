using Apparat.Helpers.Interfaces;
using NetObserver.PingUtility;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using WinObserver.Algorithms;
using WinObserver.Model;

namespace Apparat.Helpers
{
    public class UpdateStatisticOfTracerouteElementsHelper : IUpdateStatisticOfTracerouteElementsHelper
    {
        private int _timeout = 1500;
        private IcmpRequestSender _icmpUtility = new IcmpRequestSender();
        private PingOptions _option = new PingOptions() { DontFragment = true };
        private byte[] _buffer = new byte[32];

        public ObservableCollection<TracertModel> Update(ObservableCollection<TracertModel> externalCollection, int buffer)
        {
            _buffer = ConvertIntToBufferFormat(buffer);
            foreach (TracertModel itemCollection in externalCollection.ToList())
            {
                PingReply tmpResult = _icmpUtility.RequestIcmp(itemCollection.Hostname, _timeout, _buffer, _option);
                TracertModel tempItemCollection = itemCollection; // variable for further work with ref methods

                if (tmpResult.Status == IPStatus.Success)
                {
                    tempItemCollection.LastDelay = (int)tmpResult.RoundtripTime;
                    tempItemCollection.ArhivePingList!.Add((int)tmpResult.RoundtripTime);
                    DataGridStatisticAlgorithm.UpdateMinMaxPing(ref tempItemCollection, (int)tmpResult.RoundtripTime);
                    DataGridStatisticAlgorithm.MiddlePing(ref tempItemCollection);
                    tempItemCollection.CounterPacket++;
                    tempItemCollection.ArhiveStatusRequestPacket.Add(0);
                    tempItemCollection.ArhiveStateValuePercentLossPacket.Add(tempItemCollection.PercentLossPacket); // Add arhive % loss pacet in now time.
                }
                else
                {
                    tempItemCollection.LastDelay = 0;
                    tempItemCollection.CounterPacket++;
                    tempItemCollection.CounterLossPacket++;
                    tempItemCollection.ArhiveStatusRequestPacket.Add(1);
                    tempItemCollection.ArhiveStateValuePercentLossPacket.Add(tempItemCollection.PercentLossPacket); // Add arhive % loss pacet in now time.
                }

                itemCollection.PercentLossPacket = DataGridStatisticAlgorithm.CalculationofLossesOnElementsHost(tempItemCollection);
            }

            return externalCollection;
        }

        /// <summary>
        /// Convert from Int forman for byte[]
        /// </summary>
        /// <returns></returns>
        private byte[] ConvertIntToBufferFormat(int oldFormatBuffer)
        {
            if (oldFormatBuffer <= 0)
            {
                return new byte[1];
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                string data = String.Empty;

                for (int i = 0; i < oldFormatBuffer; i++)
                {
                    sb.AppendFormat("a");
                }

                data = sb.ToString();

                byte[] buffer = Encoding.ASCII.GetBytes(data);

                return buffer;
            }
        }

    }
}
