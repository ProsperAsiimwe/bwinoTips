using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Entities
{
  public class Advert
    {
        public Advert()
        {
            UploadDate = UgandaDateTime.DateNow();
        }

        [Key]
        public int AdvertId { get; set; }     

        [Display(Name = "Image")]
        [StringLength(1000)]
        public string FileName { get; set; }


        public DateTime UploadDate { get; set; }
    }
}
