using FluentValidation;
using PubApi.Bar.Models;

namespace PubApi.Bar.Services;

public class UpdateBarRequestValidator: AbstractValidator<UpdateABarRequest> 
{
    public UpdateBarRequestValidator()
    {
        RuleFor(i => i.Id).NotNull();
        RuleFor(i=>i.PercentageAlcoholByVolume).GreaterThanOrEqualTo(0);
        RuleFor(i => i.PercentageAlcoholByVolume).LessThanOrEqualTo(1);

        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();
    }
}
