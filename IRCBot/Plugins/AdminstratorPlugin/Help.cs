using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class Help : Command
    {
        public Help(Administrator plugin) : base(plugin, "Help", "Type 'help' for a list of commands or 'help [command]' for help with a particular command") { }

        public override void Execute(IrcMessageData data)
        {
            int start = data.Message.IndexOf(Name, StringComparison.CurrentCultureIgnoreCase) + Name.Length + 1;
            if (data.Message.Length <= start)
            {
				StringBuilder msg = new StringBuilder("Supported commands: ");
				for (int i = 0; i < Command.AllCommands.Length; i++)
				{
					msg.Append(Command.AllCommands[i].Name);
					if (i + 1 < Command.AllCommands.Length)
						msg.Append(", ");
				}
				plugin.Host.Reply(data, SendType.Message, msg.ToString());
                return;
            }
			
            int end = data.Message.IndexOf(' ', start);
            if (end <= start)
            {
                end = data.Message.Length;
                if (end <= start)
                {
					StringBuilder msg = new StringBuilder("Supported commands: ");
					for (int i = 0; i < Command.AllCommands.Length; i++)
					{
						msg.Append(Command.AllCommands[i].Name);
						if (i + 1 < Command.AllCommands.Length)
							msg.Append(", ");
					}
					plugin.Host.Reply(data, SendType.Message, msg.ToString());
                    return;
                }
            }

            string command = data.Message.Substring(start, end - start);
            var cmd = Command.GetCommand(command);
			if (cmd == null)
				plugin.Host.Reply(data, SendType.Message, "No such command");
			else
				plugin.Host.Reply(data, SendType.Message, cmd.Help);
        }
    }
}