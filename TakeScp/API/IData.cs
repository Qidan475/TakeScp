using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeScp.API
{
    public interface IData
    {
        void ApplyRole(Player player);

        string GetDisplayableName();

        bool CanUseCmd(Player player, out string reason);
    }
}
