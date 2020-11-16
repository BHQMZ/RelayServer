using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
    public partial class GameService : ServiceBase
    {
        public GameService()
        {
            InitializeComponent();
        }

        string filePath = @"C:\MyServiceLog.txt";

        protected override void OnStart(string[] args)
        {
            //SocketManager.Instance().Open();
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now},服务启动！");
                try
                {
                    SocketManager.Instance().Open();
                    writer.WriteLine($"{DateTime.Now},socket启动！");
                }
                catch (Exception e)
                {
                    writer.WriteLine($"{DateTime.Now},"+ e);
                }
            }
            //using (SocketManager.Instance())
            //{
            //    //SocketManager.Instance().Open();
            //}
        }

        protected override void OnStop()
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now},服务停止！");
            }
        }
    }
}
