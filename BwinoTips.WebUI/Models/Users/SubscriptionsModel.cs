using BwinoTips.Domain.Entities;
using BwinoTips.Domain.Enums;
using BwinoTips.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BwinoTips.WebUI.Models.Users
{
    public class SubscriptionsModel
    {

        public SubscriptionsModel() {
            SubscribeDate = UgandaDateTime.DateNow();
            UniqueCode = RNGCharacterMask();
        }

        public SubscriptionsModel(ApplicationUser Entity)
        {
            SetFromEntity(Entity);
        }

        [Key]
        public int DisplayId { get; set; }

        [Display(Name = "Subscription plan")]
        [Required]
        public Plan Plan { get; set; }

        [StringLength(20)]
        [Display(Name = "Unique Code")]
        [Required]
        public string UniqueCode { get; set; }

        public DateTime? SubscribeDate { get; set; }


        public ApplicationUser ParseAsEntity(ApplicationUser Entity)
        {
            if (Entity == null)
            {
                Entity = new ApplicationUser();
            }

            Entity.Plan = Plan;
            Entity.UniqueCode = UniqueCode;
            Entity.SubscribeDate = UgandaDateTime.DateNow();

            return Entity;

        }

        public void SetFromEntity(ApplicationUser Entity)
        {
            this.DisplayId = Entity.DisplayId;
            this.Plan = Entity.Plan;
            this.UniqueCode = Entity.UniqueCode;
            this.SubscribeDate = Entity.SubscribeDate ?? null;
        }

        private string RNGCharacterMask()
        {

            int maxSize = 7;

            //int minSize = 7;

            char[] chars = new char[62];

            //string a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            string a = "abcdefghijklmnopqrstuvwxyz1234567890";

            chars = a.ToCharArray();

            int size = maxSize;

            byte[] data = new byte[1];

            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();

            crypto.GetNonZeroBytes(data);

            size = maxSize;

            data = new byte[size];

            crypto.GetNonZeroBytes(data);

            StringBuilder result = new StringBuilder(size);

            foreach (byte b in data)

            {

                result.Append(chars[b % (chars.Length - 1)]);

            }

            return result.ToString();

        }

    }
}
