using MediatR;

namespace TournamentBracket.BackEnd.V1.Business.Actions.TournamentTeamMaps;


public class CreateTournamentTeamMapsCommand : IRequest<CreateTournamentTeamMapsCommandResult>
{ }

public class CreateTournamentTeamMapsCommandResult
{ }

public class CreateTournamentTeamMapsCommandHandler : IRequestHandler<CreateTournamentTeamMapsCommand, CreateTournamentTeamMapsCommandResult>
{
    public CreateTournamentTeamMapsCommandHandler()
    {

    }

    public async Task<CreateTournamentTeamMapsCommandResult> Handle(CreateTournamentTeamMapsCommand request, CancellationToken cancellationToken)
    {
        return new CreateTournamentTeamMapsCommandResult();
    }
}
