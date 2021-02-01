using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Free
{
    public class TipStatusViewModel
    {

        public TipStatusViewModel() { }

        public TipStatusViewModel(FreeTip Entity)
        {
            SetFromEntity(Entity);
        }

        public int FreeTipId { get; set; }

        public Status Status { get; set; }


        public FreeTip ParseAsEntity(FreeTip Entity)
        {
            if (Entity == null)
            {
                Entity = new FreeTip();
            }

            Entity.Status = Status;

            return Entity;

        }

        public void SetFromEntity(FreeTip Entity)
        {
            this.FreeTipId = Entity.FreeTipId;
            this.Status = Entity.Status;
        }
    }
}
