using FluentValidation;
using PrescriberSystem.Models;

namespace PrescriberSystem.Validators;

public class DiagnosticRequestValidator : AbstractValidator<DiagnosticRequest>
{
    /// <summary>
    ///  開頭為大寫英文字母，之後有 9 位數字
    /// </summary>
    public DiagnosticRequestValidator()
    {
        // RuleFor(x => x.PatientId).NotEmpty().Length(10).Matches("^[A-Z][0-9]{9}$");
        RuleFor(x => x.Symptoms).NotNull().NotEmpty().Must(x => x.Count > 0);
    }
}
