using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Feature.Accounts
{
    public struct Templates
    {
        public struct Users
        {
            public static readonly ID DataFolder = new ID("{7FFF6637-B37F-4036-B3E3-745B1301F0CA}");
            public static readonly ID ID = new ID("{4BBA3F0F-0791-4033-AE68-30848F229BCF}");
        }
        public struct User
        {
            public static readonly ID ID = new ID("{9434A464-A583-491B-AD18-59E505083FB9}");
            public struct Fields
            {
                public static readonly ID Username = new ID("{B7C61562-65DA-4729-A58A-4E871DEDE65F}");
                public static readonly ID Email = new ID("{A963294C-7FA3-4633-95BB-606E7BB663D5}");
            }
        }
    }
}