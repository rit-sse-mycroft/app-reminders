using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reminders
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Expected arguments in the form host port");
                return;
            }
            var server = new RemindersClient("app.json");
            server.Connect(args[0], args[1]);
        }
    }
}
