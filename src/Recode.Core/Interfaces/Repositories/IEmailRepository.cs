﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.Models;

namespace Recode.Core.Interfaces.Repositories
{
    public interface IEmailRepository
    {
        Task<EmailLogModel> CreateMail(EmailLogModel model);
        Task<EmailLogModel[]> GetUnsentMail(int count);
        Task<bool> UpdateMailAfterSent(EmailLogModel model);
    }
}
