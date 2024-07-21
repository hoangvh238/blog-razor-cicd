using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Chủ đề không được để trống");
            RuleFor(x => x.MessageDetails).NotEmpty().WithMessage("Nội dung tin nhắn không được để trống");
            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Vui lòng nhập ít nhất 3 ký tự");
            RuleFor(x => x.Subject).MaximumLength(20).WithMessage("Vui lòng không nhập quá 20 ký tự");
            RuleFor(x => x.MessageDetails).MinimumLength(10).WithMessage("Vui lòng nhập ít nhất 10 ký tự");
            RuleFor(x => x.MessageDetails).MaximumLength(200).WithMessage("Vui lòng không nhập quá 200 ký tự");
        }
    }
}
