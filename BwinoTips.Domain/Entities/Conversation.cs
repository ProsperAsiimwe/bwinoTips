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
    public class Conversation
    {
        public Conversation()
        {
            Status = MessageStatus.Sent;
            Created = UgandaDateTime.DateNow();
        }

        public enum MessageStatus
        {
            Sent,
            Delivered
        }

        [Key]
        public int ConversationId { get; set; }

        [StringLength(128)]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        [StringLength(128)]
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        [StringLength(1000)]
        public string Message { get; set; }
        public MessageStatus Status { get; set; }
        public DateTime Created { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public virtual ApplicationUser Receiver { get; set; }
    }
}
