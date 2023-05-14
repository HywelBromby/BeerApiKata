using BeerApiKata.Infrastructure.Repository.InMemRepository;
using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PubApi.Beer.Models;
using PubApi.Beer.Services;
using PubApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PubApi.FunctionalTests;

public class BeerFunctionalTests
{
    #region Setup
    
    BeerController _sut; 
    private static IGenericRepository<Guid, BeerModel> _beerRepository;
    private Mock<IGenericValidator<AddABeerRequest>> _mockAddABeerValidator;
    private Mock<IGenericValidator<UpdateABeerRequest>> _mockUpdateABeerValidator;


    [SetUp]
    public void Setup()
    {
        _beerRepository = new InMemAsyncRepository<Guid, BeerModel>();

        _mockAddABeerValidator = new Mock<IGenericValidator<AddABeerRequest>>();
        _mockAddABeerValidator.Setup(i => i.Validate(It.IsAny<AddABeerRequest>())).ReturnsAsync(new BeerApiKata.Infrastructure.Validation.Models.GenericValidationResult { IsValid = true });

        _mockUpdateABeerValidator = new Mock<IGenericValidator<UpdateABeerRequest>>();
        _mockUpdateABeerValidator.Setup(i => i.Validate(It.IsAny<UpdateABeerRequest>())).ReturnsAsync(new BeerApiKata.Infrastructure.Validation.Models.GenericValidationResult { IsValid = true });

        _sut = new BeerController(new BeerService(_beerRepository, _mockAddABeerValidator.Object, _mockUpdateABeerValidator.Object));
    }

    #endregion Setup

    #region GetById Tests

    [Test]
    public async Task Given_GetByIdIsCalled_AndAMatchingItemIsFound_Then_ItShouldBeReturned()
    {
        var beerModel1 = new BeerModel { Name = "Test1", PercentageAlcoholByVolume = 0.1M, Id = Guid.NewGuid() };
       
        ((InMemAsyncRepository<Guid, BeerModel>)_beerRepository)._theStore.Add(beerModel1.Id, beerModel1);       

        var result = await _sut.Get(beerModel1.Id) as ObjectResult;

        Assert.AreEqual(beerModel1, (BeerModel)result.Value);
        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

    }

    [Test]
    public async Task Given_GetByIdIsCalled_AndNoMatchingBeerIsFound_Then_404ShouldBeReturned()
    {
        var result = await _sut.Get(Guid.NewGuid()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

    }

    #endregion Get Test

    #region Get Tests

    [Test]
    public async Task Given_GetAllBeersIsCalled_Then_All_BeersInTheRepositoryShouldBeReturend()
    {
        var beerModel1 = new BeerModel { Name = "Test1", PercentageAlcoholByVolume = 0.1M, Id = Guid.NewGuid() };
        var beerModel2 = new BeerModel { Name = "Test2", PercentageAlcoholByVolume = 0.2M, Id = Guid.NewGuid() };


        ((InMemAsyncRepository<Guid, BeerModel>)_beerRepository)._theStore.Add(beerModel1.Id, beerModel1);
        ((InMemAsyncRepository<Guid, BeerModel>)_beerRepository)._theStore.Add(beerModel2.Id, beerModel2);

        var result = await _sut.Get() as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(beerModel1, ((List<BeerModel>)result.Value)[0]);
        Assert.AreEqual(beerModel2, ((List<BeerModel>)result.Value)[1]);
    }

    #endregion Get Test

    #region Post Tests
    
    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemInIt()
    {
        var itemToAdd = new AddABeerRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BeerModel>)result.Value).Count);

        var postResult = await _sut.Post(itemToAdd) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(1, ((List<BeerModel>)result.Value).Count);
    }

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemWiththeCorrectValuesInIt()
    {
        var itemToAdd = new AddABeerRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BeerModel>)result.Value).Count);

        var postResult = await _sut.Post(itemToAdd) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(itemToAdd.Name, ((List<BeerModel>)result.Value)[0].Name);
        Assert.AreEqual(itemToAdd.PercentageAlcoholByVolume, ((List<BeerModel>)result.Value)[0].PercentageAlcoholByVolume);
    }

    [Test]
    public async Task Given_AnInvalidItemIsAdded_Then_AValidationErrorShouldOccur()
    {
        _mockAddABeerValidator.Setup(i => i.Validate(It.IsAny<AddABeerRequest>())).ReturnsAsync(new BeerApiKata.Infrastructure.Validation.Models.GenericValidationResult { IsValid = false });

        var postResult = await _sut.Post(new AddABeerRequest()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, postResult.StatusCode);        
    }

    #endregion Post Tests

    #region Put Tests

    [Test]
    public async Task Given_TheItemExistsInTheRepository_When_AnItemIsUpdated_Then_TheRepositoryValueShouldChange()
    {
        var itemToAdd = new AddABeerRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };
        
        var postResult = await _sut.Post(itemToAdd)as ObjectResult;
        
        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(1, ((List<BeerModel>)result.Value).Count);

        var itemToUpdate = new UpdateABeerRequest
        {
            Id = (Guid)postResult.Value,
            Name = "Test2",
            PercentageAlcoholByVolume = 0.75M
        };

        var updateResult = await _sut.Put(itemToUpdate) as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.OK, updateResult.StatusCode);


        result = await _sut.Get(itemToUpdate.Id) as ObjectResult;

        Assert.AreEqual(itemToUpdate.Name, ((BeerModel)result.Value).Name);
        Assert.AreEqual(itemToUpdate.PercentageAlcoholByVolume, ((BeerModel)result.Value).PercentageAlcoholByVolume);


    }

    [Test]
    public async Task Given_TheItemDoesNotExistsInTheRepository_When_AnItemIsUpdated_Then_ItemNotFoundCodeShouldBeReturned()
    {     
        //ensure that the repo is empty
        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BeerModel>)result.Value).Count);


        var itemToUpdate = new UpdateABeerRequest
        {
            Id = Guid.NewGuid(),
            Name = "Test2",
            PercentageAlcoholByVolume = 0.75M
        };

        var updateResult = await _sut.Put(itemToUpdate) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.NotFound, updateResult.StatusCode);
    }


    [Test]
    public async Task Given_AnInvalidItemIsUpdated_Then_AValidationErrorShouldOccur()
    {
        _mockUpdateABeerValidator.Setup(i => i.Validate(It.IsAny<UpdateABeerRequest>())).ReturnsAsync(new BeerApiKata.Infrastructure.Validation.Models.GenericValidationResult { IsValid = false });
        
        var postResult = await _sut.Put(new UpdateABeerRequest()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, postResult.StatusCode);
    }

    #endregion Post Tests



    //todo: filter beers by querystring tests

}
