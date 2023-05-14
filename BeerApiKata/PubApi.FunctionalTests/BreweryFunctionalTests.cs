using BeerApiKata.Infrastructure.Repository.InMemRepository;
using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using BeerApiKata.Infrastructure.Validation.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PubApi.Brewery.Models;
using PubApi.Brewery.Services;
using PubApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PubApi.FunctionalTests;

public class BreweryFunctionalTests
{
    #region Setup
    
    BreweryController _sut; 
    private static IGenericRepository<Guid, BreweryModel> _BreweryRepository;
    private Mock<IGenericValidator<AddABreweryRequest>> _mockAddABreweryValidator;
    private Mock<IGenericValidator<UpdateABreweryRequest>> _mockUpdateABreweryValidator;


    [SetUp]
    public void Setup()
    {
        _BreweryRepository = new InMemAsyncRepository<Guid, BreweryModel>();

        _mockAddABreweryValidator = new Mock<IGenericValidator<AddABreweryRequest>>();
        _mockAddABreweryValidator.Setup(i => i.Validate(It.IsAny<AddABreweryRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = true });

        _mockUpdateABreweryValidator = new Mock<IGenericValidator<UpdateABreweryRequest>>();
        _mockUpdateABreweryValidator.Setup(i => i.Validate(It.IsAny<UpdateABreweryRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = true });

        _sut = new BreweryController(new BreweryService(_BreweryRepository, _mockAddABreweryValidator.Object, _mockUpdateABreweryValidator.Object));
    }

    #endregion Setup

    #region GetById Tests

    [Test]
    public async Task Given_GetByIdIsCalled_AndAMatchingItemIsFound_Then_ItShouldBeReturned()
    {
        var BreweryModel1 = new BreweryModel{  Name = "Test1", Id = Guid.NewGuid() };
       
        ((InMemAsyncRepository<Guid, BreweryModel>)_BreweryRepository)._theStore.Add(BreweryModel1.Id, BreweryModel1);       

        var result = await _sut.Get(BreweryModel1.Id) as ObjectResult;

        Assert.AreEqual(BreweryModel1, (BreweryModel)result.Value);
        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

    }

    [Test]
    public async Task Given_GetByIdIsCalled_AndNoMatchingBreweryIsFound_Then_404ShouldBeReturned()
    {
        var result = await _sut.Get(Guid.NewGuid()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

    }

    #endregion Get Test

    #region Get Tests

    [Test]
    public async Task Given_GetAllIsCalled_Then_All_BrewerysInTheRepositoryShouldBeReturend()
    {
        var BreweryModel1 = new BreweryModel { Name = "Test1", Id = Guid.NewGuid() };
        var BreweryModel2 = new BreweryModel { Name = "Test2", Id = Guid.NewGuid() };


        ((InMemAsyncRepository<Guid, BreweryModel>)_BreweryRepository)._theStore.Add(BreweryModel1.Id, BreweryModel1);
        ((InMemAsyncRepository<Guid, BreweryModel>)_BreweryRepository)._theStore.Add(BreweryModel2.Id, BreweryModel2);

        var result = await _sut.Get() as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BreweryModel1, ((List<BreweryModel>)result.Value)[0]);
        Assert.AreEqual(BreweryModel2, ((List<BreweryModel>)result.Value)[1]);
    }

    #endregion Get Test

    #region Post Tests
    
    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemInIt()
    {
        var itemToAdd = new AddABreweryRequest()
        {
            Name = "Test"
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BreweryModel>)result.Value).Count);

        var postResult = await _sut.Post(itemToAdd) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(1, ((List<BreweryModel>)result.Value).Count);
    }

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemWiththeCorrectValuesInIt()
    {
        var itemToAdd = new AddABreweryRequest()
        {
            Name = "Test"
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BreweryModel>)result.Value).Count);

        var postResult = await _sut.Post(itemToAdd) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(itemToAdd.Name, ((List<BreweryModel>)result.Value)[0].Name);       
    }

    [Test]
    public async Task Given_AnInvalidItemIsAdded_Then_AValidationErrorShouldOccur()
    {
        _mockAddABreweryValidator.Setup(i => i.Validate(It.IsAny<AddABreweryRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = false });

        var postResult = await _sut.Post(new AddABreweryRequest()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, postResult.StatusCode);        
    }

    #endregion Post Tests

    #region Put Tests

    [Test]
    public async Task Given_TheItemExistsInTheRepository_When_AnItemIsUpdated_Then_TheRepositoryValueShouldChange()
    {
        var itemToAdd = new AddABreweryRequest()
        {
            Name = "Test"
        };
        
        var postResult = await _sut.Post(itemToAdd)as ObjectResult;
        
        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(1, ((List<BreweryModel>)result.Value).Count);

        var itemToUpdate = new UpdateABreweryRequest
        {
            Id = (Guid)postResult.Value,
            Name = "Test2"
        };

        var updateResult = await _sut.Put(itemToUpdate) as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.OK, updateResult.StatusCode);


        result = await _sut.Get(itemToUpdate.Id) as ObjectResult;

        Assert.AreEqual(itemToUpdate.Name, ((BreweryModel)result.Value).Name);
    }

    [Test]
    public async Task Given_TheItemDoesNotExistsInTheRepository_When_AnItemIsUpdated_Then_ItemNotFoundCodeShouldBeReturned()
    {     
        //ensure that the repo is empty
        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BreweryModel>)result.Value).Count);


        var itemToUpdate = new UpdateABreweryRequest
        {
            Id = Guid.NewGuid(),
            Name = "Test2"
        };

        var updateResult = await _sut.Put(itemToUpdate) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.NotFound, updateResult.StatusCode);
    }


    [Test]
    public async Task Given_AnInvalidItemIsUpdated_Then_AValidationErrorShouldOccur()
    {
        _mockUpdateABreweryValidator.Setup(i => i.Validate(It.IsAny<UpdateABreweryRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = false });
        
        var postResult = await _sut.Put(new UpdateABreweryRequest()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, postResult.StatusCode);
    }

    #endregion Post Tests

}
