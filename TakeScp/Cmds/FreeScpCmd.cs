using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using TakeScp.API;

namespace TakeScp.Cmds
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class FreeScpCmd: ICommand
    {
        public string Command => "freescp";

        public string[] Aliases { get; } = new string[] { "акууысз" };

        public string Description => String.Empty;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = String.Empty;
            Player player = Player.Get(sender);
            if (player is null)
                return false;

            if (!player.CheckPermission("takescp.free"))
            {
                response = "У вас нет прав на использование этой команды";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Введите id игрока";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int pid))
            {
                response = "Укажите корректный id";
                return false;
            }

            Player foundPlayer = Player.Get(pid);
            if (foundPlayer is null)
            {
                response = "Игрок с таким id не найден";
                return false;
            }

            if (!foundPlayer.IsScp || foundPlayer.Role.Type == PlayerRoles.RoleTypeId.Scp0492)
            {
                response = "Игрок должен быть SCP";
                return false;
            }

            IData data;
            if (foundPlayer.Role.Type == PlayerRoles.RoleTypeId.Scp079)
                data = new Scp079Data(foundPlayer);
            else
                data = new FpcData(foundPlayer);

            TakeScpApi.AnnounceFreeScp(data);
            foundPlayer.Role.Set(PlayerRoles.RoleTypeId.Spectator);
            response = "Успешно";
            return true;
        }
    }
}
