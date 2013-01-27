using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using ChatterBotAPI;

namespace IRCBot.Plugins
{
    public partial class DictionaryLookup : Plugin
    {
        class AsyncLookup
        {
            const string URBAN_DICTIONARY_LOOKUP_URL = "http://api.urbandictionary.com/v0/define?term={0}";

            DictionaryLookup plugin;
            Meebey.SmartIrc4net.IrcMessageData msg;
            string word;

            public AsyncLookup(DictionaryLookup plugin, Meebey.SmartIrc4net.IrcMessageData msg, string word)
            {
                this.plugin = plugin;
                this.msg = msg;
                this.word = word;
            }

            public static void Run(object args)
            {
                AsyncLookup lookup = args as AsyncLookup;

                try
                {
                    var request = HttpWebRequest.Create(string.Format(URBAN_DICTIONARY_LOOKUP_URL, lookup.word));
                    var response = request.GetResponse();
                    string result = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

                    const string DEF_TAG = "\"definition\":";
                    int start = result.IndexOf(DEF_TAG);
                    if (start < 0)
                    {
                        lookup.plugin.Host.Reply(lookup.msg, Meebey.SmartIrc4net.SendType.Message, "Dunno what -that- means");
                        return;
                    }

                    start = start + DEF_TAG.Length + 1;
                    int end = result.IndexOf('"', start);
                    if (end < start)
                    {
                        lookup.plugin.Host.Reply(lookup.msg, Meebey.SmartIrc4net.SendType.Message, "Dunno what -that- means");
                        return;
                    }
                    string definition = result.Substring(start, end - start);

                    lookup.plugin.Host.Reply(lookup.msg, Meebey.SmartIrc4net.SendType.Message, "definition of " + lookup.word + " - " + definition);
                }
                catch
                {
                    lookup.plugin.Host.Reply(lookup.msg, Meebey.SmartIrc4net.SendType.Message, "Dunno what -that- means");
                }
            }
        }

        public DictionaryLookup()
        {
        }

        private void Lookup(Meebey.SmartIrc4net.IrcMessageData msg, string word)
        {
            AsyncLookup lookup = new AsyncLookup(this, msg, word);
            System.Threading.ThreadPool.QueueUserWorkItem(AsyncLookup.Run, lookup);
        }
    }
}
