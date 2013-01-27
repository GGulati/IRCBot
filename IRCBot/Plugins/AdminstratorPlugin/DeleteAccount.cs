using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class DeleteAccount : Command
    {
        public DeleteAccount(Administrator plugin) : base(plugin, "Account.Delete") { }

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

            plugin.RemoveAccount(account);
            plugin.Host.Reply(data, SendType.Message, "Account deleted");
        }
    }
}
