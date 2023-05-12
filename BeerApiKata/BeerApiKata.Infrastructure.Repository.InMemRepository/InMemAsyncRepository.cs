using BeerApiKata.Infrastructure.Repository.Interfaces;

namespace BeerApiKata.Infrastructure.Repository.InMemRepository;

public class InMemAsyncRepository<TId, TEntity> : IGenericRepository<TId, TEntity>
{
    public static readonly Dictionary<TId, TEntity> _theStore = new Dictionary<TId, TEntity>();

    public async Task Create(TId id, TEntity entity)
    {
        if (id==null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _theStore.Add(id, entity);
    }

    public async Task<TEntity> Get(TId id)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        if(!_theStore.TryGetValue(id, out TEntity entity))
        {
            return default(TEntity);
        }

        return entity;

    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return _theStore.Values.ToList();
    }

    public async Task Update(TId id, TEntity entity)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (_theStore.TryGetValue(id, out TEntity entityInStore))
        {
            _theStore[id] = entity;            
        }
    }
}
