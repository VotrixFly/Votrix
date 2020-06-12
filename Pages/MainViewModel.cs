using System;
using System.Windows;
using Stylet;
using AIGS.Common;
using AIGS.Helper;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using Votrix.Else;
using Votrix.Protocol;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace Votrix.Pages
{
    public class MainViewModel : Screen
    {
        public string ErrMessage { get; set; }
        public bool ShowErr { get; set; } = false;

        public Settings Setting;
        public bool ShowViewServerList { get; set; } = true;
        public bool ShowViewRSS { get; set; }
        public bool ShowViewSettings { get; set; }
        public ServerListViewModel VMServerList { get; private set; }
        public SettingsViewModel VMSettings { get; private set; }
        public AboutViewModel VMAbout { get; set; }

        public MainViewModel(ServerListViewModel serverlist, SettingsViewModel settings, AboutViewModel about)
        {
            VMAbout = about;
            VMSettings = settings;
            VMSettings.Callback_Change = ChangeSettings;
            VMServerList = serverlist;
            VMServerList.Callback_ShowErr = ShowErrmessage;
            return;
        }

        //运行前加载配置
        protected override void OnViewLoaded()
        {
            if (SystemHelper.IsProcessExist("Votrix"))
            {
                MessageBox.Show("Votrix已经运行！");
                WindowHide();
                WindowClose();
                return;
            }

            //配置读取
            Settings set = Config.RWSettings();
            ChangeSettings(set);
        }

        //获取所选的语言文件
        public string GetLanguageFile(int iSelectLanguageIndex)
        {
            string sBasePath = "Language/";
            switch (iSelectLanguageIndex)
            {
                case 0: return sBasePath + "en.xaml";
                case 1: return sBasePath + "zh_cn.xaml";
                default: return sBasePath + "zh_cn.xaml";
            }
        }

        //修改配置
        public void ChangeSettings(Settings info)
        {
            //开机启动
            if (Setting == null || Setting.AutoStart != info.AutoStart)
                SystemHelper.SetPowerBoot("Votrix",info.AutoStart);
            //自动更新
            if (Setting == null || Setting.AutoUpdate != info.AutoUpdate)
            { 
                //TODO
            }
            //语言
            if (Setting == null || Setting.Language != info.Language)
            {
                foreach (var item in Application.Current.Resources.MergedDictionaries)
                {
                    string sTmp = item.Source.ToString();
                    if(sTmp.IndexOf("Language") >= 0)
                    {
                        int iIndex = Application.Current.Resources.MergedDictionaries.IndexOf(item);
                        Application.Current.Resources.MergedDictionaries[iIndex] = new ResourceDictionary() { Source = new Uri(GetLanguageFile(info.Language), UriKind.RelativeOrAbsolute) };
                        break;
                    }
                }
            }
            //托盘显示(刚启动时，如果设置了托盘显示，并且至少配置了一个服务器，则隐藏主窗口)
            if (Setting == null)
            {
                if (VMServerList.ServerList.Count > 0 && info.SmallStart)
                    WindowHide();
            }
            //主题
            if (Setting == null || Setting.Theme != info.Theme)
                VotrixTheme.Set((VotrixTheme.Type)info.Theme);
            //端口、UDP、局域网分享
            if (Setting == null || Setting.PortSocks5 != info.PortSocks5 ||
                Setting.PortHttp != info.PortHttp ||
                Setting.SupportUDP != info.SupportUDP ||
                Setting.ShareAreaNetwork != info.ShareAreaNetwork)
            {
                //TODO
            }
            if (Setting == null)
                Setting = new Settings();
            AIGS.Common.Convert.ConverClassBToClassA<Settings, Settings>(info, ref Setting);
            return;
        }

        #region 托盘响应
        public void AddServerFromQrcode()
        {
            VMServerList.AddServerFromQrcode();
            WindowShow();
        }

        public void AddServerFromUrl()
        {
            VMServerList.AddServerFromUrl();
            WindowShow();
        }
        #endregion

        #region 页面显示
        //隐藏所有页面
        private void HideAllPage()
        {
            ShowViewServerList = false;
            ShowViewRSS = false;
            ShowViewSettings = false;
        }

        //显示服务列表
        public void ShowPageServerList()
        {
            HideAllPage();
            ShowViewServerList = true;
        }

        //显示设置
        public void ShowPageSettings()
        {
            HideAllPage();
            ShowViewSettings = true;
        }

        //显示订阅
        public void ShowPageRSS()
        {
            HideAllPage();
            ShowViewRSS = true;
        }

        //显示关于
        public async void ShowAbout()
        {
            var view = new AboutView() { DataContext = VMAbout };
            var result = await DialogHost.Show(view, "RootDialog");
        }
        #endregion

        #region 窗口按钮
        //关闭
        public void WindowClose()
        {
            RequestClose();
        }

        //最小化
        public void WindowMin()
        {
            ((MainView)this.View).WindowState = WindowState.Minimized;
        }

        //最大化
        public void WindowMax()
        {
            AIGS.Helper.ScreenShotHelper.MaxWindow((MainView)this.View);
        }

        //移动
        public void WindowMove()
        {
            ((MainView)this.View).DragMove();
        }

        //显示窗口
        public void WindowShow()
        {
            Application.Current.MainWindow.Show();
        }

        //隐藏窗口
        public void WindowHide()
        {
            Application.Current.MainWindow.Hide();
        }
        #endregion

        #region 错误窗口
        public void ShowErrmessage(string message)
        {
            ShowErr = true;
            ErrMessage = message;
        }

        public void HideErrmessage()
        {
            ShowErr = false;
        }
        #endregion
    }
}
