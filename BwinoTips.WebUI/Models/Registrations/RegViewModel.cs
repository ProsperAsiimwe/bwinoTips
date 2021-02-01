using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Models;
using BwinoTips.WebUI.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BwinoTips.WebUI.Models.Registrations
{
    public class RegViewModel
    {

        public RegViewModel()
        {                      
        }

        public RegViewModel(Registration Entity)
        {

            SetFromEntity(Entity);
        }

        [Key]
        public int RegistrationId { get; set; }

       
        [StringLength(60)]
        [Display(Name = "Forename(s)")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Surname")]
        [Required]
        public string LastName { get; set; }

        [StringLength(20)]
        [Display(Name = "Telephone")]
        [Required]
        public string Tel { get; set; }

        [StringLength(50)]
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        public Registration ParseAsEntity(Registration Entity)
        {
            if (Entity == null)
            {
                Entity = new Registration();
            }

           
            Entity.FirstName = FirstName;
            Entity.LastName = LastName;
            Entity.Tel = Tel;
            Entity.Email = Email;
           
            return Entity;
        }

        protected void SetFromEntity(Registration Entity)
        {
            this.RegistrationId = Entity.RegistrationId;           
            this.FirstName = Entity.FirstName;
            this.Tel = Entity.Tel;
            this.Email = Entity.Email;
          
        }


    }
}