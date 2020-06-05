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
        public Config.Base Info { get; set; }
        public static Dictionary<int, string> ComboxTheme { get; set; }

        public SettingsViewModel()
        {
            ComboxTheme = AIGS.Common.Convert.ConverEnumToDictionary(typeof(VotrixTheme.Type), false);
            Info = Config.RWBase();
            return;
        }

        public void Save()
        {
            //保存和响应
            Config.RWBase(Info);
            VotrixTheme.Set((VotrixTheme.Type)Info.Theme);
        }
    }
}
