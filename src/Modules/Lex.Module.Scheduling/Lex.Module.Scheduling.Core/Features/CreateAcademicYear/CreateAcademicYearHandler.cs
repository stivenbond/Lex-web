using FluentValidation;
using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Scheduling.Core.Features.CreateAcademicYear;

public sealed record TermInput(string Name, DateOnly StartDate, DateOnly EndDate);

public sealed record CreateAcademicYearCommand(
    string Name,
    int Year,
    DateOnly StartDate,
    DateOnly EndDate,
    IReadOnlyList<TermInput>? Terms) : IRequest<Result<int>>;

internal sealed class CreateAcademicYearValidator : AbstractValidator<CreateAcademicYearCommand>
{
    public CreateAcademicYearValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Year).GreaterThan(2000);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        RuleForEach(x => x.Terms).SetValidator(new TermInputValidator());
    }

    private sealed class TermInputValidator : AbstractValidator<TermInput>
    {
        public TermInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }
}

internal sealed class CreateAcademicYearHandler(
    ISchedulingRepository repository,
    ISchedulingEventPublisher eventPublisher) : IRequestHandler<CreateAcademicYearCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateAcademicYearCommand request, CancellationToken ct)
    {
        if (request.Terms is { Count: > 0 })
        {
            foreach (var termInput in request.Terms)
            {
                if (termInput.StartDate < request.StartDate || termInput.EndDate > request.EndDate)
                {
                    return Error.Validation("Scheduling.Term.OutOfAcademicYearRange", "Term dates must be within academic year boundaries.");
                }
            }

            var ordered = request.Terms.OrderBy(t => t.StartDate).ToList();
            for (var i = 1; i < ordered.Count; i++)
            {
                if (ordered[i].StartDate <= ordered[i - 1].EndDate)
                {
                    return Error.Conflict("Scheduling.Term.Overlap", "Terms in the same academic year must not overlap.");
                }
            }
        }

        var year = AcademicYear.Create(request.Name, request.Year, request.StartDate, request.EndDate);
        repository.AddAcademicYear(year);
        await repository.SaveChangesAsync(ct);

        if (request.Terms is { Count: > 0 })
        {
            var ordered = request.Terms.OrderBy(t => t.StartDate).ToList();
            for (var i = 0; i < ordered.Count; i++)
            {
                repository.AddTerm(Term.Create(year.Id, ordered[i].Name, i + 1, ordered[i].StartDate, ordered[i].EndDate));
            }
        }

        await repository.SaveChangesAsync(ct);

        await eventPublisher.PublishAcademicYearCreatedAsync(new AcademicYearCreatedEvent(year.Id, year.Name), ct);
        return year.Id;
    }
}
