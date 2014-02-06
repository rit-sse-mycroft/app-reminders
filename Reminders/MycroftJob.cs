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
    /// <summary>
    /// The Mycroft Job class
    /// </summary>
    public class MycroftJob : IJob
    {
        /// <summary>
        /// Executes a job
        /// </summary>
        /// <param name="context">THe job context</param>
        public void Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Client client = (Client) dataMap["client"];
            string phrase = (string) dataMap["phrase"];
            Console.WriteLine(phrase);
            SendTTS(client, phrase);
        }
        /// <summary>
        /// Sends a message to TTS
        /// </summary>
        /// <param name="client">The Reminders client</param>
        /// <param name="phrase">the phrase to say</param>
        protected async void SendTTS(Client client, string phrase)
        {
            var data =  new
            {
                text = new object[] {
                    new {
                            phrase = phrase, 
                            delay= 0
                        }
                    },
                targetSpeaker = "speakers"
            };
            await client.Query("tts", "stream", data);
        }
    }
}
