using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChatterBotAPI;

namespace IRCBot.Plugins
{
    public partial class Chatbot : Plugin
    {
        class ConversationState
        {
            class Reply
            {
                IPluginHost host;
                Meebey.SmartIrc4net.IrcMessageData msg;
                ChatterBotSession session;
                string query;

                public Reply(IPluginHost host, Meebey.SmartIrc4net.IrcMessageData msg, ChatterBotSession session, string query)
                {
                    this.host = host;
                    this.msg = msg;
                    this.session = session;
                    this.query = query;
                }

                public static void Think(object args)
                {
                    Reply reply = args as Reply;
                    if (reply == null)
                        return;

                    string response = reply.session.Think(reply.query);
                    reply.host.Reply(reply.msg, Meebey.SmartIrc4net.SendType.Message, response);
                }
            }

            Chatbot bot;
            ChatterBotType botType;
            ChatterBotSession session;

            public ConversationState(Chatbot bot, ChatterBotType type)
            {
                botType = type;
                session = bot.CreateSession(type);
                this.bot = bot;
            }

            public void GetResponse(Meebey.SmartIrc4net.IrcMessageData msg, string remark)
            {
                Reply reply = new Reply(bot.Host, msg, session, remark);
                System.Threading.ThreadPool.QueueUserWorkItem(Reply.Think, reply);
            }

            public ChatterBotType Type { get { return botType; } }
        }
        
        Dictionary<string, ConversationState> conversations;
        Dictionary<ChatterBotType, ChatterBot> bots;
        ChatterBotFactory factory;

        public Chatbot()
        {
            factory = new ChatterBotFactory();
            bots = new Dictionary<ChatterBotType, ChatterBot>();
            conversations = new Dictionary<string,ConversationState>();
        }

        private void Chat(Meebey.SmartIrc4net.IrcMessageData data, string user, string remark)
        {
            user = user.ToLower();

            if (!conversations.ContainsKey(user))
                conversations.Add(user, new ConversationState(this, ChatterBotType.CLEVERBOT));

            conversations[user].GetResponse(data, remark);
        }

        private void ChangeBot(string user, ChatterBotType type)
        {
            user = user.ToLower();

            if (!conversations.ContainsKey(user))
                conversations.Add(user, new ConversationState(this, type));
            else
                conversations[user] = new ConversationState(this, type);
        }

        private ChatterBotSession CreateSession(ChatterBotType type)
        {
            if (!bots.ContainsKey(type))
                bots.Add(type, factory.Create(type, "d689f7b8de347251"));

            return bots[type].CreateSession();
        }

        private bool TryConvert(string name, out ChatterBotType type)
        {
            name = name.ToLower();

            if (name == "clever" || name == "cleverbot")
            {
                type = ChatterBotType.CLEVERBOT;
                return true;
            }

            if (name == "pandora" || name == "pandorabot" || name == "pandorabots")
            {
                type = ChatterBotType.PANDORABOTS;
                return true;
            }

            type = ChatterBotType.CLEVERBOT;
            return false;
        }
    }
}
