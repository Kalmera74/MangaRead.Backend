namespace MangaRead.Application.UnitOfWork;
public interface IUnitOfWork
{
    Task CommitAsync();
    void Rollback();
}

