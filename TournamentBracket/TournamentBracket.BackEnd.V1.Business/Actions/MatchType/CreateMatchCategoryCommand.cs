using MediatR;
using TournamentBracket.BackEnd.V1.Business.Actions.Definitions;
using TournamentBracket.BackEnd.V1.Common.Database;
using TournamentBracket.BackEnd.V1.Common.Entity;

namespace TournamentBracket.BackEnd.V1.Business.Actions.MatchType
{

    public class CreateMatchCategoryCommand : IRequest<CreateMatchCategoryCommandResult>
    {
        public string MatchCategoryName { get; set; }
    }

    public class CreateMatchCategoryCommandResult
    {
        public bool IsSuccess { get; set; }
    }

    public class CreateMatchCategoryCommandHandler : BackEndGenericHandler, IRequestHandler<CreateMatchCategoryCommand, CreateMatchCategoryCommandResult>
    {
        public CreateMatchCategoryCommandHandler(ITournamentBracketDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CreateMatchCategoryCommandResult> Handle(CreateMatchCategoryCommand request, CancellationToken cancellationToken)
        {
            var successStatues = new CreateMatchCategoryCommandResult();
            try
            {
                var matchcategory = new MatchCategory
                {
                    MatchCategoryID = Guid.NewGuid(),
                    MatchTypeName = request.MatchCategoryName
                };

                matchCategoryRepository.MatchCategories.Add(matchcategory);
                successStatues.IsSuccess = false;
            }
            catch (Exception ex)
            {
                successStatues.IsSuccess = false;
                throw new Exception(ex.Message);
            }
            return successStatues;
        }
    }

}
