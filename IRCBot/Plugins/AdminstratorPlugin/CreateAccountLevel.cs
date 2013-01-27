using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class CreateAccountLevel : Command
    {
        public CreateAccountLevel(Administrator plugin) : base(plugin, "Level.Create") { }

        public override void Execute(IrcMessageData data)
        {
        }
    }
}
