using FluentValidation;

namespace YolkStudio.Pokemon.Api.Trainers;

public class CreateTrainerValidator :  AbstractValidator<CreateTrainerRequest>
{
    public CreateTrainerValidator()
    {
        RuleFor(t => t.Name).NotEmpty().WithMessage("Name is required")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters long");

        RuleFor(t => t.Region).NotEmpty().WithMessage("Region is required")
            .Length(1, 100).WithMessage("Region must be between 1 and 100 characters long");

        RuleFor(t => t.BirthDate).NotEmpty().WithMessage("BirthDate is required")
            .LessThan(DateTimeOffset.Now.AddYears(-18)).WithMessage("A trainer must be at least 18 years old");
    }
}