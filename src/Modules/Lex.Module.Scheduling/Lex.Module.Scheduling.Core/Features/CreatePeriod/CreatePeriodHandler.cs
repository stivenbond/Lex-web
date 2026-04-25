using FluentValidation;
using Lex.Module.Scheduling.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Scheduling.Core.Features.CreatePeriod;

public sealed record CreatePeriodCommand(
    int TermId,
    int DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotNumber) : IRequest<Result<int>>;

internal sealed class CreatePeriodValidator : AbstractValidator<CreatePeriodCommand>
{
    public CreatePeriodValidator()
    {
        RuleFor(x => x.TermId).GreaterThan(0);
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.SlotNumber).GreaterThan(0);
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
    }
}

internal sealed class CreatePeriodHandler(ISchedulingRepository repository) : IRequestHandler<CreatePeriodCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreatePeriodCommand request, CancellationToken ct)
    {
        var term = await repository.GetTermByIdAsync(request.TermId, ct);
        if (term is null)
        {
            return Error.NotFound("Scheduling.Term.NotFound", "Term was not found.");
        }

        var slot = Slot.Create(
            request.TermId,
            (DayOfWeek)request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            request.SlotNumber);

        repository.AddSlot(slot);
        await repository.SaveChangesAsync(ct);
        return slot.Id;
    }
}
