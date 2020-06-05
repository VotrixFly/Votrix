using System;
using System.Windows;
using Stylet;
using AIGS;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using Votrix.Else;

namespace Votrix.Pages
{
    public class MainViewModel : Screen
    {
        public bool ShowViewServerList { get; set; } = true;
        public bool ShowViewRSS { get; set; }
        public bool ShowViewSettings { get; set; }

        public ServerListViewModel VMServerList { get; private set; }
        public SettingsViewModel VMSettings { get; private set; }
        public AboutViewModel VMAbout { get; set; }

        public Config.Base BaseConfig { get; set; }

        public MainViewModel(ServerListViewModel serverlist,
                            SettingsViewModel settings,
                            AboutViewModel about)
        {
            VMServerList = serverlist;
            VMSettings = settings;
            VMAbout = about;

            BaseConfig = Config.RWBase();
            VotrixTheme.Set((VotrixTheme.Type)BaseConfig.Theme);
            return;
        }

        #region 页面显示
        private void HideAllPage()
        {
            ShowViewServerList = false;
            ShowViewRSS = false;
            ShowViewSettings = false;
        }

        public void ShowPageServerList()
        {
            HideAllPage();
            ShowViewServerList = true;
        }

        public void ShowPageSettings()
        {
            HideAllPage();
            ShowViewSettings = true;
        }

        public void ShowPageRSS()
        {
            HideAllPage();
            ShowViewRSS = true;
        }

        public async void ShowAbout()
        {
            var view = new AboutView() { DataContext = VMAbout };
            var result = await DialogHost.Show(view, "RootDialog");
        }
        #endregion

        #region 窗口按钮
        public void WindowClose()
        {
            RequestClose();
        }

        public void WindowMin()
        {
            ((MainView)this.View).WindowState = WindowState.Minimized;
        }

        public void WindowMax()
        {
            AIGS.Helper.ScreenShotHelper.MaxWindow((MainView)this.View);
        }

        public void WindowMove()
        {
            ((MainView)this.View).DragMove();
        }
        #endregion


        
    }
}
