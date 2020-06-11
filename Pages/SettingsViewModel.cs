using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;
using AIGS;
using System.Collections.ObjectModel;
using Votrix.Else;
using Votrix.Protocol;

namespace Votrix.Pages
{
    public class SettingsViewModel : Screen
    {
        //配置信息
        public Settings Info { get; set; }
        
        //主题列表和日志等级
        public static Dictionary<int, string> ComboxTheme { get; set; }
        public static Dictionary<int, string> ComboxLogLevel { get; set; }

        //配置改变响应
        public delegate void ChangeSettings(Settings info);
        public ChangeSettings Callback_Change;

        //页面显示
        public bool ShowBase { get; set; } = true;
        public bool ShowPac { get; set; }
        public bool ShowKcp { get; set; }
        public bool ShowRoute { get; set; }

        public SettingsViewModel()
        {
            ComboxTheme = AIGS.Common.Convert.ConverEnumToDictionary(typeof(VotrixTheme.Type), false);
            ComboxLogLevel = AIGS.Common.Convert.ConverEnumToDictionary(typeof(eLogLevel), false);
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

        //取消
        public void Cancel()
        {
            Info = Config.RWSettings();
        }
    }
}
