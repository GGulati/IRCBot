using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class AccountGroupMembers : Command
    {
        public AccountGroupMembers(Administrator plugin) : base(plugin, "AccountGroup.Members", "Type 'AccountGroup.Members [group]' for a list of the members of that group") { }

        public override void Execute(IrcMessageData data)
        {
            int start = data.Message.IndexOf(Name, StringComparison.CurrentCultureIgnoreCase) + Name.Length + 1;
            if (data.Message.Length <= start)
            {
                plugin.Host.Reply(data, SendType.Message, "No such command");
                return;
            }
			
            int end = data.Message.IndexOf(' ', start);
            if (end <= start)
            {
                end = data.Message.Length;
                if (end <= start)
                {
                    plugin.Host.Reply(data, SendType.Message, "No account group specified");
                    return;
                }
            }

            string group = data.Message.Substring(start, end - start);
            var members = plugin.GetAccountMembers(group);
			
			StringBuilder msg = new StringBuilder("Members: ");
			for (int i = 0; i < members.Length; i++)
			{
				msg.Append(members[i]);
				if (i + 1 < members.Length)
					msg.Append(", ");
			}
			
			plugin.Host.Reply(data, SendType.Message, msg.ToString());
        }
    }
}