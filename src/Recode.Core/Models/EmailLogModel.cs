using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class EmailLogModel
    {
        public long Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public int Retires { get; set; }
        public bool IsSent { get; set; }
        public DateTimeOffset? DateSent { get; set; }
        public DateTimeOffset DateToSend { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
