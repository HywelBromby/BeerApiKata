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
    private readonly IGenericValidator<UpdateABeerRequest> _updateABeerValidator;

    public BeerService(
        IGenericRepository<Guid, BeerModel> repository, 
        IGenericValidator<AddABeerRequest> addABeerValidator,
        IGenericValidator<UpdateABeerRequest> updateABeerValidator)
    {
        _repository = repository;
        _addABeerValidator = addABeerValidator;
       _updateABeerValidator = updateABeerValidator;
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

    public async Task<ObjectResult> GetBeers(GetBeersRequest filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));
        }

        //not the most efficient, but assuming low over all numbers of Beers
        var allBeers = await _repository.GetAll();
        var filteredBeers = allBeers.Where(i=>i.PercentageAlcoholByVolume > filter.gtAlcoholByVolume && i.PercentageAlcoholByVolume < filter.ltAlcoholByVolume).ToList();

        return new ObjectResult(filteredBeers)
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

    public async Task<ObjectResult> UpdateABeer(UpdateABeerRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = await _updateABeerValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ObjectResult(validationResult.ValidationErrorsAsJson)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        var beerToUpdate = await _repository.Get(request.Id);

        if (beerToUpdate == default(BeerModel))
        {
            return new ObjectResult(null)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        var beermodel = new BeerModel
        {
            Name = request.Name,
            Id = request.Id,
            PercentageAlcoholByVolume = request.PercentageAlcoholByVolume
        };

        await _repository.Update(request.Id, beermodel);
        return new ObjectResult(null)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }   
}
