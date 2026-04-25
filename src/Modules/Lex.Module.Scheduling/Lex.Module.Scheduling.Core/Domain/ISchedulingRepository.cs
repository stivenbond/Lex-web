using Lex.Module.Scheduling.Core.Features.QuerySchedule;

namespace Lex.Module.Scheduling.Core.Domain;

public interface ISchedulingRepository
{
    void AddAcademicYear(AcademicYear academicYear);
    void AddTerm(Term term);
    void AddSlot(Slot slot);
    void AddPeriod(Period period);
    void RemovePeriod(Period period);

    Task<AcademicYear?> GetAcademicYearByIdAsync(int id, CancellationToken ct = default);
    Task<Term?> GetTermByIdAsync(int id, CancellationToken ct = default);
    Task<Slot?> GetSlotByIdAsync(int id, CancellationToken ct = default);
    Task<Period?> GetPeriodByIdAsync(int id, CancellationToken ct = default);

    Task<bool> HasOverlappingTermAsync(int academicYearId, DateOnly startDate, DateOnly endDate, CancellationToken ct = default);
    Task<bool> HasConflictingPeriodAsync(int slotId, int sectionId, int classroomId, string teacherId, int? excludePeriodId, CancellationToken ct = default);

    Task<IReadOnlyList<ScheduleItemDto>> GetScheduleForClassAsync(int classId, DateOnly startDate, DateOnly endDate, CancellationToken ct = default);
    Task<IReadOnlyList<ScheduleItemDto>> GetScheduleForTeacherAsync(string teacherId, DateOnly startDate, DateOnly endDate, CancellationToken ct = default);
    Task<IReadOnlyList<PeriodSummaryDto>> GetPeriodsForDateAsync(DateOnly date, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
