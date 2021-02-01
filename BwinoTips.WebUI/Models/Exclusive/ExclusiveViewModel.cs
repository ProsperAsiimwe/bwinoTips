using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Enums;
using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BwinoTips.WebUI.Models.Exclusive
{
    public class ExclusiveViewModel
    {

        public ExclusiveViewModel() {
            Date = UgandaDateTime.DateNow();
            Leagues = new List<SelectListItem>();           
        }

        public ExclusiveViewModel(ExclusiveTip Entity)
        {

            SetFromEntity(Entity);
        }

        [Key]
        public int ExclusiveTipId { get; set; }

        [Display(Name = "League")]
        public int LeagueId { get; set; }

        [Required]
        [Display(Name = "Home Team")]
        [StringLength(30)]
        public string HomeTeam { get; set; }

        [Required]
        [Display(Name = "Away Team")]
        [StringLength(30)]
        public string AwayTeam { get; set; }

        [Required]
        [Display(Name = "Tip")]
        [StringLength(30)]
        public string Tip { get; set; }

        [Display(Name = "Result")]
        [StringLength(20)]
        public string Result { get; set; }

        [Required]       
        [Display(Name = "Odd")]
        public double? Odd { get; set; }

        [UIHint("_DateTimePicker")]
        [Required]
        public DateTime Date { get; set; }

        public IEnumerable<SelectListItem> Leagues { get; set; }


        public ExclusiveTip ParseAsEntity(ExclusiveTip Entity)
        {
            if (Entity == null)
            {
                Entity = new ExclusiveTip();
            }

            Entity.HomeTeam = HomeTeam;
            Entity.AwayTeam = AwayTeam;
            Entity.Tip = Tip;
            Entity.Odd = Odd ?? 0;
            Entity.Result = Result;
            Entity.LeagueId = LeagueId;
            Entity.Date = Date;
            //Entity.Status = Status;

            return Entity;
        }

        protected void SetFromEntity(ExclusiveTip Entity)
        {
            this.ExclusiveTipId = Entity.ExclusiveTipId;
            this.HomeTeam = Entity.HomeTeam;
            this.AwayTeam = Entity.AwayTeam;
            this.Tip = Entity.Tip;
            this.Odd = Entity.Odd;
            this.Result = Entity.Result;
            this.LeagueId = Entity.LeagueId;
            this.Date = Entity.Date;
            //this.Status = Entity.Status;
                    
        }

    }
}