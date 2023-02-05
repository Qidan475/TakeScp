using Exiled.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeScp.API;
using UnityEngine;

namespace TakeScp
{
    internal class FpcData : IData
    {
        private string _suicider;

        private RoleTypeId _role;
        private float _health;
        private float _maxHp;
        private float _maxAhp;

        public FpcData(RoleTypeId role, string userIdOfSuicider)
        {
            _role = role;
            _suicider = userIdOfSuicider;
        }

        public FpcData(Player player)
        {
            _role = player.Role.Type;
            _health = player.Health;
            _maxHp = player.MaxHealth;
            _maxAhp = player.MaxArtificialHealth;
        }

        public void ApplyRole(Player player)
        {
            player.Role.Set(_role, Exiled.API.Enums.SpawnReason.ForceClass, RoleSpawnFlags.All);
            if (_suicider == null)
            {
                Timing.CallDelayed(0.3f, () =>
                {
                    player.Health = _health;
                    player.MaxHealth = _maxHp;
                    player.MaxArtificialHealth = _maxAhp;
                });
            }
        }

        public string GetDisplayableName() => $"<color=red>{_role}</color>";

        public bool CanUseCmd(Player player, out string reason)
        {
            reason = "обойдёшься";
            return player.UserId != _suicider;
        }
    }
}
