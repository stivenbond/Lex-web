using Lex.Module.AssessmentCreation.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.AssessmentCreation.Core.Features.CreateAssessment;

public sealed record CreateAssessmentCommand(string Title) : IRequest<Result<Guid>>;

internal sealed class CreateAssessmentCommandHandler(IAssessmentRepository repository) : IRequestHandler<CreateAssessmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateAssessmentCommand request, CancellationToken cancellationToken)
    {
        var assessment = Assessment.Create(request.Title);
        repository.AddAssessment(assessment);
        await repository.SaveChangesAsync(cancellationToken);
        return assessment.Id;
    }
}
