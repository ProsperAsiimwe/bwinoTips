using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BwinoTips.Domain.Models;
using BwinoTips.Domain.Enums;

namespace BwinoTips.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            References = new List<Reference>();
            //Registrations = new List<Registration>();
          
        }

        [Display(Name = "User Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DisplayId { get; set; }
              
        [StringLength(4)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [StringLength(60)]
        [Display(Name = "Forename(s)")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Surname")]
        public string LastName { get; set; }
        
        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName {
            get {
                return AsPerson().FullName;
            }
        }

        [Display(Name = "Subscription plan")]
        public Plan Plan { get; set; }

        [StringLength(20)]
        [Display(Name = "Unique Code")]
        public string UniqueCode { get; set; }

        [Display(Name = "Subscription Date")]
        public DateTime? SubscribeDate { get; set; }

        // Virtuals (related entities)
        public virtual ICollection<Reference> References { get; set; }

        //public virtual ICollection<Registration> Registrations { get; set; }

        public Person AsPerson()
        {
            return new Person {
                Title = Title,
                FirstName = FirstName,
                LastName = LastName,
            };
        }

        public string UserId()
        {
            return DisplayId.ToString();
        }

        public string DocFolder()
        {
            string docRoot = System.Configuration.ConfigurationManager.AppSettings["Settings.Site.DocFolder"];
            return string.Format(@"{0}\Users\{1}", docRoot, DisplayId);
        }

        public string GetStatus()
        {
            if (Expired)
            {
                return "Expired";
            }
            else if (!SubscribeDate.HasValue)
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
                return Plan == Plan.Trial ? SubscribeDate.Value.AddDays(3) : Plan == Plan.Weekly ? SubscribeDate.Value.AddDays(7) : Plan == Plan.Monthly ? SubscribeDate.Value.AddMonths(1) : SubscribeDate.Value.AddYears(1);
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

        [NotMapped]
        public bool Active
        {
            get
            {
                return GetStatus() == "Active";
            }
        }

        [NotMapped]
        public bool Inactive
        {
            get
            {
                return GetStatus() == "Inactive";
            }
        }

        [NotMapped]
        public string subDate
        {
            get
            {
                return SubscribeDate.ToString();
            }
        }

        [NotMapped]
        public string expDate
        {
            get
            {
                return ExpiryDate.ToString();
            }
        }

        public bool HasMaxReferees()
        {
            return (References.Where(x => !x.OptedOutDate.HasValue).Count() >= 2);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

       
       
    }
}
