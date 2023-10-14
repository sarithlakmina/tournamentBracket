using MediatR;
using TournamentBracket.BackEnd.V1.Common.Common;

namespace TournamentBracket.BackEnd.V1.Business.Actions.Tournaments;


public class CreateTournamentsCommand : IRequest<CreateTournamentsCommandResult>
{
    public string TypeOfMatch { get; set; }
    public List<SeedDetails> SeedDetails { get; set; }

    public Guid TournamentID { get; set; }

}

public class CreateTournamentsCommandResult
{ }

public class CreateTournamentsCommandHandler : IRequestHandler<CreateTournamentsCommand, CreateTournamentsCommandResult>
{
    public CreateTournamentsCommandHandler()
    {

    }

    public async Task<CreateTournamentsCommandResult> Handle(CreateTournamentsCommand request, CancellationToken cancellationToken)
    {


        return new CreateTournamentsCommandResult();
    }
}
