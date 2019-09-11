using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Recode.Core.ConfigModels;
using Recode.Core.Interfaces.Repositories;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Data;
using Recode.Data.AppEntity;
using Recode.Service.Utilities;

namespace Recode.Service.Implementations.Services
{
    public class EmailSendingJob : IEmailSendingJob
    {
        private IEmailService _emailService;
        private readonly IEmailRepository _emailRepository;
        private MailSetting _mailsettings;
        public EmailSendingJob(IOptions<MailSetting> mailsettings,
            IEmailRepository emailRepository, IEmailService emailService)
        {
            _emailService = emailService;
            _emailRepository = emailRepository;
            _mailsettings = mailsettings.Value;

        }
        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await SendPendingEmail();
        }

        public async Task SendPendingEmail()
        {
            EmailLogModel[] emailLogs = await _emailRepository.GetUnsentMail(20);
            if (emailLogs.Length > 0)
            {
                foreach (EmailLogModel logmodel in emailLogs)
                {
                    bool result = _emailService.SendMail(logmodel.Subject, logmodel.Receiver, logmodel.MessageBody);
                    if (result)
                    {
                        logmodel.DateSent = DateTime.Now;
                        logmodel.IsSent = true;
                        logmodel.Retires++;
                    }
                    else
                    {
                        logmodel.IsSent = false;
                        logmodel.Retires++;
                    }
                    await _emailRepository.UpdateMailAfterSent(logmodel);
                    // var result = await CoreServices.MessageServices.SendPayferEmailAsync("Email Verification Notification", email, fileContents);
                }
            }
        }

        public bool SendMail(string subject, string email, string content, string mailFrom)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            try
            {
                message.IsBodyHtml = true;
                MailAddress fromAddress = new MailAddress(mailFrom);
                message.From = fromAddress;
                message.To.Add(new MailAddress(email));

                message.Subject = subject;
                message.Body = subject;
                message.IsBodyHtml = true;

                smtpClient.Host = _mailsettings.SMTPServer;
                int portno = 25;
                int.TryParse(_mailsettings.SMTPPORT, out portno);
                smtpClient.Port = portno;
                smtpClient.Credentials = new System.Net.NetworkCredential(_mailsettings.SMTPUserName, _mailsettings.SMTPPassword); //not neccessary
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
            finally
            {
                smtpClient.Dispose();
                message.Dispose();
            }
        }
    }
}
