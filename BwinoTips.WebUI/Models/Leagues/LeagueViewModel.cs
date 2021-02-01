using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Leagues
{
    public class LeagueViewModel
    {
        public LeagueViewModel() { }

        public LeagueViewModel(League Entity)
        {

            SetFromEntity(Entity);
        }

        public int LeagueId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Country")]
        [StringLength(30)]
        public string Country { get; set; }

        public League ParseAsEntity(League Entity)
        {
            if (Entity == null)
            {
                Entity = new League();
            }

            Entity.Name = Name;
            Entity.Country = Country;
            Entity.Updated = UgandaDateTime.DateNow();
            
            return Entity;
        }

        protected void SetFromEntity(League Entity)
        {
            this.LeagueId = Entity.LeagueId;
            this.Name = Entity.Name;
            this.Country = Entity.Country;
           
        }

    }
}