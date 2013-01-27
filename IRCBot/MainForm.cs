using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Meebey.SmartIrc4net;

namespace IRCBot
{
    partial class MainForm : Form
    {
        Thread ircThread;
        IrcClient client;
        Config cfg;
        ConcurrentQueue<string> toAppend;

        public MainForm(Config config)
        {
            InitializeComponent();

            toAppend = new ConcurrentQueue<string>();

            client = new IrcClient();
            client.ActiveChannelSyncing = true;
            client.OnRawMessage += new IrcEventHandler(client_OnRawMessage);

            cfg = config;
        }

        private void timerTick_Tick(object sender, EventArgs e)
        {
            string line;
            while (!toAppend.IsEmpty)
            {
                if (toAppend.TryDequeue(out line) && line != null)
                    textBoxDebug.AppendText(line);
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            buttonConnect.Enabled = false;

            client.Connect(cfg.IRCServer, cfg.Port);
            client.Login(cfg.Nick, cfg.Name, 0, cfg.Nick);

            ircThread = new Thread(StartClient);
            ircThread.Start();

            labelName.Text = labelName.Text + client.Nickname;

            PluginHostExt.Load(this, "plugins.xml", System.Reflection.Assembly.GetExecutingAssembly());

            if (plugins.Count == 0)
            {
                IRCBot.Plugins.Administrator admin = new Plugins.Administrator();
                admin.AddAccountGroup("admin", IRCBot.Plugins.AdminstratorPlugin.Command.AllCommands);
                admin.AddAccount("garr", "admin");
                AddPlugin(admin);
                AddPlugin(new IRCBot.Plugins.ChannelManager());
                AddPlugin(new IRCBot.Plugins.Chatbot());
                AddPlugin(new IRCBot.Plugins.DictionaryLookup());
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Quit();
            }
            catch { }

            PluginHostExt.Save(this, "plugins.xml");
        }

        private void StartClient()
        {
            client.Listen();
        }
    }
}
