using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BwinoTips.Domain.Entities;

namespace BwinoTips.WebUI.Models.Highlights
{
    public class HightlightViewModel
    {
        public HightlightViewModel() { }

        public HightlightViewModel(Highlight Highlight)
        {
            setFromHighlight(Highlight);
        }

        [Key]
        public int HighlightId { get; set; }

        [Display(Name = "Title")]
        [StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Url")]
        [StringLength(200)]
        public string Url { get; set; }

        [Display(Name = "Competition")]
        [StringLength(50)]
        public string Competition { get; set; }

        [Required(ErrorMessage = "Highlight Art is Required")]
        [Display(Name = "Highlight Art")]
        public HttpPostedFileBase[] HighlightArts { get; set; }

        public Highlight ParseAsEntity(Highlight Highlight)
        {
            if (Highlight == null)
            {
                Highlight = new Highlight();
            }

            Highlight.Title = Title;
            Highlight.Url = Url;
            Highlight.Competition = Competition;

            return Highlight;

        }



        public void setFromHighlight(Highlight Highlight)
        {
            this.HighlightId = Highlight.HighlightId;
            this.Title = Highlight.Title;
            this.Url = Highlight.Url;
            this.Competition = Highlight.Competition;

        }
    }
}