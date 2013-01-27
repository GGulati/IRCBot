using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class ModifyAccount : Command
    {
        public ModifyAccount(Administrator plugin) : base(plugin, "Account.Modify") { }

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
                    plugin.Host.Reply(data, SendType.Message, "No account specified");
                    return;
                }
            }

            string account = data.Message.Substring(start, end - start);

            start = end + 1;
            end = data.Message.IndexOf(' ', start);
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
            if (plugin.GetCommandsAtGroup(group) == null)
            {
                plugin.Host.Reply(data, SendType.Message, "No account group named " + group);
                return;
            }

            plugin.AddAccount(account, group);
            plugin.Host.Reply(data, SendType.Message, string.Format("Account '{0}' successfully modified", account, group));
        }
    }
}
