using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votrix.Net
{
    public enum eProtocolType
    {
        SS,
        Vmess,
        Socks,
    }
    
    public enum eSecuritySS
    {
        aes_128_gcm,
        aes_192_gcm,
        aes_256_gcm,
        aes_128_cfb,
        aes_192_cfb,
        aes_256_cfb,
        aes_128_ctr,
        aes_192_ctr,
        aes_256_ctr,
        chacha20,
        chacha20_ietf,
        chacha20_ietf_poly1305,
        rc4_md5,
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
}
