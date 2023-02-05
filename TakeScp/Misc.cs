using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace TakeScp
{
    internal static class Misc
    {
        public static void SendFormattedBroadcast(this Player player, ushort duration, string msg, Dictionary<string, string> formatter)
        {
            string formattedMsg = msg;
            foreach (var item in formatter)
            {
                formattedMsg = formattedMsg.Replace(item.Key, item.Value);
            }

            player.Broadcast(duration, formattedMsg);
        }
    }
}
