using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagicApps.Models;
using BwinoTips.Domain.Entities;

namespace BwinoTips.WebUI.Models.Users
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public SearchUsersModel SearchModel { get; set; }

        public string[] Roles { get; set; }
    }
}