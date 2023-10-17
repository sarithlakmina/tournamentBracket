using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Constants;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;

public class CreateTournamentsCommand : IRequest<CreateTournamentsCommandResult>
{
    public string Name { get; set; }
}

public class CreateTournamentsCommandResult
{
    public Guid TournamentID { get; set; }
}

public class CreateTournamentsCommandHandler : BackEndGenericHandler, IRequestHandler<CreateTournamentsCommand, CreateTournamentsCommandResult>
{
    public CreateTournamentsCommandHandler(ITournamentBracketDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<CreateTournamentsCommandResult> Handle(CreateTournamentsCommand request, CancellationToken cancellationToken)
    {
        if (request.Name == null)
            throw new Exception(ExceptionMessages.TournamentNameCannotBeEmpty);

        var isNameUniqueToCreate = await tournamentRepository.IsTournamentNameUniqueToCreate(request.Name);

        if (!isNameUniqueToCreate)
            throw new Exception(ExceptionMessages.TournamentNameCannotBeEmpty);

        var tournament = new Tournament
        {
            TournamentID = Guid.NewGuid(),
            Name = request.Name,
        };

        tournamentRepository.Tournaments.Add(tournament);

        return new CreateTournamentsCommandResult
        {
            TournamentID = tournament.TournamentID
        };
    }
}
