using System;
using System.Windows;
using Stylet;
using AIGS;

namespace Votrix.Pages
{
    public class MainViewModel : Screen
    {
        public ServerListViewModel VMServerList { get; private set; }
        public MainViewModel(ServerListViewModel serverlist)
        {
            VMServerList = serverlist;
        }


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
    }
}
