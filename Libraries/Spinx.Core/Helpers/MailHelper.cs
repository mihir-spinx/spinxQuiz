using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;

/*
How to use code sample: 
=======================
    new MailHelper()
        .From("test@gmail.com")
        .To("ajmerainfo@gmail.com")
        .Subject("Hello Subject {%WebsiteUrl%}")
        .Body("Test Body {%Name%}")
        .Variables(new Dictionary<string, object>()
        {
            {"Name", "Keyur Ajmera"}
        }).Send();
*/

namespace Spinx.Core.Helpers
{
    public class MailHelper
    {
        private readonly IList<MailAddress> _toAddresses;
        private string _subject;
        private string _body;
        private IDictionary<string, object> _variables;
        private string _from;

        public MailHelper()
        {
            _toAddresses = new List<MailAddress>();
            _variables = new Dictionary<string, object>();
        }

        public MailHelper To(string email)
        {
            _toAddresses.Add(new MailAddress(email));
            return this;
        }

        public MailHelper To(string name, string email)
        {
            _toAddresses.Add(new MailAddress(email, name));
            return this;
        }

        public MailHelper Subject(string subject)
        {
            _subject = subject;
            return this;
        }

        public MailHelper Body(string body)
        {
            _body = body;
            return this;
        }

        public MailHelper Variables(IDictionary<string, object> bodyValues)
        {
            _variables = bodyValues;
            return this;
        }

        public MailHelper AddVariables(string key, object value)
        {
            _variables.Add(key, value);
            return this;
        }

        public MailHelper From(string from)
        {
            _from = from;
            return this;
        }

        public void Send()
        {
            try
            {
                var message = new MailMessage();
                foreach (var toAddress in _toAddresses)
                    message.To.Add(toAddress);

                message.Subject = PrepareSubjectWithVariables();
                message.From = new MailAddress(_from ?? SmtpSection.From);
                message.Body = PrepareBodyWithVariables();
                message.IsBodyHtml = true;

                GetSmtpClient()
                    .Send(message);
            }
            catch (Exception ex)
            {
                Console.Write("Ex: " + ex);
                // TODO: Enter logic for log error
            }
        }

        private static SmtpSection SmtpSection => (SmtpSection) ConfigurationManager.GetSection("system.net/mailSettings/smtp");

        private static SmtpClient GetSmtpClient()
        {
            return new SmtpClient(SmtpSection.Network.Host, SmtpSection.Network.Port)
            {
                Credentials =
                    new System.Net.NetworkCredential(SmtpSection.Network.UserName,
                        SmtpSection.Network.Password),
                EnableSsl = SmtpSection.Network.EnableSsl
            };
        }

        public string PrepareSubjectWithVariables()
        {
            if (!_variables.Any())
                return _subject;

            return _variables.Aggregate(_subject,
                (current, subjectValue) => current.Replace("{%" + subjectValue.Key + "%}", Convert.ToString(subjectValue.Value)));
        }

        public string PrepareBodyWithVariables()
        {
            if (!_variables.Any())
                return _body;

            return _variables.Aggregate(_body,
                (current, bodyValue) => current.Replace("{%" + bodyValue.Key + "%}", Convert.ToString(bodyValue.Value)));
        }
    }
}