using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Models
{
    public class Cart
    {
        private List<CartLine> itemsCollection = new List<CartLine>();

        public void AddItem(ExclusiveTip tip, int quantity)
        {
            CartLine line = itemsCollection.Where(p => p.ExclusiveTip.ExclusiveTipId == tip.ExclusiveTipId).FirstOrDefault();
            if (line == null)
            {
                itemsCollection.Add(new CartLine { ExclusiveTip = tip, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
                      
        }

        public CartLine GetItem(int ExclusiveTipId)
        {
            CartLine line = itemsCollection.FirstOrDefault(p => p.ExclusiveTip.ExclusiveTipId == ExclusiveTipId);
            if (line == null)
            {
                return null;
            }

            return line;
        }

        public bool Validate(int ExclusiveTipId, int available, int quantity)
        {
            var line = itemsCollection.FirstOrDefault(p => p.ExclusiveTip.ExclusiveTipId == ExclusiveTipId);
                      
            // validate quantity
            int currQty = line != null ? line.Quantity : 0;
            currQty += quantity;

            return available >= currQty;
        }

        public void RemoveItem(ExclusiveTip tip)
        {
            itemsCollection.RemoveAll(p => p.ExclusiveTip.ExclusiveTipId == tip.ExclusiveTipId);
        }

        public double computeTotalValue()
        {
            return computeTotal();
        }

        public double computeTotal()
        {
            return itemsCollection.Select(p => p.Odd).Aggregate((x, y) => x * y);
        }

        public IEnumerable<CartLine> Lines
        {
            get { return itemsCollection; }
        }

        public void Clear()
        {          
            itemsCollection.Clear();
        }

   
    }
}
