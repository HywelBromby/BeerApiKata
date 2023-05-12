using BeerApiKata.Infrastructure.Validation.Models;

namespace BeerApiKata.Infrastructure.Validation.Interfaces;

public interface IGenericValidator<T>
{
    Task<GenericValidationResult> Validate(T request);
}
