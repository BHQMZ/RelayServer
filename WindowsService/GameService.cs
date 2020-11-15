using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using RelayServer;

namespace WindowsService
{
    public partial class GameService : ServiceBase
    {
        public GameService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SocketManager.Instance().Open();
        }

        protected override void OnStop()
        {
        }
    }
}
