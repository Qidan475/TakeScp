using Exiled.API.Features;
using Exiled.API.Features.Roles;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeScp.API;

namespace TakeScp
{
    internal class Scp079Data : IData
    {
        private int _level;
        private int _exp;
        private float _maxEnergy;

        public Scp079Data(Player player)
        {
            Scp079Role scp079 = player.Role.As<Scp079Role>();
            _level = scp079.Level;
            _exp = scp079.Experience;
            _maxEnergy = scp079.MaxEnergy;
        }

        public void ApplyRole(Player player)
        {
            player.Role.Set(PlayerRoles.RoleTypeId.Scp079, Exiled.API.Enums.SpawnReason.ForceClass);
            Timing.CallDelayed(0.3f, () =>
            {
                Scp079Role scp079 = player.Role.As<Scp079Role>();
                scp079.Level = _level;
                scp079.Experience = _exp;
                scp079.MaxEnergy = _maxEnergy;
            });
        }

        public string GetDisplayableName() => "<color=red>SCP-079</color>";

        public bool CanUseCmd(Player player, out string reason)
        {
            reason = null;
            return true;
        }
    }
}
