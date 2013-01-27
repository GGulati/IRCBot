using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

using Meebey.SmartIrc4net;

namespace IRCBot
{
    public static class PluginHostExt
    {
        public static void Reply(this IPluginHost host, IrcMessageData replyTo, SendType type, string message)
        {
            host.SendMessage(type, replyTo.Channel ?? replyTo.Ident, replyTo.Channel == null ? message : string.Concat(replyTo.Nick, ": ", message));
        }

        public static void Save(this IPluginHost host, string filepath)
        {
            var plugins = host.Plugins;

            using (XmlWriter writer = XmlWriter.Create(filepath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("PluginHost");

                foreach (var plugin in plugins)
                {
                    writer.WriteStartElement("Plugin");
                    writer.WriteAttributeString("type", plugin.GetType().FullName);

                    plugin.Save(writer);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public static void Load(this IPluginHost host, string filepath, params Assembly[] pluginAssemblies)
        {
            host.ClearPlugins();

            using (XmlReader reader = XmlReader.Create(filepath))
            {
                bool started = false;
                while (reader.Read())
                {
                    if (reader.MoveToContent() == XmlNodeType.Element)
                    {
                        if (reader.Name == "PluginHost")
                        {
                            if (started)
                                throw new Exception("Parent nodes cannot be nested");

                            started = true;
                        }
                        else if (!started)
                            throw new Exception("A parent node of type 'PluginHost' must be included");
                        else if (reader.Name == "Plugin")
                        {
                            string type = reader.GetAttribute("type");
                            Plugin plugin = GetPlugin(type, pluginAssemblies);
                            if (plugin == null)
                                throw new DependencyNotFoundException("Plugin " + type + " not found");

                            plugin.Load(reader.ReadSubtree());
                            host.AddPlugin(plugin);
                        }
                        else
                            throw new NotSupportedException(reader.Name);
                    }
                }
            }
        }

        private static Plugin GetPlugin(string fullname, Assembly[] assemblies)
        {
            foreach (Assembly asm in assemblies)
            {
                Type type = asm.GetType(fullname, false);

                if (type != null)
                    return type.GetConstructor(new Type[] { }).Invoke(new object[] { }) as Plugin;
            }

            throw new DependencyNotFoundException("Plugin " + fullname + " cannot be initialized.");
        }
    }
}
