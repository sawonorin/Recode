using System;
using System.Collections.Generic;
using System.Text;

namespace Recode.Core.Models
{
    public class BusinessAccountModel
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public int BankId { get; set; }

        public string MandateFilePath { get; set; }

        public bool IsVerified { get; set; }

        public DateTimeOffset? DateVerified { get; set; }

        public string VerifiedByUserId { get; set; }

        public string Comment { get; set; }

        public long BalanceInKobo { get; set; }
        public decimal Balance { get { return BalanceInKobo / 100; } }

        public BankAccountStatus Status { get; set; } = BankAccountStatus.New;
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.Now;

        public string BankName { get; set; }
        public string BusinessName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }

    public enum BankAccountStatus
    {
        New = 1,
        AwaitingMandate,
        Processing,
        Declined,
        Approved
    }
}
