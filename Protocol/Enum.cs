using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votrix.Protocol
{
   
    public enum eProtocolType
    {
        shadowsocks,
        vmess,
        socks,
        trojan,
    }

    public enum eSecuritySS
    {
        aes_128_gcm,
        aes_256_gcm,
        aes_128_cfb,
        aes_256_cfb,
        chacha20,
        chacha20_ietf,
        chacha20_ietf_poly1305,
        chacha20_poly1305,
    }

    public enum eSecurityVmess
    {
        none,
        auto,
        aes_128_gcm,
        chacha20_poly1305,
    }

    public enum eSecuritySocks
    {
        aes_128_gcm,
        aes_256_gcm,
        aes_128_cfb,
        aes_256_cfb,
        chacha20,
        chacha20_ietf,
    }

    public enum eNetwork
    {
        tcp,
        kcp,
        ws,
        h2,
        quic,
    }

    public enum eLogLevel
    {
        debug,
        info, 
        warning,
        error,
        none,
    }

    public enum eDomainStrategy
    {
        AsIs,
        IPIfNonMatch,
        IPOnDemand,
    }

}
