using FluentValidation;
using PrescriberSystem.Models;

namespace PrescriberSystem.Validators;

public sealed class PrescriptionValidator : AbstractValidator<Prescription>
{
    public PrescriptionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("名字不能為空")
            .Length(4, 30).WithMessage("名字長度必須在4到30個字符之間");

        RuleFor(x => x.PotentialDisease)
            .NotEmpty().WithMessage("潛在疾病不能為空")
            .Length(3, 100).WithMessage("潛在疾病長度必須在3到100個字符之間");

        RuleFor(x => x.Medicines)
            .NotEmpty().WithMessage("藥物列表不能為空");

        RuleForEach(x => x.Medicines)
            .NotEmpty().WithMessage("藥物名稱不能為空")
            .Length(3, 30).WithMessage("藥物名稱長度必須在3到30個字符之間");

        RuleFor(x => x.Usage)
            .MaximumLength(1000).WithMessage("使用方法長度不能超過1000個字符");
    }
}
