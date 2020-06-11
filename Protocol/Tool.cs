using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votrix.Else;
using AIGS.Common;
using AIGS.Helper;
using System.IO;
using System.Diagnostics;

namespace Votrix.Protocol
{
    public class Tool
    {
        #region 变量
        public static string VMESS_STRING_FLAG = "vmess://";
        public static string SS_STRING_FLAG = "ss://";
        public static string SOCKS_STRING_FLAG = "socks://";
        public static string V2RAY_CORE_DIR = SystemHelper.GetExeDirectoryName() + "\\v2ray_core\\";
        public static string V2RAY_CORE_EXE = V2RAY_CORE_DIR + "v2ray.exe";
        public static string V2RAY_CORE_LOG_ACCESS = V2RAY_CORE_DIR + "Vaccess.log";
        public static string V2RAY_CORE_LOG_ERROR = V2RAY_CORE_DIR + "Verror.log";
        public static string V2RAY_CORE_CONFIG = V2RAY_CORE_DIR + "config.json";

        private static Process V2rayProcess = null;
        #endregion

        #region URL、二维码转换为Server
        //导入链接
        public static Server LoadURLString(string sText)
        {
            if (sText.IsBlank())
                return null;

            if (sText.StartsWith(VMESS_STRING_FLAG))
            {
                sText = sText.Substring(VMESS_STRING_FLAG.Length);
                sText = StringHelper.Base64Decode(sText);

                VmessUrl VmessUrl = JsonHelper.ConverStringToObject<VmessUrl>(sText);
                if (VmessUrl == null)
                    return null;
                Server ret = new Server()
                {
                    PType = eProtocolType.vmess,
                    Name = VmessUrl.ps,
                    Address = VmessUrl.add,
                    Port = VmessUrl.port,
                    UUID = VmessUrl.id,
                    AlterID = VmessUrl.aid,
                    Host = VmessUrl.host,
                    Path = VmessUrl.path,
                    TLSEnable = VmessUrl.tls == "tls",
                    Network = AIGS.Common.Convert.ConverStringToEnum(VmessUrl.net, typeof(eNetwork), (int)eNetwork.ws, true),
                    Type = VmessUrl.type,
                };
                return ret;
            }
            if (sText.StartsWith(SS_STRING_FLAG))
            {
                sText = sText.Substring(SS_STRING_FLAG.Length);
                string[] oTmp = sText.Split("#");
                string sName = oTmp.Length == 2 ? oTmp[1] : null;

                int index = oTmp[0].IndexOf("@");
                if (index > 0)
                    sText = StringHelper.Base64Decode(oTmp[0].Substring(0, index)) + oTmp[0].Substring(index, oTmp[0].Length - index);
                else
                    sText = StringHelper.Base64Decode(oTmp[0]);

                string[] oStrs = sText.Split('@');
                if (oStrs.Length != 2)
                    return null;
                string[] oArr1 = oStrs[0].Split(':');
                string[] oArr2 = oStrs[1].Split(':');
                if (oArr1.Length != 2 || oArr2.Length != 2)
                    return null;
                Server ret = new Server()
                {
                    PType = eProtocolType.shadowsocks,
                    Name = sName,
                    Address = oArr2[0],
                    Port = oArr2[1],
                    SecuritySS = AIGS.Common.Convert.ConverStringToEnum(oArr1[0].Replace('-', '_'), typeof(eSecuritySS), (int)eSecuritySS.aes_128_cfb, true),
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
                    sText = StringHelper.Base64Decode(oTmp[0]);
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
                    PType = eProtocolType.socks,
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



        #region 启动
        //检查引擎是否存在
        public static bool CheckV2ray()
        {
            if (File.Exists(V2RAY_CORE_EXE))
                return true;
            return false;
        }

        //启动引擎
        public static bool StartV2ray(DataReceivedEventHandler pRecieveFuc)
        {
            if (!CheckV2ray())
                return false;

            CmdHelper.KillProcess("v2ray");
            if (CmdHelper.StartExe2(ref V2rayProcess, V2RAY_CORE_EXE, null, null, pRecieveFuc, false) != 0)
                return false;
            return true;
        }
        #endregion


        #region 生成v2ray配置文件
        public static bool CreatV2rayConfig(Server oObject, Settings bBase)
        {
            V2rayConfig config = JsonHelper.ConverStringToObject<V2rayConfig>(SystemHelper.GetEmbedText("Votrix.Protocol.V2rayTemplate.json"));
            config.SetLog((eLogLevel)bBase.Loglevel, bBase.SaveLog);
            config.SetApi("api", "StatsService");
            config.SetPolicy(true, true);
            //DNS
            //config.SetDns(params string[] sServices);
            //路由设置
            //config.SetRoutingDomainStrategy(eDomainStrategy eObj);
            //config.AddRoutingRulesItem(string sType, string sPort, string[] sinboundTag, string sOutboundTag, string[] sIP, string[] sDomain);

            ////传入连接配置
            //string[] sSniffingDestOverride = {"http","tls" };
            //Inbounds inItem1 = config.CreatInbound("proxy", int.Parse(bBase.PortSocks5), "socks", bBase.ShareAreaNetwork, bBase.OpenSniffing, sSniffingDestOverride, "noauth", bBase.SupportUDP);
            //Inbounds inItem2 = config.CreatInbound("api", 53543, "dokodemo-door", false, false, null, null, false, null, "127.0.0.1");
            ////config.AddInboundSettingsClient();
            ////config.SetStreamSettingsBase();
            //config.inbounds.Add(inItem1);
            //config.inbounds.Add(inItem2);

            ////传出连接配置
            //Outbounds outItem1 = config.CreatOutbounds("proxy", "vmess", bBase.OpenMux, 8, sSettingsResponseType: null);
            //config.AddOutboundsVnextItem(ref outItem1, oObject.Address, int.Parse(oObject.Port), new List<UsersItem>() { new UsersItem() {
            //    id = oObject.UUID,
            //    alterId = int.Parse(oObject.AlterID),
            //    email = "t@t.tt",
            //    security = AIGS.Common.Convert.ConverEnumToString(oObject.SecurityVmess, typeof(eSecurityVmess), (int)(eSecurityVmess.auto)).Replace('_','-')
            //    } });
            //StreamSettings stream = outItem1.streamSettings;
            //config.SetStreamSettingsBase(ref stream, AIGS.Common.Convert.ConverEnumToString(oObject.Network, typeof(eNetwork), (int)(eNetwork.ws)).Replace('_', '-'),
            //    oObject.TLSEnable ? "tls" : "",
            //    oObject.TLSEnable ? oObject.AllowInsecure : false,
            //    oObject.TLSEnable ? oObject.Address : null);



            return true;
        }
        #endregion





        

    }
}
