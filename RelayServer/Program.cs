using System;

namespace RelayServer
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketManager.Instance().Open();
            
            Console.ReadLine();
        }
    }
}
