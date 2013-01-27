using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class AccountGroupPrint : Command
    {
        public AccountGroupPrint(Administrator plugin) : base(plugin, "AccountGroup.Print", "Type 'AccountGroup.Print [group]' for a list of the commands that its members can use") { }

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
            var commands = plugin.GetCommandsAtGroup(group);
            if (commands == null)
            {
                plugin.Host.Reply(data, SendType.Message, "No account group named " + group);
                return;
            }
			
			StringBuilder msg = new StringBuilder("Allowed commands: ");
			for (int i = 0; i < commands.Length; i++)
			{
				msg.Append(commands[i].Name);
				if (i + 1 < commands.Length)
					msg.Append(", ");
			}
			
			plugin.Host.Reply(data, SendType.Message, msg.ToString());
        }
    }
}
