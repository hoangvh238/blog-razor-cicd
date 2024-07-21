using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Tên danh mục không được để trống.");
            RuleFor(x => x.CategoryName).MinimumLength(3).WithMessage("Vui lòng nhập ít nhất 3 ký tự.");
            RuleFor(x => x.CategoryName).MaximumLength(20).WithMessage("Vui lòng không nhập quá 20 ký tự.");
            RuleFor(x => x.CategoryDescription).NotEmpty().WithMessage("Mô tả không được để trống.");
            RuleFor(x => x.CategoryDescription).MinimumLength(3).WithMessage("Vui lòng nhập ít nhất 3 ký tự.");
        }
    }
}
