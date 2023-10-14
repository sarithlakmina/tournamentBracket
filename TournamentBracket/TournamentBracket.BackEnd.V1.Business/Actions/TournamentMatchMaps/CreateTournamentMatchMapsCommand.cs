using MediatR;

namespace TournamentBracket.BackEnd.V1.Business.Actions.TournamentMatchMaps
{

    public class CreateTournamentMatchMapsCommand : IRequest<CreateTournamentMatchMapsCommandResult>
    { }

    public class CreateTournamentMatchMapsCommandResult
    { }

    public class CreateTournamentMatchMapsCommandHandler : IRequestHandler<CreateTournamentMatchMapsCommand, CreateTournamentMatchMapsCommandResult>
    {
        public CreateTournamentMatchMapsCommandHandler()
        {

        }

        public async Task<CreateTournamentMatchMapsCommandResult> Handle(CreateTournamentMatchMapsCommand request, CancellationToken cancellationToken)
        {
            return new CreateTournamentMatchMapsCommandResult();
        }
    }

}
