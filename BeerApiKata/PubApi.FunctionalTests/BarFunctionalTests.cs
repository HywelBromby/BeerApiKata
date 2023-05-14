using BeerApiKata.Infrastructure.Repository.InMemRepository;
using BeerApiKata.Infrastructure.Repository.Interfaces;
using BeerApiKata.Infrastructure.Validation.Interfaces;
using BeerApiKata.Infrastructure.Validation.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PubApi.Bar.Models;
using PubApi.Bar.Services;
using PubApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PubApi.FunctionalTests;

public class BarFunctionalTests
{
    #region Setup
    
    BarController _sut; 
    private static IGenericRepository<Guid, BarModel> _BarRepository;
    private Mock<IGenericValidator<AddABarRequest>> _mockAddABarValidator;
    private Mock<IGenericValidator<UpdateABarRequest>> _mockUpdateABarValidator;


    [SetUp]
    public void Setup()
    {
        _BarRepository = new InMemAsyncRepository<Guid, BarModel>();

        _mockAddABarValidator = new Mock<IGenericValidator<AddABarRequest>>();
        _mockAddABarValidator.Setup(i => i.Validate(It.IsAny<AddABarRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = true });

        _mockUpdateABarValidator = new Mock<IGenericValidator<UpdateABarRequest>>();
        _mockUpdateABarValidator.Setup(i => i.Validate(It.IsAny<UpdateABarRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = true });

        _sut = new BarController(new BarService(_BarRepository, _mockAddABarValidator.Object, _mockUpdateABarValidator.Object));
    }

    #endregion Setup

    #region GetById Tests

    [Test]
    public async Task Given_GetByIdIsCalled_AndAMatchingItemIsFound_Then_ItShouldBeReturned()
    {
        var BarModel1 = new BarModel { Name = "Test1", PercentageAlcoholByVolume = 0.1M, Id = Guid.NewGuid() };
       
        ((InMemAsyncRepository<Guid, BarModel>)_BarRepository)._theStore.Add(BarModel1.Id, BarModel1);       

        var result = await _sut.Get(BarModel1.Id) as ObjectResult;

        Assert.AreEqual(BarModel1, (BarModel)result.Value);
        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

    }

    [Test]
    public async Task Given_GetByIdIsCalled_AndNoMatchingBarIsFound_Then_404ShouldBeReturned()
    {
        var result = await _sut.Get(Guid.NewGuid()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

    }

    #endregion Get Test

    #region Get Tests

    [Test]
    public async Task Given_GetAllIsCalled_Then_All_BarsInTheRepositoryShouldBeReturend()
    {
        var BarModel1 = new BarModel { Name = "Test1", PercentageAlcoholByVolume = 0.1M, Id = Guid.NewGuid() };
        var BarModel2 = new BarModel { Name = "Test2", PercentageAlcoholByVolume = 0.2M, Id = Guid.NewGuid() };


        ((InMemAsyncRepository<Guid, BarModel>)_BarRepository)._theStore.Add(BarModel1.Id, BarModel1);
        ((InMemAsyncRepository<Guid, BarModel>)_BarRepository)._theStore.Add(BarModel2.Id, BarModel2);

        var result = await _sut.Get() as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(BarModel1, ((List<BarModel>)result.Value)[0]);
        Assert.AreEqual(BarModel2, ((List<BarModel>)result.Value)[1]);
    }

    #endregion Get Test

    #region Post Tests
    
    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemInIt()
    {
        var itemToAdd = new AddABarRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BarModel>)result.Value).Count);

        var postResult = await _sut.Post(itemToAdd) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(1, ((List<BarModel>)result.Value).Count);
    }

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemWiththeCorrectValuesInIt()
    {
        var itemToAdd = new AddABarRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BarModel>)result.Value).Count);

        var postResult = await _sut.Post(itemToAdd) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(itemToAdd.Name, ((List<BarModel>)result.Value)[0].Name);
        Assert.AreEqual(itemToAdd.PercentageAlcoholByVolume, ((List<BarModel>)result.Value)[0].PercentageAlcoholByVolume);
    }

    [Test]
    public async Task Given_AnInvalidItemIsAdded_Then_AValidationErrorShouldOccur()
    {
        _mockAddABarValidator.Setup(i => i.Validate(It.IsAny<AddABarRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = false });

        var postResult = await _sut.Post(new AddABarRequest()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, postResult.StatusCode);        
    }

    #endregion Post Tests

    #region Put Tests

    [Test]
    public async Task Given_TheItemExistsInTheRepository_When_AnItemIsUpdated_Then_TheRepositoryValueShouldChange()
    {
        var itemToAdd = new AddABarRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };
        
        var postResult = await _sut.Post(itemToAdd)as ObjectResult;
        
        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(1, ((List<BarModel>)result.Value).Count);

        var itemToUpdate = new UpdateABarRequest
        {
            Id = (Guid)postResult.Value,
            Name = "Test2",
            PercentageAlcoholByVolume = 0.75M
        };

        var updateResult = await _sut.Put(itemToUpdate) as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.OK, updateResult.StatusCode);


        result = await _sut.Get(itemToUpdate.Id) as ObjectResult;

        Assert.AreEqual(itemToUpdate.Name, ((BarModel)result.Value).Name);
        Assert.AreEqual(itemToUpdate.PercentageAlcoholByVolume, ((BarModel)result.Value).PercentageAlcoholByVolume);


    }

    [Test]
    public async Task Given_TheItemDoesNotExistsInTheRepository_When_AnItemIsUpdated_Then_ItemNotFoundCodeShouldBeReturned()
    {     
        //ensure that the repo is empty
        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BarModel>)result.Value).Count);


        var itemToUpdate = new UpdateABarRequest
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
        _mockUpdateABarValidator.Setup(i => i.Validate(It.IsAny<UpdateABarRequest>())).ReturnsAsync(new GenericValidationResult { IsValid = false });
        
        var postResult = await _sut.Put(new UpdateABarRequest()) as ObjectResult;

        Assert.AreEqual((int)HttpStatusCode.BadRequest, postResult.StatusCode);
    }

    #endregion Post Tests
    
}
