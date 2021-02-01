using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Models
{
    public class CartLine
    {
        public ExclusiveTip ExclusiveTip { get; set; }

        public int Quantity { get; set; }

        public double Odd
        {
            get
            {
                return (ExclusiveTip.Odd);
            }
        }
    }
}
