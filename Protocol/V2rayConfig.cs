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
        public Log log { get; set; }
        /// 统计需要， 空对象
        public Stats stats { get; set; }
        public API api { get; set; }
        public Policy policy { get; set; }
        /// DNS 配置
        public Dns dns { get; set; }
        /// 路由配置
        public Routing routing { get; set; }
        /// 传入连接配置
        public List<Inbounds> inbounds { get; set; }
        /// 传出连接配置
        public List<Outbounds> outbounds { get; set; }

        #region 简单的设置
        public V2rayConfig()
        {
            this.log = new Log();
            this.stats = new Stats();
            this.api = new API();
            this.policy = new Policy();
            this.dns = new Dns();
            this.routing = new Routing();
            this.inbounds = new List<Inbounds>();
            this.outbounds = new List<Outbounds>();
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
        /// 设置API相关
        /// </summary>
        /// <param name="sTag"></param>
        /// <param name="sServices"></param>
        public void SetApi(string sTag, params string[] sServices)
        {
            api.tag = sTag;
            api.services.Clear();
            foreach (string sName in sServices)
                api.services.Add(sName);
        }
        /// <summary>
        /// 策略相关
        /// </summary>
        /// <param name="bStatsInboundUplink"></param>
        /// <param name="bStatsInboundDownlink"></param>
        public void SetPolicy(bool bStatsInboundUplink, bool bStatsInboundDownlink)
        {
            policy.system.statsInboundUplink = bStatsInboundUplink;
            policy.system.statsInboundDownlink = bStatsInboundDownlink;
        }
        /// <summary>
        /// DNS相关
        /// </summary>
        /// <param name="sServices"></param>
        public void SetDns(params string[] sServices)
        {
            dns.servers.Clear();
            foreach (string sName in sServices)
                api.services.Add(sName);
        }
        /// <summary>
        /// 设置路由域名策略
        /// </summary>
        /// <param name="eObj"></param>
        public void SetRoutingDomainStrategy(eDomainStrategy eObj)
        {
            routing.domainStrategy = AIGS.Common.Convert.ConverEnumToString((int)eObj, typeof(eDomainStrategy), (int)eDomainStrategy.AsIs);
        }
        /// <summary>
        /// 添加路由规则
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sPort"></param>
        /// <param name="sinboundTag"></param>
        /// <param name="sOutboundTag"></param>
        /// <param name="sIP"></param>
        /// <param name="sDomain"></param>
        public void AddRoutingRulesItem(string sType, string sPort, string[] sinboundTag, string sOutboundTag, string[] sIP, string[] sDomain)
        {
            routing.rules.Add(new RulesItem()
            {
                type = sType,
                port = sPort,
                outboundTag = sOutboundTag,
                inboundTag = sinboundTag.ToList(),
                ip = sIP.ToList(),
                domain = sDomain.ToList()
            });
        }
        #endregion

        #region Stream相关
        /// <summary>
        /// 底层传输基本配置
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sNetWork"></param>
        /// <param name="sSecurity"></param>
        /// <param name="bTlsAllowInsecure"></param>
        /// <param name="sTlsServerName"></param>
        public void SetStreamSettingsBase(ref StreamSettings item, string sNetWork, string sSecurity, bool bTlsAllowInsecure = false, string sTlsServerName = null)
        {
            item.network = sNetWork;
            item.security = sSecurity;
            item.tlsSettings.allowInsecure = bTlsAllowInsecure;
            item.tlsSettings.serverName = sTlsServerName;
        }
        public void SetStreamSettingsTcp(ref StreamSettings item, bool bConnectionReuse, string sHeaderType, object oHeaderRequest, object oHeaderResponse)
        {
            item.tcpSettings.connectionReuse = bConnectionReuse;
            item.tcpSettings.header.type = sHeaderType;
            item.tcpSettings.header.request = oHeaderRequest;
            item.tcpSettings.header.response = oHeaderResponse;
        }
        public void SetStreamSettingsKcp(ref StreamSettings item, int mtu, int tti, int uplinkCapacity, int downlinkCapacity, bool congestion, int readBufferSize, int writeBufferSize, string sHeaderType, object oHeaderRequest, object oHeaderResponse)
        {
            item.kcpSettings.mtu = mtu;
            item.kcpSettings.tti = tti;
            item.kcpSettings.uplinkCapacity = uplinkCapacity;
            item.kcpSettings.downlinkCapacity = downlinkCapacity;
            item.kcpSettings.congestion = congestion;
            item.kcpSettings.readBufferSize = readBufferSize;
            item.kcpSettings.writeBufferSize = writeBufferSize;
            item.kcpSettings.header.type = sHeaderType;
            item.kcpSettings.header.request = oHeaderRequest;
            item.kcpSettings.header.response = oHeaderResponse;
        }
        public void SetStreamSettingsWs(ref StreamSettings item, bool connectionReuse, string path, string sHeadersHost)
        {
            item.wsSettings.connectionReuse = connectionReuse;
            item.wsSettings.path = path;
            item.wsSettings.headers.Host = sHeadersHost;
        }
        public void SetStreamSettingsHttp(ref StreamSettings item, string path, string[] host)
        {
            item.httpSettings.path = path;
            item.httpSettings.host = host.ToList();
        }
        public void SetStreamSettingsQuic(ref StreamSettings item, string security, string key, string sHeaderType, object oHeaderRequest, object oHeaderResponse)
        {
            item.quicSettings.security = security;
            item.quicSettings.key = key;
            item.quicSettings.header.type = sHeaderType;
            item.quicSettings.header.request = oHeaderRequest;
            item.quicSettings.header.response = oHeaderResponse;
        }
        #endregion

        #region 传入连接配置
        /// <summary>
        /// 添加传入(本地端口）
        /// </summary>
        /// <param name="sTag">备注</param>
        /// <param name="iPort">监听端口</param>
        /// <param name="sProtocol">监听协议,默认socks</param>
        /// <param name="bLanConnect">是否允许局域网连接</param>
        /// <param name="bSniffing">是否开启流量探针></param>
        /// <param name="sSniffingDestOverride">目标覆盖></param>
        /// <param name="sSettingsAuth"></param>
        /// <param name="bSupportUDP">是否支持UDP</param>
        /// <param name="sSettingsIP"></param>
        /// <param name="sSettingsAddress"></param>
        /// <returns></returns>
        public Inbounds CreatInbound(string sTag = "proxy", int iPort = 10808, string sProtocol = "socks", bool bLanConnect = false, bool bSniffing = true, string[] sSniffingDestOverride=null,
                                     string sSettingsAuth = "noauth", bool bSupportUDP = true, string sSettingsIP = null, string sSettingsAddress = null)
        {
            Inbounds item =  new Inbounds();
            item.tag = sTag;
            item.port = iPort;
            item.protocol = sProtocol;
            item.listen = bLanConnect ? "0.0.0.0" : "127.0.0.1";
            item.sniffing.enabled = bSniffing;
            if(sSniffingDestOverride != null)
                item.sniffing.destOverride = sSniffingDestOverride.ToList();

            item.settings.auth = sSettingsAuth;
            item.settings.udp = bSupportUDP;
            item.settings.ip = sSettingsIP;
            item.settings.address = sSettingsAddress;
            return item;
        }
        /// <summary>
        /// 添加传入的用户（作为客户端好像不用设置）
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sID"></param>
        /// <param name="iAlterId"></param>
        /// <param name="sEmail"></param>
        /// <param name="sSecurity"></param>
        public void AddInboundSettingsClient(ref Inbounds item, string sID, int iAlterId, string sEmail, string sSecurity)
        {
            item.settings.clients.Add(new UsersItem()
            { 
                id = sID,
                alterId = iAlterId,
                email = sEmail,
                security = sSecurity,
            });
        }
        #endregion

        #region 传出连接配置
        public Outbounds CreatOutbounds(string sTag, string sProtocol, bool bMuxEnabled, int bMuxConcurrency, string sSettingsResponseType)
        {
            Outbounds item = new Outbounds();
            item.tag = sTag;
            item.protocol = sProtocol;
            item.mux.enabled = bMuxEnabled;
            item.mux.concurrency = bMuxConcurrency;

            item.settings.response.type = sSettingsResponseType;
            return item;
        }
        public void AddOutboundsVnextItem(ref Outbounds item, string sAddress, int iPort, List<UsersItem> users)
        {
            item.settings.vnext.Add(new VnextItem()
            {
                address = sAddress,
                port = iPort,
                users = users,
            });
        }
        public void AddOutboundsServersItem(ref Outbounds item, string email, string address, string method, bool ota, string password, int iPort, int level, List<SocksUsersItem> users)
        {
            item.settings.servers.Add(new ServersItem()
            {
                email = email,
                address = address,
                method = method,
                ota = ota,
                password = password,
                port = iPort,
                level = level,
                users = users,
            });
        }
        #endregion



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
