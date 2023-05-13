using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Interfaces;
using PubApi.Beer.Models;
using System.Net;

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

    public async Task<ObjectResult> AddABeer(AddABeerRequest request)
    {
        if(request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = await _addABeerValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ObjectResult(validationResult.ValidationErrorsAsJson)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };            
        }

        var beermodel = new BeerModel
        {
            Name = request.Name,
            Id = Guid.NewGuid(),
            PercentageAlcoholByVolume = request.PercentageAlcoholByVolume
        };             
               
        await _repository.Create(beermodel.Id, beermodel);

        return new ObjectResult(beermodel.Id)
        {
            StatusCode = (int)HttpStatusCode.OK
        };        
    }

    public async Task<ObjectResult> GetAllBeers()
    {
        return new ObjectResult(await _repository.GetAll())
        {
            StatusCode = (int) HttpStatusCode.OK
        };
    }

    public async Task<ObjectResult> GetBeer(Guid id)
    {
        var result = await _repository.Get(id);

        if (result == default(BeerModel))
        {
            return new ObjectResult(null)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        return new ObjectResult(result)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    public Task<ObjectResult> UpdateABeer(UpdateABeerRequest request)
    {
        throw new NotImplementedException();
    }
}
