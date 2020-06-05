using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votrix.Protocol
{
    public class Log
    {
        public string access { get; set; }
        public string error { get; set; }
        public string loglevel { get; set; }
    }

    public class Inbounds
    {
        public int port { get; set; }
        public string listen { get; set; }
        public string protocol { get; set; }
        public Sniffing sniffing { get; set; }
        public Inboundsettings settings { get; set; }
        public StreamSettings streamSettings { get; set; }
    }

    public class Inboundsettings
    {
        public string auth { get; set; }
        public bool udp { get; set; }
        public string ip { get; set; }
        public List<UsersItem> clients { get; set; }
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
        public List<string> destOverride { get; set; }
    }

    public class Outbounds
    {
        /// 默认值agentout
        public string tag { get; set; }
        public string protocol { get; set; }
        public Outboundsettings settings { get; set; }
        public StreamSettings streamSettings { get; set; }
        public Mux mux { get; set; } = new Mux();
    }

    public class Outboundsettings
    {
        public List<VnextItem> vnext { get; set; }
        public List<ServersItem> servers { get; set; }
        public Response response { get; set; }
    }

    public class VnextItem
    {
        public string address { get; set; }
        public int port { get; set; }
        public List<UsersItem> users { get; set; }

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
    }

    public class Mux
    {
        public bool enabled { get; set; }
    }

    public class Response
    {
        public string type { get; set; }
    }

    public class Dns
    {
        public List<string> servers { get; set; }
    }

    public class RulesItem
    {
        public string type { get; set; }
        public string port { get; set; }
        public string outboundTag { get; set; }
        public List<string> ip { get; set; }
        public List<string> domain { get; set; }
    }

    public class Routing
    {
        public string domainStrategy { get; set; }
        public List<RulesItem> rules { get; set; }
    }

    public class StreamSettings
    {
        public string network { get; set; }
        public string security { get; set; }
        public TlsSettings tlsSettings { get; set; }
        /// Tcp传输额外设置
        public TcpSettings tcpSettings { get; set; }
        /// Kcp传输额外设置
        public KcpSettings kcpSettings { get; set; }
        /// ws传输额外设置
        public WsSettings wsSettings { get; set; }
        /// h2传输额外设置
        public HttpSettings httpSettings { get; set; }
        /// QUIC
        public QuicSettings quicSettings { get; set; }
    }

    public class TlsSettings
    {
        /// 是否允许不安全连接（用于客户端）
        public bool allowInsecure { get; set; }
        public string serverName { get; set; }
    }

    public class TcpSettings
    {
        /// 是否重用 TCP 连接
        public bool connectionReuse { get; set; }
        /// 数据包头部伪装设置
        public Header header { get; set; }
    }

    public class Header
    {
        /// 伪装
        public string type { get; set; }
        /// 结构复杂，直接存起来
        public object request { get; set; }
        /// 结构复杂，直接存起来
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
        public Header header { get; set; }
    }

    public class WsSettings
    {
        public bool connectionReuse { get; set; }
        public string path { get; set; }
        public Headers headers { get; set; }
    }

    public class Headers
    {
        public string Host { get; set; }
    }

    public class HttpSettings
    {
        public string path { get; set; }
        public List<string> host { get; set; }
    }

    public class QuicSettings
    {
        public string security { get; set; }
        public string key { get; set; }
        public Header header { get; set; }
    }

    [System.Serializable]
    class VmessQRCode
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
