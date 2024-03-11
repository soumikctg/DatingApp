namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        Task<long> SaveChangesAsync();
    }
}
