using BwinoTips.Domain.Enums;
using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Entities
{
   public class Registration
    {

        public Registration()
        {
            Date = UgandaDateTime.DateNow();
        }

        [Key]
        public int RegistrationId { get; set; }
               
        [StringLength(60)]
        [Display(Name = "Forename(s)")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [StringLength(20)]
        [Display(Name = "Telephone")]
        public string Tel { get; set; }

        [StringLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(20)]
        [Display(Name = "Unique Code")]
        public string UniqueCode { get; set; }

        //[Display(Name = "Status")]
        //public RegStatus RegStatus { get; set; }

        [Display(Name = "Subscription plan")]
        public Plan Plan { get; set; }

        [Display(Name = "Subscription Date")]
        public DateTime? SubscribeDate { get; set; }
              
        [Display(Name = "Request Date")]
        public DateTime Date { get; set; }

        public DateTime? Deleted { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return AsPerson().FullName;
            }
        }

        public Person AsPerson()
        {
            return new Person
            {               
                FirstName = FirstName,
                LastName = LastName,
            };
        }

        public string GetStatus()
        {
            if (Expired)
            {
                return "Expired";
            }else if (!SubscribeDate.HasValue)
            {
                return "Inactive";
            }

            return "Active";
        }

        public string GetStatusCssClass()
        {
            switch (GetStatus())
            {
                case "Expired":
                    return "danger";
                case "Active":
                    return "success";
                case "Inactive":
                    return "warning";
                default:
                    return "info";
            }

        }

        [NotMapped]
        public DateTime? ExpiryDate
        {
            get
            {
                if (!SubscribeDate.HasValue) return (DateTime?)null;
                return Plan == Plan.Weekly ? SubscribeDate.Value.AddDays(7) : Plan == Plan.Monthly ? SubscribeDate.Value.AddMonths(1) : SubscribeDate.Value.AddYears(1);
            }
        }

        [NotMapped]
        public bool Expired
        {
            get
            {
                return ExpiryDate < UgandaDateTime.DateNow();
            }
        }


    }
}
