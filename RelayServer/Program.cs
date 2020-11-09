using System;

namespace RelayServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketManager.Instance().open();

            Console.ReadLine();
        }
    }
}
