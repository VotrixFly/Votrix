using System;
using System.Windows;
using Stylet;

namespace Votrix.Pages
{
    public class MainViewModel : Screen
    {
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
            if(((MainView)this.View).WindowState != WindowState.Maximized)
                ((MainView)this.View).WindowState = WindowState.Maximized;
            else
                ((MainView)this.View).WindowState = WindowState.Normal;
        }

        public void WindowMove()
        {
            ((MainView)this.View).DragMove();
        }
    }
}
