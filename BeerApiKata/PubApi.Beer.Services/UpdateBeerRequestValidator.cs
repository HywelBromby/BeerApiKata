using FluentValidation;
using PubApi.Beer.Models;

namespace PubApi.Beer.Services;

public class UpdateBeerRequestValidator: AbstractValidator<UpdateABeerRequest> 
{
    public UpdateBeerRequestValidator()
    {
        RuleFor(i => i.Id).NotNull();
        RuleFor(i=>i.PercentageAlcoholByVolume).GreaterThanOrEqualTo(0);
        RuleFor(i => i.PercentageAlcoholByVolume).LessThanOrEqualTo(1);

        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();
    }
}
