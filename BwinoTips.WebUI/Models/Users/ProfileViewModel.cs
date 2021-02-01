using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Models;

namespace BwinoTips.WebUI.Models.Users
{
    public class ProfileViewModel : CreatePersonModel
    {
        public ProfileViewModel()
        {
            Activate = true;
        }

        public ProfileViewModel(ApplicationUser user)
        {
            this.SetFromEntity(user);
        }

        public string EditUserId { get; set; }

        public string UserId { get; set; }

        [UIHint("_DatePicker")]
        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }

        [UIHint("_Checkbox")]
        [Display(Name = "Activate user?")]
        public bool? Activate { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public ApplicationUser User { get; set; }

        public ApplicationUser ParseAsEntity(ApplicationUser user)
        {
            user.Title = TitleId;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;
            user.PhoneNumber = PhoneNumber;
            user.UserName = PhoneNumber;
                   

            if (Activate.HasValue) {
                user.EmailConfirmed = Activate.Value;
            }

            return user;
        }

        public void SetFromEntity(ApplicationUser user)
        {
            this.User = user;
            this.UserId = user.Id;
            this.TitleId = user.Title;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;          
            this.Activate = user.EmailConfirmed;
            
          
        }

        public void SetLists(string editUserId)
        {
            base.SetLists();
            this.EditUserId = editUserId;
        }
    }
}