using System.ComponentModel.DataAnnotations;

namespace Core.ViewModel
{
	public class ForgotPass
	{
		[Required]
		public string Email { get; set; } = default!;
		public string UserId {  get; set; }= default!;
		public string Token {  get; set; } = default!; 
		
		[Required]
		public string Password {  get; set; } = default!;
		[Required]
		public string ConfirmPassword { get; set; } = default!;
	}
}
