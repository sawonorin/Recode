using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using Recode.Core.ConfigModels;
using Recode.Core.Interfaces.Repositories;
using Recode.Core.Interfaces.Services;
using Recode.Core.Models;
using Recode.Data;
using Recode.Data.AppEntity;
using Recode.Service.Utilities;

namespace Recode.Service.Implementations.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private APPURL _settings;
        private MailSetting _mailsettings;
        public EmailService(IHttpContextAccessor httpContextAccessor,
             IHostingEnvironment hostingEnvironment
            , IOptions<APPURL> settings
            , IOptions<MailSetting> mailsettings,
             IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _settings = settings.Value;
            _mailsettings = mailsettings.Value;
        }

        public bool EmailConfirmation(string token, string email, string fullName, string userId)
        {
            try
            {
                string projectRootPath = _hostingEnvironment.ContentRootPath;

                var callbackUrl = $"{_settings.EmailURL}/#/Confirmation/{userId}?token={token}";
                string confirmationEmailPath = Path.Combine(projectRootPath, "EmailTemplate/ConfirmationEmail.html");

                string fileContents = File.ReadAllText(confirmationEmailPath);
                fileContents = fileContents.Replace("##NAME##", $"{fullName}");
                fileContents = fileContents.Replace("##ACTIVATIONLINK##", callbackUrl);
                EmailLogModel logmodel = new EmailLogModel();
                logmodel.Receiver = email;
                logmodel.Sender = _mailsettings.MailFrom;
                logmodel.Subject = "Email Verification Notification";
                logmodel.MessageBody = fileContents;
                logmodel.DateCreated = logmodel.DateToSend = DateTime.Now;
                logmodel.IsSent = false;
                bool result = SendMail(logmodel.Subject, logmodel.Receiver, logmodel.MessageBody);
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
                _emailRepository.CreateMail(logmodel);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public bool ForgetPasswordEmail(string token, string email, string fullName, string userId)
        {
            try
            {
                string projectRootPath = _hostingEnvironment.ContentRootPath;
                //string folderPath = Path.Combine(projectRootPath, "~/EmailTemplate/PasswordResetNotification.html");

                var callbackUrl = "";
                callbackUrl = $"{_settings.EmailURL}/#/resetPassword/{userId}?token={token}";
                string confirmationEmailPath = Path.Combine(projectRootPath, "EmailTemplate/PasswordResetNotification.html");

                string fileContents = File.ReadAllText(confirmationEmailPath);
                fileContents = fileContents.Replace("##NAME##", $"{fullName}");
                fileContents = fileContents.Replace("##ACTIVATIONLINK##", callbackUrl);
                EmailLogModel logmodel = new EmailLogModel();
                logmodel.Receiver = email;
                logmodel.Sender = _mailsettings.MailFrom;
                logmodel.Subject = "Password Reset Notification";
                logmodel.MessageBody = fileContents;
                logmodel.DateCreated = logmodel.DateToSend = DateTime.Now;
                logmodel.IsSent = false;
                bool result = SendMail(logmodel.Subject, logmodel.Receiver, fileContents);
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
                _emailRepository.CreateMail(logmodel);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public bool SendMail(string subject, string email, string content)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage message = new MailMessage();
            try
            {
                message.IsBodyHtml = true;
                MailAddress fromAddress = new MailAddress(_mailsettings.MailFrom, _mailsettings.MailFromName);
                message.From = fromAddress;
                message.To.Add(new MailAddress(email, email));

                message.Subject = subject;
                message.Body = content;
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
