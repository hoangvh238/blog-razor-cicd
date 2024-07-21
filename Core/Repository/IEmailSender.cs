using DocumentFormat.OpenXml.Drawing;
using System.Threading.Tasks;

namespace Core.Repository
{
	public interface IEmailSender
	{
		Task<bool> EmailSendAsync(string email,string subject,string message);
	}
}
