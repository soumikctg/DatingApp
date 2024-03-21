namespace UserAPI.Interfaces;

public interface IUnitOfWork
{
    Task<long> SaveChangesAsync();
}