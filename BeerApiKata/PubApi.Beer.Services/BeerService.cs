using BeerApiKata.Infrastructure.Repository.Interfaces;
using PubApi.Beer.Interfaces;
using PubApi.Domain.Beer.Models;

namespace PubApi.Beer.Services;

public class BeerService : IBeerService
{
    private readonly IGenericRepository<Guid, BeerModel> _repository;

    public BeerService(IGenericRepository<Guid, BeerModel> repository)
    {
        _repository = repository;
    }

    public async Task AddABeer(BeerModel request)
    {
        //validate model
        //update repo
        var id = Guid.NewGuid();
        await _repository.Create(id, request);
        //format response        
    }

    public Task<IEnumerable<BeerModel>> GetAllBeers()
    {
        return _repository.GetAll();
    }
}
