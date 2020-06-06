using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIGS.Common;
using AIGS.Helper;
using Stylet;
using System.IO;
using Votrix.Protocol;
using System.Collections.ObjectModel;

namespace Votrix.Else
{
    public class Settings
    {
        //开机启动
        public bool AutoStart { get; set; } = true;
        //自动更新
        public bool AutoUpdate { get; set; } = true;

        //语言与主题
        public int Language { get; set; } = 0;
        public int Theme { get; set; } = 0;

        //端口
        public string PortSocks5 { get; set; } = "10808";
        public string PortHttp { get; set; } = "10809";

        //支持UDP与局域网分享
        public bool SupportUDP { get; set; } = false;
        public bool ShareAreaNetwork { get; set; } = false;
    }
}
