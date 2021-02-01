using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BwinoTips.Domain.Models;

namespace BwinoTips.Domain.Entities
{
   public class Highlight
    {
        [Key]
        public int HighlightId { get; set; }

        public Highlight()
        {
            UploadDate = UgandaDateTime.DateNow();
        }

        [Display(Name = "Title")]
        [StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Url")]
        [StringLength(200)]
        public string Url { get; set; }

        [Display(Name = "Competition")]
        [StringLength(50)]
        public string Competition { get; set; }

        [Display(Name = "Image")]
        [StringLength(1000)]
        public string FileName { get; set; }


        public DateTime UploadDate { get; set; }
    }
}
