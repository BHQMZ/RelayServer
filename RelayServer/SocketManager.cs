using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RelayServer
{
    public class SocketManager : Socket
    {
        public SocketManager(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) : base(addressFamily, socketType, protocolType) { }

        private static SocketManager _socketManager;

        public  static SocketManager Instance()
        {
            if(_socketManager == null)
            {
                _socketManager = new SocketManager(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            return _socketManager;
        }

        private List<Socket> _clientSocket = new List<Socket>();

        /// <summary>
        /// 开启socket
        /// </summary>
        /// <returns></returns>
        public void Open()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(GameUtils.GetLocalIP()), 5555);
            _socketManager.Bind(ip);

            // 设置最大连接数
            _socketManager.Listen(10);

            Thread thread = new Thread(_Listen);
            thread.IsBackground = true;
            thread.Start();
        }

        //监听客户端链接
        private void _Listen()
        {
            Console.WriteLine("开始监听");
            while (true)
            {
                //开始监听
                Socket socket = _socketManager.Accept();
                Console.WriteLine(socket.RemoteEndPoint.ToString() + " 客户端链接\n");
                //保存客户端
                _clientSocket.Add(socket);
                //开始接收消息
                Thread thread = new Thread(_ReceiveMessage);
                thread.IsBackground = true;
                thread.Start(socket);
            }
        }

        //监听客户端信息
        private void _ReceiveMessage(object obj)
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
                        //转发给其他连入的客户端
                        _Relay(bs);

                        Console.WriteLine(socket.RemoteEndPoint.ToString() + ":" + new UTF8Encoding().GetString(bs) + "\n");

                        //Console.WriteLine(socket.RemoteEndPoint.ToString() + ":" + BitConverter.ToUInt32(bs) + "\n");
                    }
                    else
                    {
                        _ClientBreakOff(socket);

                        break;
                    }
                }
                catch
                {
                    _ClientBreakOff(socket);

                    break;
                }
            }
        }

        private void _ClientBreakOff(Socket socket)
        {
            _clientSocket.Remove(socket);
            Console.WriteLine(socket.RemoteEndPoint.ToString() + "客户端断开连接\n");
        }

        private void _Relay(byte[] bs)
        {
            _clientSocket.ForEach(socket =>
            {
                socket.Send(bs);
            });
        }
    }
}
 