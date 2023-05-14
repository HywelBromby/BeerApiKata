using FluentValidation;
using PubApi.Bar.Models;

namespace PubApi.Bar.Services;

public class AddBarRequestValidator: AbstractValidator<AddABarRequest> 
{
    public AddBarRequestValidator()
    {
        RuleFor(i=>i.PercentageAlcoholByVolume).GreaterThanOrEqualTo(0);
        RuleFor(i => i.PercentageAlcoholByVolume).LessThanOrEqualTo(1);

        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();
    }
}
