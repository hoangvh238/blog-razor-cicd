using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
	public class BlogValidator : AbstractValidator<Blog>
	{
		public BlogValidator()
		{
			RuleFor(x => x.BlogTitle).NotEmpty().WithMessage("Tiêu đề không được để trống.");
			RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Nội dung không được để trống.");
			RuleFor(x => x.BlogImage).NotEmpty().WithMessage("Hình ảnh không được để trống.");
			RuleFor(x => x.BlogTitle).MaximumLength(100).WithMessage("Vui lòng không nhập quá 100 ký tự.");
			RuleFor(x => x.BlogTitle).MinimumLength(5).WithMessage("Vui lòng nhập ít nhất 5 ký tự.");
		}
	}
}
