using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;
using System.Configuration;

namespace BwinoTips.Domain.Entities
{
   public class BlogPost
    {
        public BlogPost()
        {
            Date = UgandaDateTime.DateNow();
        }

        [Key]
        public int BlogPostId { get; set; }
               
        [Required]
        [Display(Name = "Title")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Author")]
        [StringLength(50)]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Content")] 
        [StringLength(10000)]       
        public string Content { get; set; }

        [StringLength(20)]
        public string FileName { get; set; }

        [Display(Name = "Image")]
        [StringLength(100)]
        public string FolderPath { get; set; }
       
        public DateTime Date { get; set; }
       
    }
}
