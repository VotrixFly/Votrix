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
