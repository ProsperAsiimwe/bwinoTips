using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagicApps.Infrastructure.Helpers;

namespace BwinoTips.WebUI.Infrastructure.Helpers
{
    public class CookieHelper
    {
        public void SetCookies(int referenceId)
        {
            ReferenceId = referenceId;
            Authorised = true;
        }

        public bool Authorised {
            get {
                bool _id = bool.Parse(CustomHelper.GetCookieValue("BwinoTips-Authorised", Boolean.FalseString));
                return _id;
            }
            set {
                CustomHelper.CreateCookie("BwinoTips-Authorised", value.ToString());
            }
        }

        public int ReferenceId {
            get {
                int _id = int.Parse(CustomHelper.GetCookieValue("BwinoTips-ReferenceId"));
                return _id;
            }
            set {
                CustomHelper.CreateCookie("BwinoTips-ReferenceId", value.ToString());
            }
        }

        public void Flush()
        {
            CustomHelper.CreateCookie("BwinoTips-Authorised", Boolean.FalseString, -1);
            CustomHelper.CreateCookie("BwinoTips-ReferenceId", "0", -1);
        }
    }
}