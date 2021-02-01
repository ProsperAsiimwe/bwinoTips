using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Entities
{
    public class OrderItem
    {

        public OrderItem()
        {

        }
        public OrderItem(int ExclusiveTipId, int OrderId)
        {
            this.ExclusiveTipId = ExclusiveTipId;
            this.OrderId = OrderId;           
        }

        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("ExclusiveTip")]
        public int ExclusiveTipId { get; set; }

        public int Quantity { get; set; }

        public virtual ExclusiveTip ExclusiveTip { get; set; }

        public virtual Order Order { get; set; }

        [NotMapped]
        public double Odd
        {
            get
            {
                return (ExclusiveTip.Odd);
            }
        }

    }
}
