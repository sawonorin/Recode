using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Repositories
{
    public interface IBankRepository
    {
        Task<BankModel[]> Get();
    }
}
