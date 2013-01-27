using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IRCBot.Plugins.AdminstratorPlugin;

namespace IRCBot.Plugins
{
    public partial class Administrator : Plugin
    {
        protected override void OnAdded()
        {
            Host.OnMessageRecieved += Host_OnMessageRecieved;
        }

        protected override void OnRemoved()
        {
            Host.OnMessageRecieved -= Host_OnMessageRecieved;
        }

        private void Host_OnMessageRecieved(object sender, Meebey.SmartIrc4net.IrcEventArgs e)
        {
            string user = e.Data.Nick.ToLower();
            if (!accounts.ContainsKey(user))
            {
                if (accountGroups.ContainsKey(DEFAULT_USER_GROUP_NAME))
                    accounts.Add(user, DEFAULT_USER_GROUP_NAME);
                else
                    return;
            }
            if (!accountGroups.ContainsKey(accounts[user]))
            {
                Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "Your account does not belong to a valid account group.");
                RemoveAccount(user);
                return;
            }

            Command[] commands = Command.AllCommands;
            int index = -1;

            int cmdEnd = e.Data.Message.IndexOf(' ');
            if (cmdEnd < 1)
            {
                cmdEnd = e.Data.Message.Length;
                if (cmdEnd < 1)
                    return;
            }
            string command = e.Data.Message.Substring(0, cmdEnd);

            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i].Name.Equals(command, StringComparison.CurrentCultureIgnoreCase))
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
                return;

            foreach (var cmd in accountGroups[accounts[user]])
            {
                if (cmd == commands[index])
                {
                    try
                    {
                        commands[index].Execute(e.Data);
                    }
                    catch
                    {
                        Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "Command execution failed.");
                    }
                    return;
                }
            }

            Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "Your account does not have access to this command.");
            return;
        }

        public override void Save(System.Xml.XmlWriter writer)
        {
            foreach (var pair in accountGroups)
            {
                writer.WriteStartElement("AccountGroup");
                writer.WriteAttributeString("name", pair.Key);

                foreach (var command in pair.Value)
                {
                    writer.WriteElementString("Command", command.Name);
                }

                writer.WriteEndElement();
            }

            foreach (var pair in accounts)
            {
                writer.WriteStartElement("Account");
                writer.WriteAttributeString("group", pair.Value);
                writer.WriteString(pair.Key);
                writer.WriteEndElement();
            }
        }

        public override void Load(System.Xml.XmlReader reader)
        {
            string accountGroupNameBuffer = null;
            List<Command> commandBuffer = new List<Command>();

            while (reader.Read())
            {
                var content = reader.MoveToContent();
                if (content == System.Xml.XmlNodeType.Element)
                {
                    if (reader.Name == "AccountGroup")
                    {
                        if (accountGroupNameBuffer != null)
                            throw new Exception("AccountGroup nodes cannot be nested");

                        accountGroupNameBuffer = reader.GetAttribute("name");
                        commandBuffer.Clear();
                    }
                    else if (reader.Name == "Command")
                    {
                        if (accountGroupNameBuffer == null)
                            throw new Exception("Command nodes cannot be nested in non-AccountGroup nodes");

                        string commandName = reader.ReadString();
                        commandBuffer.Add(Command.GetCommand(commandName));
                        if (commandBuffer[commandBuffer.Count - 1] == null)
                            throw new Exception("Command '" + commandName + "' not found");
                    }
                    else if (reader.Name == "Account")
                    {
                        if (accountGroupNameBuffer != null)
                            throw new Exception("Account nodes cannot be nested in AccountGroup nodes");

                        string group = reader.GetAttribute("group");
                        string accountName = reader.ReadString();
                        accounts.Add(accountName, group);
                    }
                }
                else if (content == System.Xml.XmlNodeType.EndElement)
                {
                    if (reader.Name == "AccountGroup")
                    {
                        if (accountGroupNameBuffer != null)
                            accountGroups.Add(accountGroupNameBuffer, commandBuffer.ToArray());

                        accountGroupNameBuffer = null;
                    }
                }
            }
        }
    }
}
