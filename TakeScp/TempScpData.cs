using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeScp.API;

namespace TakeScp
{
    internal class TempScpData
    {
        public int MagicNumber;
        public int SecondsWaited;
        public IData ScpData;

        public TempScpData(int magicNumber, int secondsWaited, IData scpData)
        {
            MagicNumber = magicNumber;
            ScpData = scpData;
            SecondsWaited = secondsWaited;
        }
    }
}
