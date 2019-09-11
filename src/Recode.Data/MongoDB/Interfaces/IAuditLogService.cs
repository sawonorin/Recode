using System;
using System.Collections.Generic;
using System.Text;
using Recode.Data.AppEntity;

namespace Recode.Data.MongoDB.Interfaces
{
    public interface IAuditLogService
    {
        void SavLog(AuditLog model);
    }
}
