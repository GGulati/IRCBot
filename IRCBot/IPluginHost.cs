using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;
using System.Collections.ObjectModel;

namespace IRCBot
{
    public interface IPluginHost
    {
        event IrcEventHandler OnMessageRecieved;

        void Quit();

        bool TryJoinChannel(string channel);

        void SendMessage(SendType type, string destination, string message);

        void AddPlugin(Plugin plugin);

        void RemovePlugin(Plugin plugin);

        void ClearPlugins();

        T GetPlugin<T>() where T : Plugin;

        ReadOnlyCollection<Plugin> Plugins { get; }

        Config Config { get; }
    }
}
