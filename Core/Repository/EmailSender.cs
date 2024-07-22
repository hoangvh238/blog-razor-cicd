using Core.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Core.Repository
{
	public class EmailSender : IEmailSender
	{private readonly IConfiguration _configuration;
		public EmailSender(IConfiguration configuration) {
			_configuration = configuration;
				}
		public async Task<bool> EmailSendAsync(string email, string subject, string message)
		{
			bool status = false;
			try
			{
				GetEmailSetting getEmailSetting = new GetEmailSetting()
				{
					SecretKey = _configuration.GetValue<string>("Appsettings:SecretKey"),
					From = _configuration.GetValue<string>("Appsettings:EmailSettings:From"),
					SmtpServer = _configuration.GetValue<string>("Appsettings:EmailSettings:SmtpServer"),
					Port = _configuration.GetValue<int>("Appsettings:EmailSettings:Port"),
					EnablSSL = _configuration.GetValue<bool>("Appsettings:EmailSettings:EnablSSL"),
				};
				MailMessage mail = new MailMessage()
				{
					From = new MailAddress(getEmailSetting.From),
					Subject = subject,
					Body = message,
					IsBodyHtml = true 

				};
				mail.To.Add(email);
				SmtpClient smtpClient = new SmtpClient(getEmailSetting.SmtpServer)
				{
					Port = getEmailSetting.Port,
					Credentials = new NetworkCredential(getEmailSetting.From, getEmailSetting.SecretKey),
					EnableSsl = getEmailSetting.EnablSSL

				};
				await smtpClient.SendMailAsync(mail);
				status = true;
			}
			catch (Exception)
			{
				status=false;
			}
			return status;
		}
	}
}
