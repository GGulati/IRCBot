using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCBot
{
    public sealed class PluginDependencyAttribute : Attribute
    {
        public readonly Type[] Dependencies;

        public PluginDependencyAttribute(params Type[] types)
        {
            Dependencies = types;
        }
    }
}
