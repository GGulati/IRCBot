using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChatterBotAPI;

namespace IRCBot.Plugins
{
    public partial class ChannelManager : Plugin
    {
        protected override void OnAdded()
        {
            Host.OnMessageRecieved += Host_OnMessageRecieved;

            foreach (string channel in channels)
                TryJoinChannel(channel);
        }

        protected override void OnRemoved()
        {
            Host.OnMessageRecieved -= Host_OnMessageRecieved;
        }

        private void Host_OnMessageRecieved(object sender, Meebey.SmartIrc4net.IrcEventArgs e)
        {
            if (e.Data.Message == null)
                return;

            const string INVITE = "bot.join";
            if (e.Data.Message.Length <= INVITE.Length || !e.Data.Message.StartsWith(INVITE, StringComparison.CurrentCultureIgnoreCase))
                return;

            string channel = e.Data.Message.Substring(INVITE.Length).Trim();
            if (!TryJoinChannel(channel))
                Host.Reply(e.Data, Meebey.SmartIrc4net.SendType.Message, "No such channel");
        }

        public override void Save(System.Xml.XmlWriter writer)
        {
            foreach (string channel in channels)
                writer.WriteElementString("Channel", channel);
        }

        public override void Load(System.Xml.XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.MoveToContent() == System.Xml.XmlNodeType.Element)
                {
                    if (reader.Name == "Channel")
                    {
                        channels.Add(reader.ReadString());
                    }
                }
            }
        }
    }
}
