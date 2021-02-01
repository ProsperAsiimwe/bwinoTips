using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BwinoTips.WebUI.Models.Account
{
    public class ResetPasswordViewModel
    {
        public ResetPasswordViewModel()
        {
            Verb = "Set";
            //UniqueCode = RNGCharacterMask();
          
        }

        [Required]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

        public string Verb { get; set; }


        //private string RNGCharacterMask()

        //{

        //    int maxSize = 7;

        //    int minSize = 7;

        //    char[] chars = new char[62];

        //    //string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        //    string a = "abcdefghijklmnopqrstuvwxyz1234567890";

        //    chars = a.ToCharArray();

        //    int size = maxSize;

        //    byte[] data = new byte[1];

        //    RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

        //    crypto.GetNonZeroBytes(data);

        //    size = maxSize;

        //    data = new byte[size];

        //    crypto.GetNonZeroBytes(data);

        //    StringBuilder result = new StringBuilder(size);

        //    foreach (byte b in data)

        //    {

        //        result.Append(chars[b % (chars.Length - 1)]);

        //    }

        //    return result.ToString();

        //}

    }
}