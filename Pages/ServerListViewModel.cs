using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;
using AIGS.Helper;
using AIGS.Common;
using AIGS;
using System.Collections.ObjectModel;
using System.Collections;
using Votrix.Else;
using Votrix.Protocol;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Windows.Controls;
using System.Windows;
using System.Drawing;

namespace Votrix.Pages
{
    public class ServerListViewModel : Screen
    {
        public bool ShowSS { get; set; } = true;
        public bool ShowVMess { get; set; }
        public bool ShowShocks { get; set; }

        public int SelectIndex { get; set; }
        public Server CurServer { get; set; }
        public ObservableCollection<Server> ServerList { get; set; } = new ObservableCollection<Server>();


        public static Dictionary<int, string> ComboxSecuritySS { get; set; }
        public static Dictionary<int, string> ComboxSecurityVmess { get; set; }
        public static Dictionary<int, string> ComboxSecuritySocks { get; set; }
        public static Dictionary<int, string> ComboxNetwork { get; set; }

        public ServerListViewModel()
        {
            ComboxSecuritySS = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eSecuritySS), false);
            ComboxSecurityVmess = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eSecurityVmess), false); 
            ComboxSecuritySocks = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eSecuritySocks), false);
            ComboxNetwork = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eNetwork), false);

            ServerList = Config.RWServers();
        }

        public void SetProtocolView(eProtocolType eType)
        {
            switch(eType)
            {
                case eProtocolType.shadowsocks: ShowSS = true;break;
                case eProtocolType.vmess: ShowVMess = true; break;
                case eProtocolType.socks: ShowShocks = true; break;
            }
        }

        #region 操作服务器
        //添加服务器
        public void AddServer(Server oObj = null)
        {
            if (oObj == null)
                oObj = new Server();
            ServerList.Add(oObj);
            SelectIndex = ServerList.Count - 1;
            
            //显示相应的协议配置页面
            SetProtocolView(CurServer.PType);
            //保存到配置文件中
            Config.RWServers(ServerList);
        }

        //从黏贴导入
        public void AddServerFromUrl(string sQRCodeText = null)
        {
            string sText = null;
            bool bIsFromQrcode = false;
            if (sQRCodeText.IsNotBlank())
            {
                sText = sQRCodeText;
                bIsFromQrcode = true;
            }
            else
            {
                if (SystemHelper.IsClipBoardEmpty())
                {
                    MessageBox.Show("导入失败！粘贴板为空" + sText);
                    return;
                }
                sText = SystemHelper.GetClipBoardData<string>();
            }

            //url解析
            Server oObj = Tool.LoadURLString(sText);
            if (oObj == null)
            {
                if(bIsFromQrcode)
                    MessageBox.Show("导入失败！二维码不正确！");
                else
                    MessageBox.Show("导入失败！粘贴板内容：" + sText);
                return;
            }
            AddServer(oObj);
        }

        //扫描二维码导入
        public void AddServerFromQrcode()
        {
            try
            {
                foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
                {
                    using (Bitmap fullImage = new Bitmap(screen.Bounds.Width, screen.Bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(fullImage))
                        {
                            g.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, fullImage.Size, CopyPixelOperation.SourceCopy);
                        }
                        int maxTry = 10;
                        for (int i = 0; i < maxTry; i++)
                        {
                            int marginLeft = (int)((double)fullImage.Width * i / 2.5 / maxTry);
                            int marginTop = (int)((double)fullImage.Height * i / 2.5 / maxTry);
                            Rectangle cropRect = new Rectangle(marginLeft, marginTop, fullImage.Width - marginLeft * 2, fullImage.Height - marginTop * 2);
                            Bitmap target = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);
                            double imageScale = (double)screen.Bounds.Width / (double)cropRect.Width;
                            using (Graphics g = Graphics.FromImage(target))
                            {
                                g.DrawImage(fullImage, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
                            }
                            BitmapLuminanceSource source = new BitmapLuminanceSource(target);
                            BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
                            QRCodeReader reader = new QRCodeReader();
                            Result result = reader.decode(bitmap);
                            if (result != null)
                            {
                                AddServerFromUrl(result.Text);
                                return;
                            }
                        }
                    }
                    MessageBox.Show("导入失败！扫描不到二维码！");
                }
            }
            catch { MessageBox.Show("导入失败！未知错误！"); }
        }

        //删除服务器
        public void DelServer()
        {
            if (SelectIndex < 0)
                return;
            int iIndex = SelectIndex;
            ServerList.RemoveAt(SelectIndex);

            //删除之后SelectIndex自动变为-1，将当前选项改为下一个服务
            if (iIndex > ServerList.Count - 1)
                iIndex -= 1;
            SelectIndex = iIndex;

            //保存
            Config.RWServers(ServerList);
        }

        //启动服务器
        public void StartServer()
        {

        }

        //关闭服务器
        public void CloseServer()
        {

        }

        //重启服务器
        public void RestartServer()
        {

        }

        //选择服务器
        public void SelectServer()
        {
            if (SelectIndex < 0)
                return;
            CurServer = AIGS.Common.Convert.ConverClassBToClassA<Server, Server>(ServerList[SelectIndex]);
            SetProtocolView(CurServer.PType);
        }

        //保存服务器
        public void SaveServer()
        {
            if (CurServer == null || SelectIndex < 0)
                return;

            if (ShowSS)
                CurServer.PType = eProtocolType.shadowsocks;
            if (ShowShocks)
                CurServer.PType = eProtocolType.socks;
            if (ShowVMess)
                CurServer.PType = eProtocolType.vmess;
            CurServer.Image = Server.GetImageName(CurServer.PType);

            ServerList[SelectIndex] = AIGS.Common.Convert.ConverClassBToClassA<Server, Server>(CurServer);
            Config.RWServers(ServerList);
            return;
        }

        //取消修改
        public void CancelChange()
        {
            SelectServer();
        }

        
        #endregion

    }
}
