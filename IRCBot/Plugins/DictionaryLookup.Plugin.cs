using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChatterBotAPI;

namespace IRCBot.Plugins
{
    public partial class DictionaryLookup : Plugin
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

            const string LOOKUP = "!urban";
            if (!e.Data.Message.StartsWith(LOOKUP, StringComparison.CurrentCultureIgnoreCase))
                return;

            string word = e.Data.Message.Substring(LOOKUP.Length).Trim();
            if (word.Length > 0)
                Lookup(e.Data, word);
        }

        public override void Save(System.Xml.XmlWriter writer)
        {
        }

        public override void Load(System.Xml.XmlReader reader)
        {
        }
    }
}
