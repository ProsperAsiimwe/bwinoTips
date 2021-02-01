using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Adverts
{
    public class AdViewModel
    {
        public AdViewModel() { }

        public AdViewModel(Advert Advert)
        {
            setFromEntity(Advert);
        }

        [Key]
        public int AdvertId { get; set; }

        [Required(ErrorMessage = "Advert Image is Required")]
        [Display(Name = "Advert Image")]

        public HttpPostedFileBase[] AdImages { get; set; }

        public Advert ParseAsEntity(Advert Advert)
        {
            if (Advert == null)
            {
                Advert = new Advert();
            }

            return Advert;

        }
        
        public void setFromEntity(Advert Advert)
        {
            this.AdvertId = Advert.AdvertId;

        }

    }
}