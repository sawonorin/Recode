using System;
using System.Collections.Generic;
using System.Text;
using Recode.Core.Interfaces.Services;
using Recode.Data.AppEntity;
using Recode.Data.MongoDB.Interfaces;
using Recode.Repository.MongoDBRepo;

namespace Recode.Service.Implementations.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IMongoDBRepository<AuditLog, string> _RequestlogCommand;
        public AuditLogService(IMongoDBRepository<AuditLog, string> RequestlogCommand)
        {
            _RequestlogCommand = RequestlogCommand;
        }

        public void SavLog(AuditLog model)
        {
            _RequestlogCommand.Insert(model);
        }
    }
}
