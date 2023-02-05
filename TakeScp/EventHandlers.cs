using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;
using MEC;
using TakeScp.API;

namespace TakeScp
{
    internal class EventHandlers: IDisposable
    {
        public EventHandlers()
        {
            Exiled.Events.Handlers.Player.Destroying += OnPlayerLeft;
            Exiled.Events.Handlers.Player.Dying += OnPlayerDying;
            Exiled.Events.Handlers.Warhead.Detonated += OnWarheadBoom;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        }

        public void Dispose()
        {
            Exiled.Events.Handlers.Player.Destroying -= OnPlayerLeft;
            Exiled.Events.Handlers.Player.Dying -= OnPlayerDying;
            Exiled.Events.Handlers.Warhead.Detonated -= OnWarheadBoom;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
        }

        private void OnPlayerLeft(DestroyingEventArgs ev)
        {
            if (!(ev.Player?.IsScp ?? false) || ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp0492)
                return;

            if (ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp079 && Recontainer.Base._activatorGlass.isBroken)
                return;

            if (Round.ElapsedTime > MainThing.Instance.Config.AnnounceTime)
                return;

            IData playerData;
            if (ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp079)
                playerData = new Scp079Data(ev.Player);
            else
                playerData = new FpcData(ev.Player);
            TakeScpApi.AnnounceFreeScp(playerData);
        }

        private void OnPlayerDying(DyingEventArgs ev)
        {
            if (!(ev.Player?.IsScp ?? false) || ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp0492 || ev.Player.Role.Type == PlayerRoles.RoleTypeId.Scp079)
                return;

            if (Round.ElapsedTime > MainThing.Instance.Config.SuicideTime)
                return;

            bool isSuicide = false;
            switch (ev.DamageHandler.Type)
            {
                case Exiled.API.Enums.DamageType.Unknown:
                case Exiled.API.Enums.DamageType.Tesla:
                case Exiled.API.Enums.DamageType.Crushed:
                case Exiled.API.Enums.DamageType.FemurBreaker:
                case Exiled.API.Enums.DamageType.PocketDimension:
                    isSuicide = true;
                    break;
            }

            if (isSuicide)
                TakeScpApi.AnnounceFreeScp(new FpcData(ev.Player.Role.Type, ev.Player.UserId));
        }

        private void OnWarheadBoom()
        {
            Map.ClearBroadcasts();
        }

        private void OnRoundRestart()
        {
            TakeScpApi.CurrentUsages = 0;
            TakeScpApi.TakenScps.Clear();
            TakeScpApi.AvailableScps.Clear();
            Timing.KillCoroutines(TakeScpApi.Announcer);
        }
    }
}
