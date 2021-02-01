using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.BlogPosts
{
    public class BlogPostListViewModel
    {
        public IEnumerable<BlogPost> BlogPosts { get; set; }
    }
}