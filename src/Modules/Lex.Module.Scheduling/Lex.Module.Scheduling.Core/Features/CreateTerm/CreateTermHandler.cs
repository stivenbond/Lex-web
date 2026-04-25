using FluentValidation;
using Lex.Module.Scheduling.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Scheduling.Core.Features.CreateTerm;

public sealed record CreateTermCommand(
    int AcademicYearId,
    string Name,
    int SequenceNumber,
    DateOnly StartDate,
    DateOnly EndDate) : IRequest<Result<int>>;

internal sealed class CreateTermValidator : AbstractValidator<CreateTermCommand>
{
    public CreateTermValidator()
    {
        RuleFor(x => x.AcademicYearId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.SequenceNumber).GreaterThan(0);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
    }
}

internal sealed class CreateTermHandler(ISchedulingRepository repository) : IRequestHandler<CreateTermCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateTermCommand request, CancellationToken ct)
    {
        var academicYear = await repository.GetAcademicYearByIdAsync(request.AcademicYearId, ct);
        if (academicYear is null)
        {
            return Error.NotFound("Scheduling.AcademicYear.NotFound", "Academic year was not found.");
        }

        if (request.StartDate < academicYear.StartDate || request.EndDate > academicYear.EndDate)
        {
            return Error.Validation("Scheduling.Term.OutOfAcademicYearRange", "Term dates must be within academic year boundaries.");
        }

        var hasOverlap = await repository.HasOverlappingTermAsync(request.AcademicYearId, request.StartDate, request.EndDate, ct);
        if (hasOverlap)
        {
            return Error.Conflict("Scheduling.Term.Overlap", "Term dates overlap with an existing term.");
        }

        var term = Term.Create(request.AcademicYearId, request.Name, request.SequenceNumber, request.StartDate, request.EndDate);
        repository.AddTerm(term);
        await repository.SaveChangesAsync(ct);
        return term.Id;
    }
}
