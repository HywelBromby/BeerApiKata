using FluentValidation;
using PubApi.Bar.Models;

namespace PubApi.Bar.Services;

public class AddBarRequestValidator: AbstractValidator<AddABarRequest> 
{
    public AddBarRequestValidator()
    {
        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Address)
            .NotNull()
            .NotEmpty();
    }
}
