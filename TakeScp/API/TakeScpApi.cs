using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static NineTailedFoxAnnouncer;

namespace TakeScp.API
{
    public static class TakeScpApi
    {
        internal static CoroutineHandle Announcer;
        internal static List<TempScpData> AvailableScps = new List<TempScpData>();
        internal static Dictionary<int, string> TakenScps = new Dictionary<int, string>();
        internal static int CurrentUsages;

        public static void AnnounceFreeScp(IData scpData)
        {
            if (Warhead.IsDetonated || Round.IsEnded || CurrentUsages >= MainThing.Instance.Config.MaxUsagesPerRound)
                return;

            int magicNumber;
            do
            {
                magicNumber = UnityEngine.Random.Range(10000, 100000);
            } while (AvailableScps.Any(x => x.MagicNumber == magicNumber));

            AvailableScps.Add(new TempScpData(magicNumber, 0, scpData));
            CurrentUsages++;

            if (!Timing.IsRunning(Announcer))
                Announcer = Timing.RunCoroutine(LeAnnouncer());
        }

        private static IEnumerator<float> LeAnnouncer()
        {
            while (AvailableScps.Count > 0)
            {
                var scpToAnnounce = AvailableScps[0];
                Map.ClearBroadcasts();
                foreach (var ply in Player.List)
                {
                    ply.SendFormattedBroadcast(3, MainThing.Instance.Config.MsgWhenScpLeft, new Dictionary<string, string>()
                    {
                        ["%scp%"] = scpToAnnounce.ScpData.GetDisplayableName(),
                        ["%magic_number%"] = scpToAnnounce.MagicNumber.ToString()
                    });
                }

                int waitSec = 2;
                for (int i = 0; i < waitSec * Server.Tps; i += 2)
                {
                    yield return Timing.WaitForOneFrame;
                    yield return Timing.WaitForOneFrame;
                }

                scpToAnnounce.SecondsWaited += waitSec;
                if (scpToAnnounce.SecondsWaited >= MainThing.Instance.Config.WaitTime)
                    AvailableScps.Remove(scpToAnnounce);
            }
        }
    }
}
