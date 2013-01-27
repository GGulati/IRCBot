using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChatterBotAPI;

namespace IRCBot.Plugins
{
    public partial class Chatbot : Plugin
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
            if (e.Data.Message == null)
                return;

            string remark;
            if (e.Data.Channel != null)
            {
                if (e.Data.Message.Length <= Host.Config.Nick.Length || !e.Data.Message.StartsWith(Host.Config.Nick, StringComparison.CurrentCultureIgnoreCase))
                    return;
                remark = e.Data.Message.Substring(Host.Config.Nick.Length + 1).Trim();
            }
            else
                remark = e.Data.Message.Trim();

            if (remark.Length == 0)
                return;

            string user = e.Data.Nick;

            /*const string BOT_CHANGE_TRIGGER = "bot.change";
            if (remark.StartsWith(BOT_CHANGE_TRIGGER, StringComparison.CurrentCultureIgnoreCase) && remark.Length > BOT_CHANGE_TRIGGER.Length)
            {
                string bottype = remark.Substring(BOT_CHANGE_TRIGGER.Length).Trim();

                ChatterBotType type;
                if (TryConvert(bottype, out type))
                {
                    ChangeBot(user, type);
                    Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "Your personal bot type changed to " + type.ToString());
                }
                else
                    Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "Unsupported or unknown bot type");

                return;
            }*/

            const string HELP_TRIGGER = "bot.help";
            if (remark.StartsWith(HELP_TRIGGER, StringComparison.CurrentCultureIgnoreCase))
            {
                Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "Talk to me by saying '" + Host.Config.Nick + ": text'");
                /*Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "Change your personal bot type by typing '" + BOT_CHANGE_TRIGGER + " typename'");

                StringBuilder message = new StringBuilder("Potential bots - ");

                var bots = Enum.GetNames(typeof(ChatterBotType));

                for (int i = 0; i < bots.Length; i++)
                {
                    message.Append(bots[i]);
                    if (i + 1 < bots.Length)
                        message.Append(", ");
                }
                
                Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, message.ToString());*/
                return;
            }

            Chat(e.Data, user, remark);
        }

        public override void Save(System.Xml.XmlWriter writer)
        {
            foreach (var pair in conversations)
            {
                writer.WriteStartElement("ConversationState");
                writer.WriteAttributeString("user", pair.Key);
                writer.WriteAttributeString("type", pair.Value.Type.ToString());
                writer.WriteEndElement();
            }
        }

        public override void Load(System.Xml.XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.MoveToContent() == System.Xml.XmlNodeType.Element)
                {
                    if (reader.Name == "ConversationState")
                    {
                        string user = reader.GetAttribute("user");
                        var state = new ConversationState(this, (ChatterBotType)Enum.Parse(typeof(ChatterBotType), reader.GetAttribute("type")));
                        conversations.Add(user, state);
                    }
                }
            }
        }
    }
}
