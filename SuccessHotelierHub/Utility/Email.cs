using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Net.Configuration;
using System.Linq;
using System.IO;
using SuccessHotelierHub.Models;
using System.Configuration;
using System.Web;


namespace SuccessHotelierHub.Utility
{
    public class Email
    {
        public static void sendMail(string mTo, string mSubject, string mBody, string attachmentName = "", bool hasAttachment = false, byte[] pdfBytes = null)
        {
            try
            {

                string mailBody = "";
                mailBody = mailBody + mBody;

                string mailServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string mailUser = ConfigurationManager.AppSettings["mailAccount"].ToString();
                string mailPassword = ConfigurationManager.AppSettings["mailPassword"].ToString();
                string mailTo = ConfigurationManager.AppSettings["mailAccount"].ToString();
                string mFrom = ConfigurationManager.AppSettings["mailAccount"].ToString();

                if (string.IsNullOrEmpty(mTo))
                    mTo = mailTo;

                System.Net.Mail.MailMessage mailObject = new System.Net.Mail.MailMessage();

                if (hasAttachment)
                {
                    // If there needs to be send as attachment
                    Stream stream = new MemoryStream(pdfBytes);
                    var attachment = new Attachment(stream, attachmentName, "application/octet-stream"/*MediaTypeNames.Application.Pdf*/);

                    // add the attachments
                    if (attachment != null && attachment.ContentStream.Length > 0)
                    {
                        // add each attachemnt in order
                        mailObject.Attachments.Add(attachment);
                    }
                }

                mailObject.To.Add(new MailAddress(mTo));

                mailObject.From = new MailAddress(mFrom, "HotelierHub");

                mailObject.Subject = mSubject;
                mailObject.IsBodyHtml = true;
                mailObject.Body = mailBody;
                mailObject.BodyEncoding = System.Text.Encoding.UTF8;
                mailObject.SubjectEncoding = System.Text.Encoding.UTF8;

                SmtpClient smtp = new SmtpClient();

                smtp.Host = mailServer;
                smtp.Port = 25;
                smtp.EnableSsl = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(mailUser, mailPassword);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = basicAuthenticationInfo;

                smtp.Send(mailObject);
            }
            catch (Exception ex)
            {
                Utility.LogError(ex, "sendMail");
            }
        }


        public static void sendMailTemplate(string mTo, string mSubject, string mBody)
        {
            try
            {
                string mailServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string mailUser = ConfigurationManager.AppSettings["mailAccount"].ToString();
                string mailPassword = ConfigurationManager.AppSettings["mailPassword"].ToString();
                string mailTo = ConfigurationManager.AppSettings["mailAccount"].ToString();
                string mFrom = ConfigurationManager.AppSettings["mailAccount"].ToString();

                if (string.IsNullOrEmpty(mTo))
                    mTo = mailTo;

                System.Net.Mail.MailMessage mailObject = new System.Net.Mail.MailMessage();

                mailObject.To.Add(mTo);
                mailObject.From = new MailAddress(mFrom);

                mailObject.Subject = mSubject;
                mailObject.IsBodyHtml = true;
                mailObject.Body = mBody;

                mailObject.BodyEncoding = System.Text.Encoding.UTF8;
                mailObject.SubjectEncoding = System.Text.Encoding.UTF8;

                SmtpClient smtp = new SmtpClient();

                smtp.Host = mailServer;
                smtp.Port = 8889;
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(mailUser, mailPassword);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = basicAuthenticationInfo;

                smtp.Send(mailObject);
            }
            catch (Exception ex)
            {   
                Utility.LogError(ex, "sendMailTemplate");
            }
        }


        public static bool SendRegistrationMail(string SentTo, String Body)
        {
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(ConfigurationManager.AppSettings["Admin"]);
            msg.To.Add(SentTo);
            msg.Subject = "Welcome to HotelierHub!";
            msg.Priority = MailPriority.High;
            msg.Body = Body;
            msg.IsBodyHtml = true;


            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"], Convert.ToInt32(587));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Admin"], ConfigurationManager.AppSettings["mailPassword"]);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = false;
            smtpClient.Port = 8889;

            try
            {
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                Utility.LogError(ex, "SendRegistrationMail");
                return false;
            }
            return true;
        }
    }
}