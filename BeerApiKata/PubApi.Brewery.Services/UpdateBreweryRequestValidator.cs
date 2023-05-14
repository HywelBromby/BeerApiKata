using FluentValidation;
using PubApi.Brewery.Models;

namespace PubApi.Brewery.Services;

public class UpdateBreweryRequestValidator : AbstractValidator<UpdateABreweryRequest>
{
    public UpdateBreweryRequestValidator()
    {
        RuleFor(i => i.Id).NotNull();

        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();
    }
}
