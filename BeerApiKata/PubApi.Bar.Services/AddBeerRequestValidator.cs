using FluentValidation;
using PubApi.Beer.Models;

namespace PubApi.Beer.Services;

public class AddBeerRequestValidator: AbstractValidator<AddABeerRequest> 
{
    public AddBeerRequestValidator()
    {
        RuleFor(i=>i.PercentageAlcoholByVolume).GreaterThanOrEqualTo(0);
        RuleFor(i => i.PercentageAlcoholByVolume).LessThanOrEqualTo(1);

        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();
    }
}
