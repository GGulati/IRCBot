using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class AccountGroupPrintAll : Command
    {
        public AccountGroupPrintAll(Administrator plugin) : base(plugin, "AccountGroup.PrintAll", "Prints all AccountGroups") { }

        public override void Execute(IrcMessageData data)
        {
			var groups = plugin.GetAccountGroups();
			StringBuilder msg = new StringBuilder("Existing groups: ");
			for (int i = 0; i < groups.Length; i++)
			{
				msg.Append(groups[i]);
				if (i + 1 < groups.Length)
					msg.Append(", ");
			}
			
			plugin.Host.Reply(data, SendType.Message, msg.ToString());
        }
    }
}