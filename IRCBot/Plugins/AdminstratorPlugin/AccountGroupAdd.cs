using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class AccountGroupAdd : Command
    {
        public AccountGroupAdd(Administrator plugin) : base(plugin, "AccountGroup.Add") { }

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

            string name = data.Message.Substring(start, end - start);
            string[] names = name.Split(',');
            Command[] toAdd = new Command[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                toAdd[i] = Command.GetCommand(names[i]);
                if (toAdd[i] == null)
                {
                    plugin.Host.Reply(data, SendType.Message, "No command named " + names[i]);
                    return;
                }
            }

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
            var commands = plugin.GetCommandsAtGroup(group);
            if (commands == null)
            {
                plugin.Host.Reply(data, SendType.Message, "No account group named " + group);
                return;
            }

            var newCommands = new Command[commands.Length + toAdd.Length];
            Array.Copy(commands, newCommands, commands.Length);
            for (int i = 0; i < toAdd.Length; i++)
                newCommands[commands.Length + i] = toAdd[i];

            plugin.AddAccountGroup(group, newCommands);
            plugin.Host.Reply(data, SendType.Message, "Commands successfully added to " + group);
        }
    }
}
