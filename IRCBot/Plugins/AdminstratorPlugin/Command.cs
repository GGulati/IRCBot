using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Meebey.SmartIrc4net;

namespace IRCBot.Plugins.AdminstratorPlugin
{
    public abstract class Command
    {
        public static Command[] AllCommands { get; private set; }
        public static void Initialize(Administrator plugin)
        {
            Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            Type commandType = typeof(Command);

            List<Command> commands = new List<Command>();
            Type[] argTypes = { typeof(Administrator) };
            object[] args = { plugin };

            foreach (var type in types)
            {
                if (type.IsSubclassOf(commandType))
                    commands.Add((Command)type.GetConstructor(argTypes).Invoke(args));
            }

            AllCommands = commands.ToArray();
        }
        public static Command GetCommand(string name)
        {
            foreach (var command in AllCommands)
            {
                if (command.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return command;
            }

            return null;
        }

        public readonly string Name;
		public readonly string Help;
        protected readonly Administrator plugin;

        public Command(Administrator plugin, string name, string help = "Not implemented yet")
        {
            Name = name.ToLower();
			Help = help;
            this.plugin = plugin;
        }

        public abstract void Execute(IrcMessageData data);
    }
}
