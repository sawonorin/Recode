using AutoMapper;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Interfaces.Services;
using Recode.Data.AppEntity;
using Recode.Data.MongoDB.Interfaces;
using Recode.Repository.MongoDBRepo;

namespace Recode.Service.Implementations.Services
{
    public class ClientRequestLogService : IClientRequestLogService
    {
        private readonly IMongoDBRepository<ClientRequestLog, string> _RequestlogCommand;
        public ClientRequestLogService(IMongoDBRepository<ClientRequestLog, string> RequestlogCommand)
        {
            _RequestlogCommand = RequestlogCommand;
        }

        public void SaveClientReQuest(ClientRequestLog model)
        {
            _RequestlogCommand.Insert(model);
        }
    }
}
