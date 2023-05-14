using BeerApiKata.Infrastructure.Repository.InMemRepository;
using BeerApiKata.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using PubApi.Bar.Models;
using PubApi.Beer.Models;
using PubApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static PubApi.Controllers.BarBeerController;

namespace PubApi.FunctionalTests;

public class BarBeerFunctionalTests
{
    #region Setup
    
    BarBeerController _sut;
    private IGenericRepository<Guid, BarBeerModel> _BarBeerRepository;
    private IGenericRepository<Guid, BeerModel> _BeerRepository;
    private IGenericRepository<Guid, BarModel> _BarRepository;


    [SetUp]
    public void Setup()
    {
        _BarBeerRepository = new InMemAsyncRepository<Guid, BarBeerModel>();
        _BeerRepository = new InMemAsyncRepository<Guid, BeerModel>();
        _BarRepository = new InMemAsyncRepository<Guid, BarModel>();
        _sut = new BarBeerController(_BarBeerRepository, _BeerRepository, _BarRepository);
    }

    #endregion Setup

    #region Post Tests

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemInIt()
    {
        var barModel = new BarModel
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Address = "test"
        };
        await _BarRepository.Create(barModel.Id, barModel);

        var beerModel = new BeerModel
        {
            Id = Guid.NewGuid(),
            Name = "test",
            PercentageAlcoholByVolume = 0.5M
        };
        await _BeerRepository.Create(beerModel.Id, beerModel);


        var itemToAdd = new AddABarBeerRequest()
        {
            BeerId = beerModel.Id,
            BarId = barModel.Id,
        };

        var result = await _BarBeerRepository.GetAll();                
        Assert.AreEqual(0, result.Count());

        var postResult = await _sut.Post(itemToAdd) as OkResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _BarBeerRepository.GetAll();
        Assert.AreEqual(1, result.Count());
    }

    [Test]
    public async Task Given_TheRepositoryIsEmpty_When_AValidItemIsAdded_Then_TheRepository_ShouldHaveAnItemWiththeCorrectValuesInIt()
    {
        var barModel = new BarModel
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Address = "test"
        };
        await _BarRepository.Create(barModel.Id, barModel);

        var beerModel = new BeerModel
        {
            Id = Guid.NewGuid(),
            Name = "test",
            PercentageAlcoholByVolume = 0.5M
        };
        await _BeerRepository.Create(beerModel.Id, beerModel);


        var itemToAdd = new AddABarBeerRequest()
        {
            BeerId = beerModel.Id,
            BarId = barModel.Id,
        };

        var result = await _BarBeerRepository.GetAll();
        Assert.AreEqual(0, result.Count());

        var postResult = await _sut.Post(itemToAdd) as OkResult;

        Assert.AreEqual((int)HttpStatusCode.OK, postResult.StatusCode);

        result = await _BarBeerRepository.GetAll();
        Assert.AreEqual(itemToAdd.BeerId, result.First().BeerId);
        Assert.AreEqual(itemToAdd.BarId, result.First().BarId);
    }


    [Test]
    public async Task Given_AnInvalidIDIsEntered_ThenABadRequestShuoldBeReturend()
    {
        var result = await _sut.Post(new AddABarBeerRequest  { BeerId = Guid.NewGuid(), BarId = Guid.NewGuid() }) as BadRequestResult;
        Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);

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

        var barModel1 = new BarModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        var barModel2 = new BarModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        await _BarRepository.Create(barModel1.Id, barModel1);
        await _BarRepository.Create(barModel2.Id, barModel2);


        await _BarBeerRepository.Create(Guid.NewGuid(), new BarBeerModel { BarId = barModel1.Id, BeerId = beerModel1.Id });
        await _BarBeerRepository.Create(Guid.NewGuid(), new BarBeerModel { BarId = barModel1.Id, BeerId = beerModel2.Id, });
        await _BarBeerRepository.Create(Guid.NewGuid(), new BarBeerModel { BarId = barModel2.Id, BeerId = beerModel1.Id });
        
        
        var result = await _sut.Get(barModel1.Id) as ObjectResult;
        var BarBeerResult = (GetBarBeerResponse)result.Value;

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(barModel1, BarBeerResult.Bar);
        Assert.AreEqual(2, BarBeerResult.Beers.Count());
        Assert.AreEqual(beerModel1, BarBeerResult.Beers.First(i=>i.Id == beerModel1.Id));
        Assert.AreEqual(beerModel2, BarBeerResult.Beers.First(i => i.Id == beerModel2.Id));      


    }
       
    [Test]
    public async Task Given_GetByIdIsCalled_AndNoMatchingBarBeerIsFound_Then_404ShouldBeReturned()
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

        var barModel1 = new BarModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        var barModel2 = new BarModel
        {
            Id = Guid.NewGuid(),
            Name = "Brains"
        };

        await _BarRepository.Create(barModel1.Id, barModel1);
        await _BarRepository.Create(barModel2.Id, barModel2);


        await _BarBeerRepository.Create(Guid.NewGuid(), new BarBeerModel { BarId = barModel1.Id, BeerId = beerModel1.Id });
        await _BarBeerRepository.Create(Guid.NewGuid(), new BarBeerModel { BarId = barModel1.Id, BeerId = beerModel2.Id, });
        await _BarBeerRepository.Create(Guid.NewGuid(), new BarBeerModel { BarId = barModel2.Id, BeerId = beerModel1.Id });


        var result = await _sut.Get() as ObjectResult;
        var BarBeerResult = (IEnumerable<GetBarBeerResponse>)result.Value;

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        Assert.AreEqual(2, BarBeerResult.Count());
    }
    
    #endregion Get Test

}
