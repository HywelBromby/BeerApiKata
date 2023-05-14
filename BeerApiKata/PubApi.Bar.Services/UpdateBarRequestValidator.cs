using FluentValidation;
using PubApi.Bar.Models;

namespace PubApi.Bar.Services;

public class UpdateBarRequestValidator: AbstractValidator<UpdateABarRequest> 
{
    public UpdateBarRequestValidator()
    {
        RuleFor(i => i.Name)
            .NotNull()
            .NotEmpty();

        RuleFor(i => i.Address)
            .NotNull()
            .NotEmpty();
    }
}
