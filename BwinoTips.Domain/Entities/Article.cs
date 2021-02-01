using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Entities
{
    public class Article
    {
        public Article()
        {
            Published = UgandaDateTime.DateNow();
        }

        [Key]
        public int ArticleId { get; set; }

        [StringLength(1000)]
        public string Author { get; set; }

        public DateTime Published { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(10000)]
        public string Description { get; set; }

    }
}