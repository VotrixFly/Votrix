using System;
using System.Windows;
using Stylet;
using AIGS.Common;
using AIGS.Helper;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using Votrix.Else;
using Votrix.Protocol;

namespace Votrix.Pages
{
    public class MainViewModel : Screen
    {
        public Settings Setting;

        public bool ShowViewServerList { get; set; } = true;
        public bool ShowViewRSS { get; set; }
        public bool ShowViewSettings { get; set; }
        public ServerListViewModel VMServerList { get; private set; }
        public SettingsViewModel VMSettings { get; private set; }
        public AboutViewModel VMAbout { get; set; }

        public MainViewModel(ServerListViewModel serverlist, SettingsViewModel settings, AboutViewModel about)
        {
            VMServerList = serverlist;
            VMSettings = settings;
            VMAbout = about;

            //配置读取
            Settings set = Config.RWSettings();
            ChangeSettings(set);
            VMSettings.Callback_Change = ChangeSettings;
            return;
        }

        //修改配置
        public void ChangeSettings(Settings info)
        {
            if(Setting == null || Setting.AutoStart != info.AutoStart)
                SystemHelper.SetPowerBoot("Votrix",info.AutoStart);
            if (Setting == null || Setting.AutoUpdate != info.AutoUpdate)
            { 
                //TODO
            }
            if (Setting == null || Setting.Language != info.Language)
            {
                //TODO
            }
            if (Setting == null || Setting.Theme != info.Theme)
                VotrixTheme.Set((VotrixTheme.Type)info.Theme);
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
        #endregion
    }
}
