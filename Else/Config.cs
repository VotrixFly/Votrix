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
        private static string SettingsPath { get { return Path + "settings.json";  } }
        private static string ServersPath { get { return Path + "servers.json"; } }

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
            return ret == null ? new ObservableCollection<Server>() : ret;
        }

        public static Settings RWSettings(Settings wObj = null)
        {
            Settings ret = ReadWrite<Settings>(SettingsPath, wObj);
            return ret == null ? new Settings() : ret;
        }
        #endregion
    }

    
}
