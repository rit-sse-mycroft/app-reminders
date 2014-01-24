using Mycroft.App;
using Mycroft.App.Message;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminders
{
    public abstract class MycroftJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Client client = (Client) dataMap["client"];
            string phrase = (string) dataMap["phrase"];
            Console.WriteLine(phrase);
            SendTTS(client, phrase);
        }
        protected async void SendTTS(Client client, string phrase)
        {
            await client.SendJson("MSG_QUERY", new MessageQuery("tts", "stream", new
            {
                text =
                    new object[] {
                        new {
                            phrase = phrase , 
                            delay= 0
                        }
                    }
            }, new string[] { }, 30));
        }
    }
}
