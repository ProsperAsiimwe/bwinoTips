using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Exclusive
{
    public class TipStatusViewModel
    {

        public TipStatusViewModel() { }

        public TipStatusViewModel(ExclusiveTip Entity)
        {
            SetFromEntity(Entity);
        }

        public int ExclusiveTipId { get; set; }

        public Status Status { get; set; }


        public ExclusiveTip ParseAsEntity(ExclusiveTip Entity)
        {
            if (Entity == null)
            {
                Entity = new ExclusiveTip();
            }

            Entity.Status = Status;

            return Entity;

        }

        public void SetFromEntity(ExclusiveTip Entity)
        {
            this.ExclusiveTipId = Entity.ExclusiveTipId;
            this.Status = Entity.Status;
        }
    }
}
