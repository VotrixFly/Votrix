using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIGS.Common;
using AIGS.Helper;
using Stylet;
using System.IO;
using System.Collections.ObjectModel;

namespace Votrix.Else
{
    public class Config
    {
        private static string Path = "./data/";
        private static string BasePath { get { return Path + "config.json";  } }
        private static string ServersPath { get { return Path + "servers.json"; } }

        public class Base : Screen
        {
            public bool AutoStart { get; set; } = true;
            public bool AutoUpdate { get; set; } = true;
            public int Language { get; set; } = 0;
            public int Theme { get; set; } = 0;

            public string PortSocks5 { get; set; } = "10808";
            public string PortHttp { get; set; } = "10809";
            public bool SupportUDP { get; set; } = false;
            public bool ShareAreaNetwork { get; set; } = false;
        }


        #region 读写
        public static T ReadWrite<T>(string path, T wObj = default(T))
        {
            string sText;
            if (wObj == null)
            {
                sText = FileHelper.Read(path);
                T rObj = JsonHelper.ConverStringToObject<T>(sText);
                return rObj;
            }

            sText = JsonHelper.ConverObjectToString<T>(wObj);
            FileHelper.Write(sText, true, path);
            return default(T);
        }

        public static ObservableCollection<Server> RWServers(ObservableCollection<Server> wObj = null)
        {
            ObservableCollection<Server> ret = ReadWrite<ObservableCollection<Server>>(ServersPath, wObj);
            return ret == null ? new ObservableCollection<Server>() : null;
        }

        public static Base RWBase(Base wObj = null)
        {
            Base ret = ReadWrite<Base>(BasePath, wObj);
            return ret == null ? new Base() : null;
        }
        #endregion
    }

    
}
