using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;
using AIGS;
using System.Collections.ObjectModel;
using Votrix.Else;

namespace Votrix.Pages
{
    public class SettingsViewModel : Screen
    {
        public Settings Info { get; set; }
        //主题列表
        public static Dictionary<int, string> ComboxTheme { get; set; }
        //配置改变响应
        public delegate void ChangeSettings(Settings info);
        public ChangeSettings Callback_Change;

        public SettingsViewModel()
        {
            ComboxTheme = AIGS.Common.Convert.ConverEnumToDictionary(typeof(VotrixTheme.Type), false);
            Info = Config.RWSettings();
            return;
        }

        //保存和响应
        public void Save()
        {
            Config.RWSettings(Info);
            if (Callback_Change != null)
                Callback_Change(Info);
        }

        public void Cancel()
        {
            Info = Config.RWSettings();
        }
    }
}
