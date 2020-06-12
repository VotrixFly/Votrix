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
using AIGS.Common;

namespace Votrix.Pages
{
    public class RSSViewModel : Screen
    {
        public ObservableCollection<RSS> RSSList { get; set; } = new ObservableCollection<RSS>();
        //错误信息显示
        public delegate void ShowErrmessage(string info);
        public ShowErrmessage Callback_ShowErr;


        public RSSViewModel()
        {
            RSSList = Config.RWRss();
        }

        public void Save()
        {
            string Errmsg = null;
            for (int i = 0; i < RSSList.Count; i++)
            {
                if(RSSList[i].Name.IsBlank())
                    Errmsg = string.Format("第{0}行的订阅名不能为空！", i + 1);
                else if(RSSList[i].Url.IsBlank())
                    Errmsg = string.Format("第{0}行的链接不能为空！", i + 1);

                if(Errmsg.IsNotBlank())
                {
                    Callback_ShowErr(Errmsg);
                    return;
                }
            }
            Config.RWRss(RSSList);
        }

        public void Cancel()
        {
            RSSList = Config.RWRss();
        }

        public void Add()
        {
            RSSList.Add(new RSS());
        }

        public void Delete()
        {
            for (int i = RSSList.Count - 1; i >= 0; i--)
            {
                if (RSSList[i].IsSelect)
                    RSSList.RemoveAt(i);
            }
        }

    }
}
