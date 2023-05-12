namespace BeerApiKata.Infrastructure.Validation.Models
{
    public class GenericValidationResult
    {
        public bool IsValid { get; set; }
        public string? ValidationErrorsAsJson { get; set; }
    }
}