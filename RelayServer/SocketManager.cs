using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RelayServer
{
    class SocketManager : Socket
    {
        public SocketManager(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType) { }

        private static SocketManager _SocketManager;

        public  static SocketManager Instance()
        {
            if(_SocketManager == null)
            {
                _SocketManager = new SocketManager(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            return _SocketManager;
        }

        private List<Socket> _clientSocket = new List<Socket>();

        public void open()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            _SocketManager.Bind(ip);

            // 设置最大连接数
            _SocketManager.Listen(10);

            Thread thread = new Thread(_listen);
            thread.IsBackground = true;
            thread.Start();
        }

        private void _listen()
        {
            Console.WriteLine("开始监听");
            while (true)
            {
                //开始监听
                Socket socket = _SocketManager.Accept();
                Console.WriteLine("有客户端链接");
                //保存客户端
                _clientSocket.Add(socket);
                //开始接收消息
                Thread thread = new Thread(_receiveMessage);
                thread.IsBackground = true;
                thread.Start(socket);
            }
        }

        private void _receiveMessage(object obj)
        {
            Socket socket = (Socket)obj;
            while (true)
            {
                try
                {
                    byte[] bs = new byte[1024];
                    int length = socket.Receive(bs);
                    //如果Receive返回值为0，则可以默认客户端已断开链接
                    if (length > 0)
                    {
                        Console.WriteLine(socket.RemoteEndPoint.ToString() + ":" + new UTF8Encoding().GetString(bs) + "\n");
                    }
                    else
                    {
                        _clientBreakOff(socket);

                        break;
                    }
                }
                catch
                {
                    _clientBreakOff(socket);

                    break;
                }
            }
        }

        private void _clientBreakOff(Socket socket)
        {
            _clientSocket.Remove(socket);
            Console.WriteLine(socket.RemoteEndPoint.ToString() + "客户端断开连接\n");
        }
    }
}
 