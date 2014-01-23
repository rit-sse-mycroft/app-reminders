using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App.Message
{
    public class MessageQuery
    {
        private string capability;
        private string action;
        private object data;
        private string[] instanceId;
        private int priority;
        private Guid id;

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
