using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BwinoTips.Domain.Models;

namespace BwinoTips.WebUI.Models.Users
{
    public class CreatePersonModel
    {
        [StringLength(4)]
        [Display(Name = "Title")]
        public string TitleId { get; set; }

        [Display(Name = "Forename(s)")]
        [Required]
        [StringLength(60)]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [UIHint("_PhoneNo")]
        [StringLength(20)]
        [Required]
        [Display(Name = "Telephone No")]
        public string PhoneNumber { get; set; }

        [Required]
        [UIHint("_Email")]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        public string[] Titles { get; set; }

        public virtual void SetLists()
        {
            this.Titles = Person.GetPersonTitles();
        }

        public Person Get()
        {
            return new Person {
                Title = TitleId,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                PhoneNumber = PhoneNumber
            };
        }

        public void Set(Person person)
        {
            this.TitleId = person.Title;
            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.PhoneNumber = person.PhoneNumber;
            this.Email = person.Email;
        }
    }
}