using TournamentBracket.BackEnd.V1.Common.Database;

namespace TournamentBracket.BackEnd.V1.Persistence.EFCustomizations;

public partial class TournamentBracketDbContext : ITournamentBracketDbContext, IUnitOfWork
{

    public Task<int> SaveChangesAsync()
    {
        try
        {
            return base.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }
    public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess)
    {
        try
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess);
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }
    }

}
