using BwinoTips.Domain.Enums;
using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Entities
{
    public class Order
    {
        public Order()
        {
            Items = new List<OrderItem>();
            Date = UgandaDateTime.DateNow();
            Status = Status.Pending;
        }

        [Key]
        public int OrderId { get; set; }
        
        [Display(Name = "Status")]
        public Status Status { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }


        [Display(Name = "Total Odds")]
        [NotMapped]
        public double Odd
        {
            get
            {
                //return Items.ToList().Sum(p => p.Odd);
                return Items.ToList().Select(p => p.Odd).Aggregate((x, y) => x * y);
            }
        }

        public string GetStatus()
        {
            return Status.ToString();
        }

        public string GetStatusCssClass()
        {
            switch (Status)
            {
                case Status.Pending:
                    return "warning";
                case Status.Correct:
                    return "success";
                case Status.Wrong:
                    return "danger";
                default:
                    return "primary";
            }

        }

    }

    }