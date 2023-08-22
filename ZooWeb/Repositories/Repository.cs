namespace ZooWeb.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    private readonly ZooContext _db;
    private readonly DbSet<T> _set;
    public Repository(ZooContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    #region Count Method
    public async Task<int> GetEntitiesCountAsync() => await _set.CountAsync();
    #endregion

    #region Get Methods
    public async Task<IEnumerable<T>> GetAllEntitiesAsync() => await _set.ToListAsync();
    public async Task<IEnumerable<T>> GetEntitiesPageAsync(int pageNumber, int pageSize) => await _set.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    public async Task<T?> GetEntityByIdAsync(int id) => await _set.FindAsync(id);
    public async Task<T?> GetEntityAsync(Expression<Func<T, bool>> condiotion) => await _set.FirstOrDefaultAsync(condiotion);
    #endregion

    #region Add Method
    public async Task<bool> AddEntityAsync(T Entity)
    {
        await _set.AddAsync(Entity);
        return await Save();
    }
    #endregion

    #region Update methods
    public async Task<bool> UpdateEntityAsync(T Entity)
    {
        _set.Update(Entity);
        return await Save();

    }
    public abstract Task<bool> UpdateEntityByIdAsync(int id, T Entity);
    #endregion

    #region Delete Methods
    public async Task<bool> DeleteEntityAsync(T Entity)
    {
        _set.Remove(Entity);
        return await Save();
    }
    public async Task<bool> DeleteEntityByIdAsync(int id)
    {
        var entity = await GetEntityByIdAsync(id);
        if (entity == null) return false;
        return await DeleteEntityAsync(entity);
    }
    public async Task<bool> DeleteEntitiesAsync(IEnumerable<T> Entities)
    {
        _set.RemoveRange(Entities);
        return await Save();
    }
    #endregion

    #region Save Method
    public async Task<bool> Save() => await _db.SaveChangesAsync() > 0;
    #endregion


}
