﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SMTP.Service.Model;

namespace SMTP.Service.SMTPLibrary
{
    public class SmtpLibrary : ISmtpLibrary
    {
        public void Send(MailModel mailModel, string smtpServerName, string smtpUserName, string smtpPassword, int serverPort)
        {
            using (SmtpClient smtpClient = new SmtpClient(smtpServerName))
            {
                if (serverPort > 0)
                    smtpClient.Port = serverPort;
                if(!string.IsNullOrWhiteSpace(smtpUserName) && (!string.IsNullOrWhiteSpace(smtpPassword)))
                    smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                try
                {
                    smtpClient.Send(getMailMessageFromMailModel(mailModel));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }

        public Task SendAsync(MailModel mailModel, string smtpServerName, string smtpUserName, string smtpPassword, int serverPort)
        {
            using (SmtpClient smtpClient = new SmtpClient(smtpServerName))
            {
                if (serverPort > 0)
                    smtpClient.Port = serverPort;
                smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                try
                {
                    return smtpClient.SendMailAsync(getMailMessageFromMailModel(mailModel));
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }
        }
        private MailMessage getMailMessageFromMailModel(MailModel mailModel)
        {
            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(mailModel.FromAddress),
                Subject = mailModel.Subject,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                Priority = MailPriority.Normal,
                Body = mailModel.Body
            };
            foreach (string mailAddress in mailModel.ToAddress)
                mailMessage.To.Add(mailAddress);
            
            foreach(string mailAddress in mailModel.Cc)
                mailMessage.CC.Add(mailAddress);
            foreach (string mailAddress in mailModel.Bcc)
                mailMessage.Bcc.Add(mailAddress);
            return mailMessage;
        }
        
    }
    public interface ISmtpLibrary
    {
        void Send(MailModel mailModel, string smtpServerName, string smtpUserName, string smtpPassword, int serverPort);
        Task SendAsync(MailModel mailModel, string smtpServerName, string smtpUserName, string smtpPassword, int serverPort);
    }
}
