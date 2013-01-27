using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCBot
{
    public struct Config
    {
        public readonly string Name, IRCServer, Nick;
        public readonly int Port;

        public Config(string name, string nick, string server, int port)
        {
            Name = name;
            Nick = nick;
            IRCServer = server;
            Port = port;
        }
    }
}
