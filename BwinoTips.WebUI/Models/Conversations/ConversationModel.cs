using BwinoTips.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static BwinoTips.Domain.Entities.Conversation;

namespace BwinoTips.WebUI.Models.Conversations
{
    public class ConversationModel
    {
        public ConversationModel() { }
        public ConversationModel(Conversation entity)
        {
            ConversationId = entity.ConversationId;
            ReceiverId = entity.Receiver.DisplayId;
            SenderId = entity.Sender.DisplayId;
            Status = entity.Status;
            Message = entity.Message;
        }

        public int ConversationId { get; set; }

        public int ReceiverId { get; set; }

        public int SenderId { get; set; }

        public string Message { get; set; }

        public MessageStatus Status { get; set; }
    }
}