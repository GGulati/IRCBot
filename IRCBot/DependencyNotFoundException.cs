using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRCBot
{
    class DependencyNotFoundException : Exception
    {
        public DependencyNotFoundException(string message) : base(message) { }
    }
}
