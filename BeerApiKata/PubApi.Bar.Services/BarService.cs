using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PubApi.Bar.Interfaces;
using PubApi.Bar.Models;
using System.Net;

namespace PubApi.Bar.Services;

public class BarService : IBarService
{
    private readonly IGenericRepository<Guid, BarModel> _repository;
    private readonly IGenericValidator<AddABarRequest> _addABarValidator;
    private readonly IGenericValidator<UpdateABarRequest> _updateABarValidator;

    public BarService(
        IGenericRepository<Guid, BarModel> repository, 
        IGenericValidator<AddABarRequest> addABarValidator,
        IGenericValidator<UpdateABarRequest> updateABarValidator)
    {
        _repository = repository;
        _addABarValidator = addABarValidator;
       _updateABarValidator = updateABarValidator;
    }

    public async Task<ObjectResult> AddABar(AddABarRequest request)
    {
        if(request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = await _addABarValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ObjectResult(validationResult.ValidationErrorsAsJson)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };            
        }

        var Barmodel = new BarModel
        {
            Name = request.Name,
            Id = Guid.NewGuid(),
            Address = request.Address
        };             
               
        await _repository.Create(Barmodel.Id, Barmodel);

        return new ObjectResult(Barmodel.Id)
        {
            StatusCode = (int)HttpStatusCode.OK
        };        
    }

    public async Task<ObjectResult> GetAllBars()
    {
        return new ObjectResult(await _repository.GetAll())
        {
            StatusCode = (int) HttpStatusCode.OK
        };
    }

    public async Task<ObjectResult> GetBar(Guid id)
    {
        var result = await _repository.Get(id);

        if (result == default(BarModel))
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

    public async Task<ObjectResult> UpdateABar(UpdateABarRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var validationResult = await _updateABarValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return new ObjectResult(validationResult.ValidationErrorsAsJson)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        var BarToUpdate = await _repository.Get(request.Id);

        if (BarToUpdate == default(BarModel))
        {
            return new ObjectResult(null)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };
        }

        var Barmodel = new BarModel
        {
            Name = request.Name,
            Id = request.Id,
            Address = request.Address
        };

        await _repository.Update(request.Id, Barmodel);
        return new ObjectResult(null)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
