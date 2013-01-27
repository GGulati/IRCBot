using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class AddCommand : Command
    {
        public AddCommand(Administrator plugin) : base(plugin, "cmd") { }

        public override void Execute(IrcMessageData data)
        {
            int start = data.Message.IndexOf(Name, StringComparison.CurrentCultureIgnoreCase) + 2;
            if (data.Message.Length <= start)
            {
                plugin.Host.Reply(data, SendType.Message, "No such command");
                return;
            }

            int end = data.Message.IndexOf(' ');
            if (end <= start)
            {
                end = data.Message.Length;
                if (end <= start)
                {
                    plugin.Host.Reply(data, SendType.Message, "No such command");
                    return;
                }
            }

            string command = data.Message.Substring(start, end - start).ToLower();

            Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            //grab and add given command
        }
    }
}
