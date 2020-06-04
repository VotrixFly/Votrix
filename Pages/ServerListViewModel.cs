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
using System.Collections;
using Votrix.Else;
using Votrix.Net;

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


        public static Dictionary<int, string> ComboxSecuritySS { get; set; }
        public static Dictionary<int, string> ComboxSecurityVmess { get; set; }
        public static Dictionary<int, string> ComboxSecuritySocks { get; set; }
        public static Dictionary<int, string> ComboxNetwork { get; set; }

        public ServerListViewModel()
        {
            ComboxSecuritySS = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eSecuritySS), false);
            ComboxSecurityVmess = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eSecurityVmess), false); 
            ComboxSecuritySocks = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eSecuritySocks), false);
            ComboxNetwork = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eNetwork), false);

        }

        #region 操作服务器
        //添加服务器
        public void AddServer()
        {
            ServerList.Add(new Server());

        }

        //删除服务器
        public void DelServer()
        {
            if (SelectIndex < 0)
                return;
            ServerList.RemoveAt(SelectIndex);
        }

        //启动服务器
        public void StartServer()
        {

        }

        //关闭服务器
        public void CloseServer()
        {

        }

        //重启服务器
        public void RestartServer()
        {

        }

        //选择服务器

        public void SelectServer()
        {
            if (SelectIndex < 0)
                return;

            if (CurServer == null)
                CurServer = new Server();
            CurServer.Copy(ServerList[SelectIndex]);
        }
        

        //保存服务器
        public void SaveServer()
        {
            if (CurServer == null || SelectIndex < 0)
                return;
            ServerList[SelectIndex].Copy(CurServer);
            return;
        }
        #endregion

    }
}
