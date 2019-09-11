using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Data.AppEntity;

namespace Recode.Data.MongoDB.Interfaces
{
    public interface IClientRequestLogService
    {
        void  SaveClientReQuest(ClientRequestLog model);
    }
}
