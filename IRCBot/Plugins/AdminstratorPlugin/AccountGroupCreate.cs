using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class AccountGroupCreate : Command
    {
        public AccountGroupCreate(Administrator plugin) : base(plugin, "AccountGroup.Create") { }

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
                    plugin.Host.Reply(data, SendType.Message, "No such command");
                    return;
                }
            }

            string group = data.Message.Substring(start, end - start);
            if (plugin.GetCommandsAtGroup(group) != null)
            {
                plugin.Host.Reply(data, SendType.Message, "An account group '" + group + "' already exists");
                return;
            }

            plugin.AddAccountGroup(group, new Command[] { });
            plugin.Host.Reply(data, SendType.Message, "Account group " + group + " successfully created");
        }
    }
}
