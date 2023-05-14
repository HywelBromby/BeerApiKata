using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PubApi.Brewery.Interfaces;
using PubApi.Brewery.Models;
using System.Net;

namespace PubApi.Brewery.Services;

public class BreweryService : IBreweryService
{
    private readonly IGenericRepository<Guid, BreweryModel> _repository;
    private readonly IGenericValidator<AddABreweryRequest> _addABreweryValidator;
    private readonly IGenericValidator<UpdateABreweryRequest> _updateABreweryValidator;

    public BreweryService(
        IGenericRepository<Guid, BreweryModel> repository,
        IGenericValidator<AddABreweryRequest> addABreweryValidator,
        IGenericValidator<UpdateABreweryRequest> updateABreweryValidator)
    {
        _repository = repository;
        _addABreweryValidator = addABreweryValidator;
        _updateABreweryValidator = updateABreweryValidator;
    }

    public async Task<ObjectResult> AddABrewery(AddABreweryRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = await _addABreweryValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ObjectResult(validationResult.ValidationErrorsAsJson)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        var Brewerymodel = new BreweryModel
        {
            Name = request.Name,
            Id = Guid.NewGuid()
        };

        await _repository.Create(Brewerymodel.Id, Brewerymodel);

        return new ObjectResult(Brewerymodel.Id)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    public async Task<ObjectResult> GetAllBreweries()
    {
        return new ObjectResult(await _repository.GetAll())
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    public async Task<ObjectResult> GetBrewery(Guid id)
    {
        var result = await _repository.Get(id);

        if (result == default(BreweryModel))
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

    public async Task<ObjectResult> UpdateABrewery(UpdateABreweryRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = await _updateABreweryValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ObjectResult(validationResult.ValidationErrorsAsJson)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        var BreweryToUpdate = await _repository.Get(request.Id);

        if (BreweryToUpdate == default(BreweryModel))
        {
            return new ObjectResult(null)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        var Brewerymodel = new BreweryModel
        {
            Name = request.Name,
            Id = request.Id
        };

        await _repository.Update(request.Id, Brewerymodel);
        return new ObjectResult(null)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
