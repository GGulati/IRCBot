using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public class AccountGroupRemove : Command
    {
        public AccountGroupRemove(Administrator plugin) : base(plugin, "AccountGroup.Remove") { }

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
            Command[] toRemove = new Command[names.Length];
            for (int i = 0; i < names.Length; i++)
            {
                toRemove[i] = Command.GetCommand(names[i]);
                if (toRemove[i] == null)
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

            List<Command> newCommands = new List<Command>(commands);
            for (int i = 0; i < newCommands.Count; i++)
            {
                foreach (Command cmd in toRemove)
                {
                    if (newCommands[i] == cmd)
                    {
                        newCommands.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            plugin.AddAccountGroup(group, newCommands.ToArray());
            plugin.Host.Reply(data, SendType.Message, "Commands successfully removed from " + group);
        }
    }
}
