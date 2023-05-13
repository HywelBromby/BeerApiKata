using BeerApiKata.Infrastructure.Validation.Interfaces;
using BeerApiKata.Infrastructure.Validation.Models;
using FluentValidation;
using Newtonsoft.Json;

namespace BeerApiKata.Infrastructure.Validation.GenericFluentValidator;

public class GenericFluentValidator<T> : IGenericValidator<T>
{
    private readonly AbstractValidator<T> _validator;

    public GenericFluentValidator(AbstractValidator<T> validator)
    {
        _validator = validator;
    }

    public async Task<GenericValidationResult> Validate(T request)
    {
        var validatorResponse = await _validator.ValidateAsync(request);

        var response = new GenericValidationResult
        {
            IsValid = validatorResponse.IsValid,
            ValidationErrorsAsJson = validatorResponse.IsValid ? null : JsonConvert.SerializeObject(validatorResponse.Errors.Select(i=>i.ErrorMessage).ToList())
        };

        return response;
    }
}
