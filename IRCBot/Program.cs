using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IRCBot
{
    static class Program
    {
        static void Main()
        {
            string[] lines = System.IO.File.ReadAllLines("config.cfg.txt");
            string realname = lines[0];
            string nick = lines[1];
            string server = lines[2];
            int port = int.Parse(lines[3]);

            Config cfg = new Config(realname, nick, server, port);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(cfg));
        }
    }
}
