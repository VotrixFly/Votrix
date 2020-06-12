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
        //开机启动、启动时最小化
        public bool AutoStart { get; set; } = true;
        public bool SmallStart { get; set; } = true;
        //自动更新
        public bool AutoUpdate { get; set; } = true;

        //语言与主题
        public int Language { get; set; } = 0;
        public int Theme { get; set; } = 0;

        //端口
        public string PortSocks5 { get; set; } = "10808";
        public string PortHttp { get; set; } = "10809";

        //支持UDP与局域网分享、多路复用
        public bool SupportUDP { get; set; } = false;
        public bool ShareAreaNetwork { get; set; } = false;
        public bool OpenMux { get; set; } = true;

        //日志
        public int Loglevel { get; set; } = (int)eLogLevel.warning;
        public bool SaveLog { get; set; } = false;

        //流量探针
        public bool OpenSniffing { get; set; } = true;

        //GFW地址/DNS/PAC
        public string GFWList { get; set; }
        public string DNSList { get; set; }
        public string PACList { get; set; }

        //KCP设置
        public int Mtu { get; set; } = 1350;
        public int Tti { get; set; } = 50;
        public int UplinkCapacity { get; set; } = 12;
        public int DownlinkCapacity { get; set; } = 100;
        public int ReadBufferSize { get; set; } = 2;
        public int WriteBufferSize { get; set; } = 2;
        public bool Congestion { get; set; } = false;

        //路由设置
        public int DomainStrategy { get; set; } = (int)eDomainStrategy.IPIfNonMatch;
        public string AgentDomain { get; set; }
        public string LimitDomain { get; set; }
        public string DirectDomain { get; set; }
        public int RouteRule { get; set; } = 3;
    }

}
