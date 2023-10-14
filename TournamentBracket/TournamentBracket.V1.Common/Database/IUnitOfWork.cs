namespace TournamentBracket.BackEnd.V1.Common.Database;

public interface IUnitOfWork
{
    int SaveChanges();

    Task<int> SaveChangesAsync();

    int SaveChanges(bool acceptAllChangesOnSuccess);

    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess);
}
