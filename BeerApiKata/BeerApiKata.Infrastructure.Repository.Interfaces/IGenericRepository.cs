namespace BeerApiKata.Infrastructure.Repository.Interfaces;

public interface IGenericRepository<TId, TEntity>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> Get(TId id);
    Task Update(TId id,TEntity entity);
    Task Create(TId id,TEntity entity);

}
