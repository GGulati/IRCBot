using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Meebey.SmartIrc4net;

namespace IRCBot
{
    partial class MainForm : IPluginHost
    {
        public event IrcEventHandler OnMessageRecieved;

        List<Plugin> plugins = new List<Plugin>();

        void client_OnRawMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.Ident == null || e.Data.Ident == "NickServ" || e.Data.Ident == "InfoServ" || e.Data.Ident == "ChanServ" || e.Data.Nick == cfg.Nick)
                return;

            toAppend.Enqueue("\r\n>> " + e.Data.Nick + " (" + e.Data.Channel + "): " + e.Data.Message);

            if (OnMessageRecieved != null)
                OnMessageRecieved(sender, e);
        }

        public bool TryJoinChannel(string channel)
        {
            try
            {
                client.RfcJoin(channel);
                return true;
            }
            catch { return false; }
        }

        public void Quit()
        {
            client.RfcQuit("this bot is outta here");
            client.Disconnect();
            ircThread.Abort();
			
			buttonConnect.Enabled = true;
        }

        public void SendMessage(SendType type, string target, string message)
        {
            toAppend.Enqueue("\r\n<< ");
            toAppend.Enqueue(target + ": " + message);
            client.SendMessage(type, target, message);
        }

        public T GetPlugin<T>() where T : Plugin
        {
            foreach (var plugin in plugins)
            {
                if (plugin is T)
                    return plugin as T;
            }
            return null;
        }

        public void AddPlugin(Plugin plugin)
        {
            if (!VerifyPlugin(plugin))
                throw new DependencyNotFoundException(plugin.GetType().FullName + " has some dependencies missing");

            plugins.Add(plugin);
            plugin.Added(this);
        }

        public void RemovePlugin(Plugin plugin)
        {
            if (!plugins.Contains(plugin))
                return;

            plugins.Remove(plugin);
            plugin.Removed();
            foreach (var plug in plugins)
            {
                if (!VerifyPlugin(plug))
                    throw new DependencyNotFoundException(plug.GetType().FullName + " depends on " + plugin.GetType().FullName);
            }
        }

        public void ClearPlugins()
        {
            plugins.Clear();
        }

        private bool VerifyPlugin(Plugin plugin)
        {
            Type attrType = typeof(PluginDependencyAttribute);
            Type pluginType = typeof(Plugin);
            Type type = plugin.GetType();

            var attrs = type.GetCustomAttributes(attrType, true);
            foreach (var attr in attrs)
            {
                if (attr is PluginDependencyAttribute)
                {
                    var depends = (attr as PluginDependencyAttribute).Dependencies;

                    foreach (var dependsOn in depends)
                    {
                        if (!dependsOn.IsSubclassOf(pluginType))
                            throw new InvalidOperationException(dependsOn.FullName);

                        bool found = false;
                        foreach (var existing in plugins)
                        {
                            if (existing.GetType() == dependsOn || existing.GetType().IsSubclassOf(dependsOn))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            return false;
                    }
                }
            }

            return true;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<Plugin> Plugins
        {
            get { return plugins.AsReadOnly(); }
        }

        public Config Config
        {
            get { return cfg; }
        }
    }
}