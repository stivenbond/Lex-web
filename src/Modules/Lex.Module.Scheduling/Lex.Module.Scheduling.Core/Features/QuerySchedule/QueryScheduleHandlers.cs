using FluentValidation;
using Lex.Module.Scheduling.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.Scheduling.Core.Features.QuerySchedule;

public sealed record ScheduleItemDto(
    int SlotId,
    int AssignmentId,
    int ClassId,
    string TeacherId,
    int ClassroomId,
    string Subject,
    int DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime);

public sealed record PeriodSummaryDto(
    int SlotId,
    int AssignmentId,
    int ClassId,
    string TeacherId,
    int ClassroomId,
    string Subject,
    TimeOnly StartTime,
    TimeOnly EndTime);

public sealed record GetScheduleForClassQuery(int ClassId, DateOnly StartDate, DateOnly EndDate) : IRequest<Result<IReadOnlyList<ScheduleItemDto>>>;
public sealed record GetScheduleForTeacherQuery(string TeacherId, DateOnly StartDate, DateOnly EndDate) : IRequest<Result<IReadOnlyList<ScheduleItemDto>>>;
public sealed record GetPeriodsForDateQuery(DateOnly Date) : IRequest<Result<IReadOnlyList<PeriodSummaryDto>>>;

internal sealed class GetScheduleForClassValidator : AbstractValidator<GetScheduleForClassQuery>
{
    public GetScheduleForClassValidator()
    {
        RuleFor(x => x.ClassId).GreaterThan(0);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.StartDate)
            .Must((q, start) => q.EndDate.DayNumber - start.DayNumber <= 93)
            .WithMessage("Date range cannot exceed 3 months.");
    }
}

internal sealed class GetScheduleForTeacherValidator : AbstractValidator<GetScheduleForTeacherQuery>
{
    public GetScheduleForTeacherValidator()
    {
        RuleFor(x => x.TeacherId).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.StartDate)
            .Must((q, start) => q.EndDate.DayNumber - start.DayNumber <= 93)
            .WithMessage("Date range cannot exceed 3 months.");
    }
}

internal sealed class GetScheduleForClassHandler(ISchedulingRepository repository)
    : IRequestHandler<GetScheduleForClassQuery, Result<IReadOnlyList<ScheduleItemDto>>>
{
    public async Task<Result<IReadOnlyList<ScheduleItemDto>>> Handle(GetScheduleForClassQuery request, CancellationToken ct)
    {
        var items = await repository.GetScheduleForClassAsync(request.ClassId, request.StartDate, request.EndDate, ct);
        return Result<IReadOnlyList<ScheduleItemDto>>.Success(items);
    }
}

internal sealed class GetScheduleForTeacherHandler(ISchedulingRepository repository)
    : IRequestHandler<GetScheduleForTeacherQuery, Result<IReadOnlyList<ScheduleItemDto>>>
{
    public async Task<Result<IReadOnlyList<ScheduleItemDto>>> Handle(GetScheduleForTeacherQuery request, CancellationToken ct)
    {
        var items = await repository.GetScheduleForTeacherAsync(request.TeacherId, request.StartDate, request.EndDate, ct);
        return Result<IReadOnlyList<ScheduleItemDto>>.Success(items);
    }
}

internal sealed class GetPeriodsForDateHandler(ISchedulingRepository repository)
    : IRequestHandler<GetPeriodsForDateQuery, Result<IReadOnlyList<PeriodSummaryDto>>>
{
    public async Task<Result<IReadOnlyList<PeriodSummaryDto>>> Handle(GetPeriodsForDateQuery request, CancellationToken ct)
    {
        var items = await repository.GetPeriodsForDateAsync(request.Date, ct);
        return Result<IReadOnlyList<PeriodSummaryDto>>.Success(items);
    }
}
