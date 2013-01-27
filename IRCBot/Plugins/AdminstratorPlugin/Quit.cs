using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class Quit : Command
    {
        public Quit(Administrator plugin) : base(plugin, "Bot.Quit") { }

        public override void Execute(IrcMessageData data)
        {
            plugin.Host.Reply(data, SendType.Message, "Goodbye");
            plugin.Host.Quit();
        }
    }
}
