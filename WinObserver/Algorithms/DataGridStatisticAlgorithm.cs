using WinObserver.Model;

namespace WinObserver.Algorithms
{
    public static class DataGridStatisticAlgorithm
    {
        public static int CalculationofLossesOnElementsHost(TracertModel TraceElement)
        {
            int result = TraceElement.CounterLossPacket * 100 / TraceElement.CounterPacket;
            return result;
        }

        public static void UpdateMinMaxPing(ref TracertModel ip, int ping)
        {
            if (ip.MinPing == 0) ip.MinPing = ping;
            if (ip.MaxPing == 0) ip.MaxPing = ping;
            else
            {
                if (ip.MinPing > ping)
                {
                    ip.MinPing = ping;
                }
                if (ip.MaxPing < ping)
                {
                    ip.MaxPing = ping;
                }
            }
        }

        public static void MiddlePing(ref TracertModel item)
        {
            int result = 0;
            int countList = item.ArhivePingList!.Count;

            for (int i = 0; i < countList; i++)
            {
                result += item.ArhivePingList[i];
            }

            item.MiddlePing = result / countList;
        }
    }
}
