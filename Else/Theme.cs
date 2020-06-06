using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIGS.Common;
using AIGS.Helper;
using Stylet;
using System.IO;
using Votrix.Protocol;

using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Media;
using MaterialDesignColors;

namespace Votrix.Else
{
    public class VotrixTheme
    {
        public enum Type
        {
            Light,
            Dark,
        }

        public static bool Set(Type eObj)
        {
            if (Application.Current == null)
                return false;

            ITheme theme = (ITheme)Application.Current.Resources.GetTheme(); ;

            if (eObj == Type.Light)
            {
                theme.SetBaseTheme((IBaseTheme)new MaterialDesignLightTheme());
                theme.PrimaryLight = new ColorPair((Color)ColorConverter.ConvertFromString("#f5f5f5"), (Color)ColorConverter.ConvertFromString("#9d9d9d")); 
                theme.PrimaryMid = new ColorPair((Color)ColorConverter.ConvertFromString("#e8e7e7"), (Color)ColorConverter.ConvertFromString("#FF515050"));
                theme.PrimaryDark = new ColorPair((Color)ColorConverter.ConvertFromString("#2a2d31"), (Color)ColorConverter.ConvertFromString("#FFFFFF"));
            }
            else if (eObj == Type.Dark)
            {
                theme.SetBaseTheme((IBaseTheme)new MaterialDesignDarkTheme());
                theme.PrimaryLight = new ColorPair((Color)ColorConverter.ConvertFromString("#484848"), (Color)ColorConverter.ConvertFromString("#757575"));
                theme.PrimaryMid = new ColorPair((Color)ColorConverter.ConvertFromString("#212121"), (Color)ColorConverter.ConvertFromString("#f5f5f5"));
                theme.PrimaryDark = new ColorPair((Color)ColorConverter.ConvertFromString("#000000"), (Color)ColorConverter.ConvertFromString("#FFFFFF"));
            }

            Application.Current.Resources.SetTheme(theme);
            return true;
        }
    }
}
