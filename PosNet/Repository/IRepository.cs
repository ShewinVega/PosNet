namespace PosNet.Repository
{
    public interface IRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> All();

        public Task<TEntity> GetById(Guid id);

        public Task Create(TEntity request);

        public Task Update(Guid id, TEntity request);

        public Task Delete(Guid id);

        public Task Save();
    }
}
