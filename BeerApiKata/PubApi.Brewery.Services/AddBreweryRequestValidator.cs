using FluentValidation;
using PubApi.Brewery.Models;

namespace PubApi.Brewery.Services;

public class AddBreweryRequestValidator : AbstractValidator<AddABreweryRequest>
{
    public AddBreweryRequestValidator()
    {
        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();
    }
}
