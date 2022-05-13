using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinObserver.Model;

namespace WinObserver.Algorithms
{
    public static class DataGridStatisticAlgorithm
    {
        public static double RateLosses(double a, double b)
        {
            double c = b * 100 / a;
            return c;
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
