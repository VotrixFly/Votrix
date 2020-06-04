using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;
using AIGS.Helper;
using AIGS.Common;
using AIGS;
using System.Collections.ObjectModel;
using Votrix.Else;
using System.Windows.Controls;

namespace Votrix.Pages
{
    public class ServerListViewModel : Screen
    {
        public bool ShowSS { get; set; } = true;
        public bool ShowVMess { get; set; }
        public bool ShowShocks { get; set; }

        public int SelectIndex { get; set; }
        public Server CurServer { get; set; }
        public ObservableCollection<Server> ServerList { get; set; } = new ObservableCollection<Server>();

        public ServerListViewModel()
        {

        }

        public void AddServer()
        {
            ServerList.Add(new Server());
        }

        public void SelectServer()
        {
            if (SelectIndex < 0)
                return;

            if (CurServer == null)
                CurServer = new Server();
            CurServer.Copy(ServerList[SelectIndex]);
        }
        
        public void SaveServer()
        {
            ServerList[SelectIndex].Copy(CurServer);
        }
    }
}
