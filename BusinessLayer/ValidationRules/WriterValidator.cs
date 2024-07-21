using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
	public class WriterValidator : AbstractValidator<Writer>
	{
		public WriterValidator()
		{
			RuleFor(x => x.WriterNameSurname).NotEmpty().WithMessage("Họ tên không được để trống");
			RuleFor(x => x.WriterNameSurname).MinimumLength(2).WithMessage("Vui lòng nhập ít nhất 2 ký tự");
			RuleFor(x => x.WriterNameSurname).MaximumLength(50).WithMessage("Vui lòng không nhập quá 50 ký tự");
			RuleFor(x => x.WriterUserName).NotEmpty().WithMessage("Tên người dùng không được để trống");
			RuleFor(x => x.WriterMail).NotEmpty().WithMessage("Địa chỉ email không được để trống");
			RuleFor(x => x.WriterPassword).NotEmpty().WithMessage("Mật khẩu không được để trống");
			RuleFor(x => x.WriterPassword).MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự");
			RuleFor(x => x.WriterAbout).NotEmpty().WithMessage("Phần giới thiệu không được để trống");
			RuleFor(x => x.WriterAbout).MinimumLength(10).WithMessage("Phần giới thiệu phải có ít nhất 10 ký tự");
		}
	}
}
