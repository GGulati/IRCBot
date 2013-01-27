using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace IRCBot
{
    public abstract class Plugin
    {
        private IPluginHost host;

        /// <summary>
        /// All children must have a constructor with 0 arguments
        /// </summary>
        public Plugin()
        {
        }

        internal void Added(IPluginHost host)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            this.host = host;

            OnAdded();
        }
        protected virtual void OnAdded()
        {
        }

        internal void Removed()
        {
            OnRemoved();
            host = null;
        }
        protected virtual void OnRemoved()
        {
        }

        public abstract void Save(XmlWriter writer);

        public abstract void Load(XmlReader reader);

        public IPluginHost Host { get { return host; } }
    }
}
