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
using System.Threading.Tasks;

namespace PubApi.FunctionalTests;

public class BeerFunctionalTests
{
    #region Setup
    
    BeerController _sut; 
    private static IGenericRepository<Guid, BeerModel> _beerRepository;
    private Mock<IGenericValidator<AddABeerRequest>> _mockAddABeerValidator;


    [SetUp]
    public void Setup()
    {
        _beerRepository = new InMemAsyncRepository<Guid, BeerModel>();

        _mockAddABeerValidator = new Mock<IGenericValidator<AddABeerRequest>>();
        _mockAddABeerValidator.Setup(i => i.Validate(It.IsAny<AddABeerRequest>())).ReturnsAsync(new BeerApiKata.Infrastructure.Validation.Models.GenericValidationResult { IsValid = true });

        _sut = new BeerController(new BeerService(_beerRepository, _mockAddABeerValidator.Object));
    }

    #endregion Setup

    #region Get Tests

    [Test]
    public async Task Given_GetAllBeersIsCalled_Then_All_BeersInTheRepositoryShouldBeReturend()
    {
        var beerModel1 = new BeerModel { Name = "Test1", PercentageAlcoholByVolume = 0.1M, Id = Guid.NewGuid() };
        var beerModel2 = new BeerModel { Name = "Test2", PercentageAlcoholByVolume = 0.2M, Id = Guid.NewGuid() };


        ((InMemAsyncRepository<Guid, BeerModel>)_beerRepository)._theStore.Add(beerModel1.Id, beerModel1);
        ((InMemAsyncRepository<Guid, BeerModel>)_beerRepository)._theStore.Add(beerModel2.Id, beerModel2);

        var result = await _sut.Get() as ObjectResult;

        Assert.AreEqual(beerModel1, ((List<BeerModel>)result.Value)[0]);
        Assert.AreEqual(beerModel2, ((List<BeerModel>)result.Value)[1]);
    }

    #endregion Get Test

    #region Post Tests
    
    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AnItemIsAdded_Then_TheRepository_ShouldHaveAnItemInIt()
    {
        var itemToAdd = new AddABeerRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BeerModel>)result.Value).Count);

        await _sut.Post(itemToAdd);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(1, ((List<BeerModel>)result.Value).Count);
    }

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AnItemIsAdded_Then_TheRepository_ShouldHaveAnItemWiththeCorrectValuesInIt()
    {
        var itemToAdd = new AddABeerRequest()
        {
            Name = "Test",
            PercentageAlcoholByVolume = 0.5M
        };

        var result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(0, ((List<BeerModel>)result.Value).Count);

        await _sut.Post(itemToAdd);

        result = await _sut.Get() as ObjectResult;
        Assert.AreEqual(itemToAdd.Name, ((List<BeerModel>)result.Value)[0].Name);
        Assert.AreEqual(itemToAdd.PercentageAlcoholByVolume, ((List<BeerModel>)result.Value)[0].PercentageAlcoholByVolume);
    }

    #endregion Post Tests

}
