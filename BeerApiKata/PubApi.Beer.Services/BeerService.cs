using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using PubApi.Beer.Interfaces;
using PubApi.Beer.Models;

namespace PubApi.Beer.Services;

public class BeerService : IBeerService
{
    private readonly IGenericRepository<Guid, BeerModel> _repository;
    private readonly IGenericValidator<AddABeerRequest> _addABeerValidator;

    public BeerService(IGenericRepository<Guid, BeerModel> repository, IGenericValidator<AddABeerRequest> addABeerValidator)
    {
        _repository = repository;
        _addABeerValidator = addABeerValidator;
    }

    public async Task<Guid> AddABeer(AddABeerRequest request)
    {
        if(request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = await _addABeerValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            //var result = new ObjectResult(validationResult.ValidationErrorsAsJson)
            //{
                
            //}
        }




         var beermodel = new BeerModel
        {
            Name = request.Name,
            Id = Guid.NewGuid(),
            PercentageAlcoholByVolume = request.PercentageAlcoholByVolume
        };             
               
        await _repository.Create(beermodel.Id, beermodel);
        
        return beermodel.Id;
    }

    public async Task<IEnumerable<BeerModel>> GetAllBeers()
    {
        throw new NotImplementedException();

        return await _repository.GetAll();
    }
}
