namespace Core.ViewModel
{
	public class GetEmailSetting
	{
		public string SecretKey { get; set; } = default!;
		public string From { get; set; } = default!;
		public string SmtpServer { get; set; } = default!;
		public int Port { get; set; }
		public bool EnablSSL {  get; set; }
	}
}
