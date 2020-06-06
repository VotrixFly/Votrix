using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using AIGS.Common;
using AIGS.Helper;

namespace Votrix.Protocol
{
    public class V2rayConfig
    {
        /// 日志配置
        public Log log { get; set; } = new Log();
        /// 传入连接配置
        public List<Inbounds> inbounds { get; set; } = new List<Inbounds>();
        /// 传出连接配置
        public List<Outbounds> outbounds { get; set; } = new List<Outbounds>();
        /// 统计需要， 空对象
        public Stats stats { get; set; } = new Stats();
        public API api { get; set; } = new API();
        public Policy policy { get; set; } = new Policy();
        /// DNS 配置
        public Dns dns { get; set; } = new Dns();
        /// 路由配置
        public Routing routing { get; set; } = new Routing();


        #region 方法
        //加载模板
        public void LoadTemplate()
        {
            string sText = SystemHelper.GetEmbedText("Votrix.Protocol.V2rayTemplate.json");
            this.log = JsonHelper.ConverStringToObject<Log>(sText, "log");
            this.inbounds = JsonHelper.ConverStringToObject<List<Inbounds>>(sText, "inbounds");
            this.outbounds = JsonHelper.ConverStringToObject<List<Outbounds>>(sText, "outbounds");
            this.stats = JsonHelper.ConverStringToObject<Stats>(sText, "stats");
            this.api = JsonHelper.ConverStringToObject<API>(sText, "api");
            this.policy = JsonHelper.ConverStringToObject<Policy>(sText, "policy");
            this.dns = JsonHelper.ConverStringToObject<Dns>(sText, "dns");
            this.routing = JsonHelper.ConverStringToObject<Routing>(sText, "routing");
        }
        /// <summary>
        /// 设置日志相关
        /// </summary>
        /// <param name="eLevel">日志登记</param>
        /// <param name="bOutput">是否输出日志文件</param>
        public void SetLog(eLogLevel eLevel, bool bOutput = false)
        {
            log.loglevel = AIGS.Common.Convert.ConverEnumToString((int)eLevel, typeof(eLogLevel), (int)eLogLevel.warning);
            log.access = bOutput ? Tool.V2RAY_CORE_LOG_ACCESS : "";
            log.error = bOutput ? Tool.V2RAY_CORE_LOG_ERROR : "";
        }
        /// <summary>
        /// 设置传入(本地端口）
        /// </summary>
        /// <param name="iPort">监听端口</param>
        /// <param name="sProtocol">监听协议,默认socks</param>
        /// <param name="bLanConnect">是否允许局域网连接</param>
        /// <param name="bUdp">是否支持UDP</param>
        /// <param name="bSniffing">是否开启流量探针></param>
        public void SetInbound(int iPort = 10808, string sProtocol = "socks", bool bLanConnect = false, bool bUdp = true, bool bSniffing = true)
        {
            if (inbounds.Count < 1)
                inbounds.Add(new Inbounds());
            inbounds[0].port = iPort;
            inbounds[0].protocol = sProtocol;
            inbounds[0].listen = bLanConnect ? "0.0.0.0" : "127.0.0.1";
            inbounds[0].settings.udp = bUdp;
            inbounds[0].sniffing.enabled = bSniffing;
        }
        // 服务器
        public void SetOutbound(eProtocolType eType, string sAddress, string sUID, int iPort = 443, int iAlterId = 233, string sEmail = "t@t.tt", string sSecurity = "auto", bool bMux = true)
        {
            if (outbounds.Count < 1)
                outbounds.Add(new Outbounds());
            /*
        public string tag { get; set; }
        public string protocol { get; set; }
        public Outboundsettings settings { get; set; } = new Outboundsettings();
        public StreamSettings streamSettings { get; set; } = new StreamSettings();
        public Mux mux { get; set; } = new Mux();   
             */
            outbounds[0].protocol = AIGS.Common.Convert.ConverEnumToString((int)eType, typeof(eProtocolType), (int)eProtocolType.vmess);
            if ( eType == eProtocolType.vmess)
            {
                //多路复用
                outbounds[0].mux.enabled = bMux;
                outbounds[0].mux.concurrency = bMux ? 8 : -1;
            }


            if (outbounds[0].settings == null)
                outbounds[0].settings = new Outboundsettings();
            if (outbounds[0].settings.vnext == null)
                outbounds[0].settings.vnext = new List<VnextItem>();

            outbounds[0].settings.servers = null;
            outbounds[0].settings.vnext[0] = new VnextItem()
            {
                address = sAddress,
                port = iPort,
                users = new List<UsersItem>() { new UsersItem() {
                    id       = sUID,
                    alterId  = iAlterId,
                    email    = sEmail,
                    security = sSecurity,
                } },
            };
        }
        // 路由
        public void SetRouting(string sDomainStrategy = "IPIfNonMatch")
        {
            if (routing == null)
                routing = new Routing();
            routing.domainStrategy = sDomainStrategy;
        }
        // 设置WS传输配置
        public void SetWSStreamSettings(string sHost, string sPath = null, string sSecurity = "tls", bool bAllowInsecure = true)
        {
            if (outbounds == null)
                outbounds = new List<Outbounds>() { new Outbounds() };
            if (outbounds[0].streamSettings == null)
                outbounds[0].streamSettings = new StreamSettings();
            if (outbounds[0].streamSettings.wsSettings == null)
                outbounds[0].streamSettings.wsSettings = new WsSettings();
            if (outbounds[0].streamSettings.wsSettings.headers == null)
                outbounds[0].streamSettings.wsSettings.headers = new Headers();
            if (outbounds[0].streamSettings.tlsSettings == null)
                outbounds[0].streamSettings.tlsSettings = new TlsSettings();

            outbounds[0].streamSettings.network = "ws";
            outbounds[0].streamSettings.security = sSecurity;
            outbounds[0].streamSettings.wsSettings.connectionReuse = true;
            outbounds[0].streamSettings.wsSettings.path = sPath;
            outbounds[0].streamSettings.wsSettings.headers.Host = sHost;

            outbounds[0].streamSettings.tlsSettings.allowInsecure = bAllowInsecure;
            outbounds[0].streamSettings.tlsSettings.serverName = sHost;
        }
        #endregion
    }




    public class Stats { };

    public class API
    {
        public string tag { get; set; }
        public List<string> services { get; set; } = new List<string>();
    }

    public class Policy
    {
        public SystemPolicy system { get; set; } = new SystemPolicy();
    }

    public class SystemPolicy
    {
        public bool statsInboundUplink { get; set; }
        public bool statsInboundDownlink { get; set; }
    }

    public class Log
    {
        public string access { get; set; } 
        public string error { get; set; } 
        public string loglevel { get; set; } 
    }

    public class Inbounds
    {
        public string tag { get; set; }
        public int port { get; set; }
        public string listen { get; set; }
        public string protocol { get; set; }
        public Sniffing sniffing { get; set; } = new Sniffing();
        public Inboundsettings settings { get; set; } = new Inboundsettings();
        public StreamSettings streamSettings { get; set; } = new StreamSettings();
    }

    public class Inboundsettings
    {
        public string auth { get; set; }
        public bool udp { get; set; }
        public string ip { get; set; }
        /// <summary>
        /// api 使用
        /// </summary>
        public string address { get; set; }
        public List<UsersItem> clients { get; set; } = new List<UsersItem>();
    }

    public class UsersItem
    {
        public string id { get; set; }
        public int alterId { get; set; }
        public string email { get; set; }
        public string security { get; set; }
    }

    public class Sniffing
    {
        public bool enabled { get; set; }
        public List<string> destOverride { get; set; } = new List<string>();
    }

    public class Outbounds
    {
        /// <summary>
        /// 默认值agentout
        /// </summary>
        public string tag { get; set; }
        public string protocol { get; set; }
        public Outboundsettings settings { get; set; } = new Outboundsettings();
        public StreamSettings streamSettings { get; set; } = new StreamSettings();
        public Mux mux { get; set; } = new Mux();   
    }

    public class Outboundsettings
    {
        public List<VnextItem> vnext { get; set; } = new List<VnextItem>();
        public List<ServersItem> servers { get; set; } = new List<ServersItem>();
        public Response response { get; set; } = new Response();
    }

    public class VnextItem
    {
        public string address { get; set; }
        public int port { get; set; }
        public List<UsersItem> users { get; set; } = new List<UsersItem>();
    }
    public class ServersItem
    {
        public string email { get; set; }
        public string address { get; set; }
        public string method { get; set; }
        public bool ota { get; set; }
        public string password { get; set; }
        public int port { get; set; }
        public int level { get; set; }
        public List<SocksUsersItem> users { get; set; } = new List<SocksUsersItem>();
    }

    public class SocksUsersItem
    {
        public string user { get; set; }
        public string pass { get; set; }
        public int level { get; set; }
    }


    public class Mux
    {
        public bool enabled { get; set; }
        public int concurrency { get; set; }
    }

    public class Response
    {
        public string type { get; set; }
    }

    public class Dns
    {
        public List<string> servers { get; set; } = new List<string>();
    }

    public class RulesItem
    {
        public string type { get; set; }
        public string port { get; set; }
        public List<string> inboundTag { get; set; } = new List<string>();
        public string outboundTag { get; set; } 
        public List<string> ip { get; set; } = new List<string>();
        public List<string> domain { get; set; } = new List<string>();
    }

    public class Routing
    {
        //域名解析策略
        public string domainStrategy { get; set; }
        public List<RulesItem> rules { get; set; } = new List<RulesItem>();
    }

    public class StreamSettings
    {

        public string network { get; set; }
        public string security { get; set; }
        public TlsSettings tlsSettings { get; set; } = new TlsSettings();
        /// <summary>
        /// Tcp传输额外设置
        /// </summary>
        public TcpSettings tcpSettings { get; set; } = new TcpSettings();
        /// <summary>
        /// Kcp传输额外设置
        /// </summary>
        public KcpSettings kcpSettings { get; set; } = new KcpSettings();
        /// <summary>
        /// ws传输额外设置
        /// </summary>
        public WsSettings wsSettings { get; set; } = new WsSettings();
        /// <summary>
        /// h2传输额外设置
        /// </summary>
        public HttpSettings httpSettings { get; set; } = new HttpSettings();
        /// <summary>
        /// QUIC
        /// </summary>
        public QuicSettings quicSettings { get; set; } = new QuicSettings();

    }

    public class TlsSettings
    {
        /// <summary>
        /// 是否允许不安全连接（用于客户端）
        /// </summary>
        public bool allowInsecure { get; set; }
        public string serverName { get; set; }
    }

    public class TcpSettings
    {
        /// <summary>
        /// 是否重用 TCP 连接
        /// </summary>
        public bool connectionReuse { get; set; }
        /// <summary>
        /// 数据包头部伪装设置
        /// </summary>
        public Header header { get; set; } = new Header();
    }

    public class Header
    {
        /// <summary>
        /// 伪装
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 结构复杂，直接存起来
        /// </summary>
        public object request { get; set; }
        /// <summary>
        /// 结构复杂，直接存起来
        /// </summary>
        public object response { get; set; }
    }

    public class KcpSettings
    {
        public int mtu { get; set; }
        public int tti { get; set; }
        public int uplinkCapacity { get; set; }
        public int downlinkCapacity { get; set; }
        public bool congestion { get; set; }
        public int readBufferSize { get; set; }
        public int writeBufferSize { get; set; }
        public Header header { get; set; } = new Header();
    }

    public class WsSettings
    {
        public bool connectionReuse { get; set; }
        public string path { get; set; }
        public Headers headers { get; set; } = new Headers();
    }
    public class Headers
    {
        public string Host { get; set; }
    }

    public class HttpSettings
    {
        public string path { get; set; }
        public List<string> host { get; set; } = new List<string>();

    }

    public class QuicSettings
    {
        public string security { get; set; }
        public string key { get; set; }
        public Header header { get; set; } = new Header();
    }



    [System.Serializable]
    class VmessUrl
    {
        /// 版本
        public string v { get; set; }
        /// 备注
        public string ps { get; set; }
        /// 远程服务器地址
        public string add { get; set; }
        /// 远程服务器端口
        public string port { get; set; }
        /// 远程服务器ID
        public string id { get; set; }
        /// 远程服务器额外ID
        public string aid { get; set; }
        /// 传输协议tcp,kcp,ws
        public string net { get; set; }
        /// 伪装类型
        public string type { get; set; }
        /// 伪装的域名
        public string host { get; set; }
        /// path
        public string path { get; set; }
        /// 底层传输安全
        public string tls { get; set; }
    }
}
