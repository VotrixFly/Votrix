using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votrix.Else;
using AIGS.Common;
using AIGS.Helper;

namespace Votrix.Protocol
{
    public class Tool
    {
        public static string VMESS_STRING_FLAG = "vmess://";
        public static string SS_STRING_FLAG = "ss://";
        public static string SOCKS_STRING_FLAG = "socks://";


        #region 导入链接
        //导入链接
        public static Server LoadURLString(string sText)
        {
            if (sText.IsBlank())
                return null;

            if (sText.StartsWith(VMESS_STRING_FLAG))
            {
                sText = sText.Substring(VMESS_STRING_FLAG.Length);
                sText = Base64Decode(sText);

                VmessQRCode vmessQRCode = JsonHelper.ConverStringToObject<VmessQRCode>(sText);
                if (vmessQRCode == null)
                    return null;
                Server ret = new Server()
                {
                    PType = eProtocolType.Vmess,
                    Name = vmessQRCode.ps,
                    Address = vmessQRCode.add,
                    Port = vmessQRCode.port,
                    UUID = vmessQRCode.id,
                    AlterID = vmessQRCode.aid,
                    Host = vmessQRCode.host,
                    Path = vmessQRCode.path,
                    TLSEnable = vmessQRCode.tls == "tls",
                    Network = AIGS.Common.Convert.ConverStringToEnum(vmessQRCode.net, typeof(eNetwork), (int)eNetwork.ws, true),
                    Type = vmessQRCode.type,
                };
                return ret;
            }
            if (sText.StartsWith(SS_STRING_FLAG))
            {
                sText = sText.Substring(SS_STRING_FLAG.Length);
                string[] oTmp = sText.Split("#");
                string sName = oTmp.Length == 2 ? oTmp[1] : null;
                
                int index = oTmp[0].IndexOf("@");
                if(index > 0)
                    sText = Base64Decode(oTmp[0].Substring(0, index)) + oTmp[0].Substring(index, oTmp[0].Length - index);
                else
                    sText = Base64Decode(oTmp[0]);

                string[] oStrs = sText.Split('@');
                if (oStrs.Length != 2)
                    return null;
                string[] oArr1 = oStrs[0].Split(':');
                string[] oArr2 = oStrs[1].Split(':');
                if (oArr1.Length != 2 || oArr2.Length != 2)
                    return null;
                Server ret = new Server()
                {
                    PType = eProtocolType.SS,
                    Name = sName,
                    Address = oArr2[0],
                    Port = oArr2[1],
                    SecuritySS = AIGS.Common.Convert.ConverStringToEnum(oArr1[0].Replace('-','_'), typeof(eSecuritySS), (int)eSecuritySS.aes_128_cfb, true),
                    Password = oArr1[1],
                };
                return ret;
            }
            if (sText.StartsWith(SOCKS_STRING_FLAG))
            {
                sText = sText.Substring(SOCKS_STRING_FLAG.Length);
                string[] oTmp = sText.Split("#");
                string sName = oTmp.Length == 2 ? oTmp[1] : null;

                int index = oTmp[0].IndexOf("@");
                if (index < 0)
                    sText = Base64Decode(oTmp[0]);
                //"name:password@sock5.com:10808"
                string[] oStrs = sText.Split('@');
                if (oStrs.Length != 2)
                    return null;
                string[] oArr1 = oStrs[0].Split(':');
                string[] oArr2 = oStrs[1].Split(':');
                if (oArr1.Length != 2 || oArr2.Length != 2)
                    return null;
                Server ret = new Server()
                {
                    PType = eProtocolType.Socks,
                    Name = sName,
                    Address = oArr2[0],
                    Port = oArr2[1],
                    UserName = oArr1[0],
                    Password = oArr1[1],
                };
                return ret;
            }

            return null;
        }
        #endregion

        #region 工具
        private static string Base64Decode(string plainText)
        {
            try
            {
                plainText = plainText.Trim()
                  .Replace("\n", "")
                  .Replace("\r\n", "")
                  .Replace("\r", "")
                  .Replace(" ", "");
                if (plainText.Length % 4 > 0)
                    plainText = plainText.PadRight(plainText.Length + 4 - plainText.Length % 4, '=');

                byte[] data = System.Convert.FromBase64String(plainText);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion
    }
}
