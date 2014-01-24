using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App.Message
{
    public class MessageQuery
    {
        public string capability;
        public string action;
        public object data;
        public string[] instanceId;
        public int priority;
        public Guid id;

        public MessageQuery(string capability, string action, object data, string[] instanceId, int priority )
        {
            this.id = Guid.NewGuid();
            this.capability = capability;
            this.action = action;
            this.data = data;
            this.instanceId = instanceId;
            this.priority = priority;
        }
    }
}
