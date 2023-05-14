using BeerApiKata.Infrastructure.Repository.InMemRepository;
using BeerApiKata.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PubApi.Beer.Models;
using PubApi.Brewery.Models;
using PubApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static PubApi.Controllers.BreweryBeerController;

namespace PubApi.FunctionalTests;

public class BreweryBeerFunctionalTests
{
    #region Setup
    
    BreweryBeerController _sut;
    private IGenericRepository<Guid, BreweryBeerModel> _BreweryBeerRepository;
    private IGenericRepository<Guid, BeerModel> _BeerRepository;
    private IGenericRepository<Guid, BreweryModel> _BreweryRepository;


    [SetUp]
    public void Setup()
    {
        _BreweryBeerRepository = new InMemAsyncRepository<Guid, BreweryBeerModel>();
        _BeerRepository = new InMemAsyncRepository<Guid, BeerModel>();
        _BreweryRepository = new InMemAsyncRepository<Guid, BreweryModel>();
        _sut = new BreweryBeerController(_BreweryBeerRepository, _BeerRepository, _BreweryRepository);
    }

    #endregion Setup

    #region Post Tests

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemInIt()
    {
        var itemToAdd = new AddABreweryBeerRequest()
        {
            BeerId = Guid.NewGuid(),
            BreweryId = Guid.NewGuid()
        };

        var result = await _BreweryBeerRepository.GetAll();                
        Assert.AreEqual(0, result.Count());

        var postResult = await _sut.Post(itemToAdd) as OkResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _BreweryBeerRepository.GetAll();
        Assert.AreEqual(1, result.Count());
    }

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemWiththeCorrectValuesInIt()
    {
        var itemToAdd = new AddABreweryBeerRequest()
        {
            BeerId = Guid.NewGuid(),
            BreweryId = Guid.NewGuid()
        };

        var result = await _BreweryBeerRepository.GetAll();
        Assert.AreEqual(0, result.Count());

        var postResult = await _sut.Post(itemToAdd) as OkResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _BreweryBeerRepository.GetAll();
        Assert.AreEqual(itemToAdd.BeerId, result.First().BeerId);
        Assert.AreEqual(itemToAdd.BreweryId, result.First().BreweryId);
    }

    #endregion Post Tests 


    #region GetById Tests

    [Test]
    public async Task Given_GetByIdIsCalled_AndAMatchingItemIsFound_Then_ItShouldBeReturned()
    {
        var beerModel1 = new BeerModel()
        {
            Id = Guid.NewGuid(),
            Name = "aBeer",
            PercentageAlcoholByVolume = 0.1M
        };
        var beerModel2 = new BeerModel()
        {
            Id = Guid.NewGuid(),
            Name = "aBeer2",
            PercentageAlcoholByVolume = 0.2M
        };
        await _BeerRepository.Create(beerModel1.Id, beerModel1);     
        await _BeerRepository.Create(beerModel2.Id, beerModel2);

        var breweryModel1 = new BreweryModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        var breweryModel2 = new BreweryModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        await _BreweryRepository.Create(breweryModel1.Id, breweryModel1);
        await _BreweryRepository.Create(breweryModel2.Id, breweryModel2);


        await _BreweryBeerRepository.Create(Guid.NewGuid(), new BreweryBeerModel { BreweryId = breweryModel1.Id, BeerId = beerModel1.Id });
        await _BreweryBeerRepository.Create(Guid.NewGuid(), new BreweryBeerModel { BreweryId = breweryModel1.Id, BeerId = beerModel2.Id, });
        await _BreweryBeerRepository.Create(Guid.NewGuid(), new BreweryBeerModel { BreweryId = breweryModel2.Id, BeerId = beerModel1.Id });
        
        
        var result = await _sut.Get(breweryModel1.Id) as ObjectResult;
        var breweryBeerResult = (GetBreweryBeerResponse)result.Value;

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(breweryModel1, breweryBeerResult.Brewery);
        Assert.AreEqual(2, breweryBeerResult.Beers.Count());
        Assert.AreEqual(beerModel1, breweryBeerResult.Beers.First(i=>i.Id == beerModel1.Id));
        Assert.AreEqual(beerModel2, breweryBeerResult.Beers.First(i => i.Id == beerModel2.Id));      


    }
       
    [Test]
    public async Task Given_GetByIdIsCalled_AndNoMatchingBreweryBeerIsFound_Then_404ShouldBeReturned()
    {
        var result = await _sut.Get(Guid.NewGuid()) as NotFoundResult;

        Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);

    }

    #endregion Get Test

    #region Get Tests
    [Test]
    public async Task Given_GetAllIsCalled_Then_AllItemsShouldBeReturned()
    {
        var beerModel1 = new BeerModel()
        {
            Id = Guid.NewGuid(),
            Name = "aBeer",
            PercentageAlcoholByVolume = 0.1M
        };
        var beerModel2 = new BeerModel()
        {
            Id = Guid.NewGuid(),
            Name = "aBeer2",
            PercentageAlcoholByVolume = 0.2M
        };
        await _BeerRepository.Create(beerModel1.Id, beerModel1);
        await _BeerRepository.Create(beerModel2.Id, beerModel2);

        var breweryModel1 = new BreweryModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        var breweryModel2 = new BreweryModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        await _BreweryRepository.Create(breweryModel1.Id, breweryModel1);
        await _BreweryRepository.Create(breweryModel2.Id, breweryModel2);


        await _BreweryBeerRepository.Create(Guid.NewGuid(), new BreweryBeerModel { BreweryId = breweryModel1.Id, BeerId = beerModel1.Id });
        await _BreweryBeerRepository.Create(Guid.NewGuid(), new BreweryBeerModel { BreweryId = breweryModel1.Id, BeerId = beerModel2.Id, });
        await _BreweryBeerRepository.Create(Guid.NewGuid(), new BreweryBeerModel { BreweryId = breweryModel2.Id, BeerId = beerModel1.Id });


        var result = await _sut.Get() as ObjectResult;
        var breweryBeerResult = (IEnumerable<GetBreweryBeerResponse>)result.Value;

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(2, breweryBeerResult.Count());
    }
    
    #endregion Get Test


}
