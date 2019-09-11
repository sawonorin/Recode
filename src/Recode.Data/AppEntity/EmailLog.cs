using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Recode.Data.EntityBase;

namespace Recode.Data.AppEntity
{
    public class EmailLog : Entity<long>
    {
        public EmailLog()
        {
            Retires = 0;
            IsSent = false;
        }

        [Required]
        [StringLength(1000)]
        public string Sender { get; set; }
        [Required]
        [StringLength(1000)]
        public string Receiver { get; set; }

        [StringLength(1000)]
        public string CC { get; set; }

        public string BCC { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string MessageBody { get; set; }

        public int Retires { get; set; }
        public bool IsSent { get; set; }

        public DateTimeOffset? DateSent { get; set; }

        public DateTimeOffset DateToSend { get; set; }
    }
}
