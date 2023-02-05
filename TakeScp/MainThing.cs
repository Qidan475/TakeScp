using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using TakeScp.API;

namespace TakeScp
{
    public class MainThing: Plugin<PluginConfig>
    {
        public override string Name => "TakeSCP";

        public override string Prefix => "take_scp";

        public override PluginPriority Priority => PluginPriority.High;

        public override Version RequiredExiledVersion { get; } = new Version(6, 0, 0);

        internal static MainThing Instance { get; private set; }

        private EventHandlers _ev;

        public override void OnEnabled()
        {
            Instance = this;
            _ev = new EventHandlers();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _ev.Dispose();

            Instance = null;
            base.OnDisabled();
        }
    }

    public class PluginConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        public ushort WaitTime { get; set; } = 15;

        public int MaxUsagesPerRound { get; set; } = 3;

        public string MsgWhenScpLeft { get; set; } = "%scp% свободен. Нажмите Ё и введите <color=red>.takescp %magic_number%</color>";

        public TimeSpan SuicideTime { get; set; } = new TimeSpan(00, 04, 00);

        public TimeSpan AnnounceTime { get; set; } = new TimeSpan(00, 12, 00);
    }
}
