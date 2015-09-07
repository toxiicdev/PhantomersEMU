using System;
using System.Collections.Generic;
using System.Text;

using LoginServer.Network;
using Shared;

namespace LoginServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@" ___________________________________________");
            Console.WriteLine(@"|  _     _   ____     __ _  __     __       |");
            Console.WriteLine(@"| |_)|_||_||\||/ \|V||_ |_)(_     |_ |V|| | |");
            Console.WriteLine(@"| |  | || || ||\_/| ||__| \__)    |__| ||_| |");
            Console.WriteLine(@"|___________________________________________|");

            Log.WriteBlank(2);

            TcpServer tcp = new TcpServer(15010); // Default port - TODO SETTINGS
            if(!tcp.HasInit)
            {
                Log.WriteError("Cannot bound Tcp Socket on the port 15010");
            }

            PacketTable.Load();

            Console.ReadLine();
        }
    }
}
