using System.Text.RegularExpressions;
using FluentValidation;
using PrescriberSystem.Models;

namespace PrescriberSystem.Validators;

public class PatientValidator : AbstractValidator<Patient>
{
    public PatientValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("身分證字號不能為空。")
            .Length(10)
            .WithMessage("身分證字號必須為10個字符。")
            .Matches(@"^[A-Z][0-9]{9}$")
            .WithMessage("身分證字號格式不正確。應為一個大寫英文字母後跟9個數字。");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("姓名不能為空。")
            .Length(1, 30)
            .WithMessage("姓名長度必須在1到30個字符之間。")
            .Matches(@"^[A-Za-z]+$")
            .WithMessage("姓名只能包含英文字母。");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("性別不能為空。")
            .Must(gender => gender == 'M' || gender == 'F')
            .WithMessage("性別必須是 'M'（男）或 'F'（女）。");

        RuleFor(x => x.Age).InclusiveBetween(1, 180).WithMessage("年齡必須在1到180歲之間。");

        RuleFor(x => x.Height).InclusiveBetween(1f, 500f).WithMessage("身高必須在1到500公分之間。");

        RuleFor(x => x.Weight).InclusiveBetween(1f, 500f).WithMessage("體重必須在1到500公斤之間。");
    }
}
