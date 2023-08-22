namespace ZooWeb.Repositories;

public interface IRepository<T> where T : class
{
    #region Count Methods
    Task<int> GetEntitiesCountAsync();
    #endregion

    #region Get Methods
    Task<IEnumerable<T>> GetAllEntitiesAsync();
    Task<IEnumerable<T>> GetEntitiesPageAsync(int pageNumber, int pageSize);
    Task<T?> GetEntityByIdAsync(int id);
    Task<T?> GetEntityAsync(Expression<Func<T, bool>> condiotion);
    #endregion

    #region Add Method
    Task<bool> AddEntityAsync(T Entity);
    #endregion

    #region Update Methods
    Task<bool> UpdateEntityAsync(T Entity);
    Task<bool> UpdateEntityByIdAsync(int Id, T Entity);
    #endregion

    #region Delete Methods
    Task<bool> DeleteEntityAsync(T Entity);
    Task<bool> DeleteEntityByIdAsync(int id);
    Task<bool> DeleteEntitiesAsync(IEnumerable<T> Entities);
    #endregion

    #region Save Method
    Task<bool> Save();

    #endregion

}
