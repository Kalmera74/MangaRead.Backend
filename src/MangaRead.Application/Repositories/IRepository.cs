namespace MangaRead.Application.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>?> GetAllAsync();
    Task<IEnumerable<T>?> GetAllPagedAsync(int pageNumber, int pageSize);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    void MarkAsModified(T entity);
    Task DeleteAsync(T entity);
}
