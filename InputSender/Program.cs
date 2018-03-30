using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputSender
{
    class Program
    {
        static void Main(string[] args)
        {
            InputSenderService svc = new InputSenderService();
            Console.WriteLine("Consle Started. Press END to terminate process.");
            while (true)
            {
                var key = Console.ReadKey(false);
                if (key.Key == ConsoleKey.End)
                    return;

                svc.SendAsync(key.KeyChar);
            }
        }
    }
}
