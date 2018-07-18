using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Sender sender = new Sender();
            
            Task.Run(() => sender.SendAsync());

            
            Console.ReadKey();
        }
    }
}
