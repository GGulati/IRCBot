using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChatterBotAPI;

namespace IRCBot.Plugins
{
    public partial class ChannelManager : Plugin
    {
        List<string> channels;

        public ChannelManager()
        {
            channels = new List<string>();
        }

        public bool TryJoinChannel(string channel)
        {
            channel = channel.ToLower();

            bool success = Host.TryJoinChannel(channel);
            if (success && !channels.Contains(channel))
                channels.Add(channel);

            return success;
        }
    }
}
