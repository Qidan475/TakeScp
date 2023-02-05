using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using RemoteAdmin;
using TakeScp.API;
using UnityEngine;

namespace TakeScp.Cmds
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class TakeScpCmd: ICommand
    {
        public string Command => "takescp";

        public string[] Aliases { get; } = new string[] { "ефлуысз" };

        public string Description => String.Empty;

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = String.Empty;
            if (!(Player.Get(sender) is Player player))
                return false;

            if (arguments.Count < 1)
            {
                response = "Введите магическое число";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int magicNumber))
            {
                response = "Введите корректное магическое число";
                return false;
            }

            if (TakeScpApi.TakenScps.TryGetValue(magicNumber, out string nick))
            {
                response = $"Вы не успели - эту роль забрал \"{nick}\"";
                return false;
            }

            if (!(TakeScpApi.AvailableScps.Find(x => x.MagicNumber == magicNumber) is TempScpData data))
            {
                response = "Неверное магическое число (не успели ввести команду вовремя?)";
                return false;
            }

            if (data.SecondsWaited >= MainThing.Instance.Config.WaitTime)
            {
                response = "Время истекло: не успел никто";
                return false;
            }

            if (!data.ScpData.CanUseCmd(player, out string reason))
            {
                response = $"Вы не можете использовать команду в данный момент по причине: {reason}";
                return false;
            }

            if (Warhead.IsDetonated)
            {
                response = "Боеголовка уже взорвана, так что нет";
                return false;
            }

            TakeScpApi.TakenScps.Add(magicNumber, player.Nickname);
            TakeScpApi.AvailableScps.Remove(data);
            Map.ClearBroadcasts();
            data.ScpData.ApplyRole(player);
            response = "Удачно отреспавнено";
            return true;
        }
    }
}
