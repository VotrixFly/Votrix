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
    public enum ServerType
    {
        SS,
        VMESS,
        SOCKS,
    }

    public class Server : Screen
    {
        public ServerType SType { get; set; } = ServerType.SS;
        public string Name { get; set; } = "New Server" + RandHelper.GetIntRandom(3,9,0);
        public string Address { get; set; } = "127.0.0.1";
        public string Port { get; set; } = "1080";

        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "admin";

        //加密方式
        public string SecuritySS { get; set; }
        public string SecurityVmess { get; set; }
        public string SecuritySocks { get; set; }

        public string UUID { get; set; } = "xxxxxx";
        public string AlterID { get; set; } = "xxxxxx";

        //传输协议
        public string Network { get; set; }
        //伪装类型
        public string Type { get; set; }

        public string Host { get; set; } = "192.168.0.1";
        public string Path { get; set; } = "/";

        public bool TLSEnable { get; set; } = false;
        public bool AllowInsecure { get; set; } = false;

        public void Copy(Server obj)
        {
            SType = obj.SType;
            Name = obj.Name;
            Address = obj.Address;
            Port = obj.Port;

            UserName = obj.UserName;
            Password = obj.Password;

            SecuritySS = obj.SecuritySS;
            SecurityVmess = obj.SecurityVmess;
            SecuritySocks = obj.SecuritySocks;

            UUID = obj.UUID;
            AlterID = obj.AlterID;

            Network = obj.Network;
            Type = obj.Type;
            Host = obj.Host;
            Path = obj.Path;

            TLSEnable = obj.TLSEnable;
            AllowInsecure = obj.AllowInsecure;
        }
    }
}
