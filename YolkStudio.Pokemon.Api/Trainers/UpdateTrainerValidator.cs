using FluentValidation;

namespace YolkStudio.Pokemon.Api.Trainers;

public class UpdateTrainerValidator : AbstractValidator<UpdateTrainerRequest>
{
    public UpdateTrainerValidator()
    {
        When(t => t.Name is not null, () =>
        {
            RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 100).WithMessage("Name must be between 1 and 100 characters long");
        });

        When(t => t.Region is not null, () =>
        {
            RuleFor(t => t.Region)
                .NotEmpty().WithMessage("Region is required")
                .Length(1, 100).WithMessage("Region must be between 1 and 100 characters long");
        });

        When(t => t.Wins is not null, () =>
        {
            RuleFor(t => t.Wins)
                .GreaterThanOrEqualTo(0).WithMessage("Wins must be greater than or equal to 0");
        });

        When(t => t.Losses is not null, () =>
        {
            RuleFor(t => t.Losses)
                .GreaterThanOrEqualTo(0).WithMessage("Losses must be greater than or equal to 0");
        });
    }
}