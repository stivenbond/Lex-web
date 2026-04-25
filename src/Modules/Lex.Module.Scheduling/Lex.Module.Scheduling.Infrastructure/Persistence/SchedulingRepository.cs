using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.QuerySchedule;
using Microsoft.EntityFrameworkCore;

namespace Lex.Module.Scheduling.Persistence;

internal sealed class SchedulingRepository(SchedulingDbContext dbContext) : ISchedulingRepository
{
    public void AddAcademicYear(AcademicYear academicYear) => dbContext.AcademicYears.Add(academicYear);
    public void AddTerm(Term term) => dbContext.Terms.Add(term);
    public void AddSlot(Slot slot) => dbContext.Slots.Add(slot);
    public void AddPeriod(Period period) => dbContext.Periods.Add(period);
    public void RemovePeriod(Period period) => dbContext.Periods.Remove(period);

    public Task<AcademicYear?> GetAcademicYearByIdAsync(int id, CancellationToken ct = default) =>
        dbContext.AcademicYears.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Term?> GetTermByIdAsync(int id, CancellationToken ct = default) =>
        dbContext.Terms.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Slot?> GetSlotByIdAsync(int id, CancellationToken ct = default) =>
        dbContext.Slots.Include(x => x.Term).FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<Period?> GetPeriodByIdAsync(int id, CancellationToken ct = default) =>
        dbContext.Periods.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<bool> HasOverlappingTermAsync(int academicYearId, DateOnly startDate, DateOnly endDate, CancellationToken ct = default) =>
        dbContext.Terms.AnyAsync(
            x => x.AcademicYearId == academicYearId &&
                 x.StartDate <= endDate &&
                 x.EndDate >= startDate,
            ct);

    public Task<bool> HasConflictingPeriodAsync(
        int slotId,
        int sectionId,
        int classroomId,
        string teacherId,
        int? excludePeriodId,
        CancellationToken ct = default) =>
        dbContext.Periods.AnyAsync(
            x => x.SlotId == slotId &&
                (excludePeriodId == null || x.Id != excludePeriodId.Value) &&
                (x.SectionId == sectionId || x.ClassroomId == classroomId || x.TeacherId == teacherId),
            ct);

    public async Task<IReadOnlyList<ScheduleItemDto>> GetScheduleForClassAsync(
        int classId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken ct = default)
    {
        return await dbContext.Periods
            .AsNoTracking()
            .Where(p => p.SectionId == classId &&
                        p.Slot.Term.StartDate <= endDate &&
                        p.Slot.Term.EndDate >= startDate)
            .Select(p => new ScheduleItemDto(
                p.SlotId,
                p.Id,
                p.SectionId,
                p.TeacherId,
                p.ClassroomId,
                p.Subject,
                p.Slot.DayOfWeek,
                p.Slot.StartTime,
                p.Slot.EndTime))
            .OrderBy(x => x.DayOfWeek)
            .ThenBy(x => x.StartTime)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<ScheduleItemDto>> GetScheduleForTeacherAsync(
        string teacherId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken ct = default)
    {
        return await dbContext.Periods
            .AsNoTracking()
            .Where(p => p.TeacherId == teacherId &&
                        p.Slot.Term.StartDate <= endDate &&
                        p.Slot.Term.EndDate >= startDate)
            .Select(p => new ScheduleItemDto(
                p.SlotId,
                p.Id,
                p.SectionId,
                p.TeacherId,
                p.ClassroomId,
                p.Subject,
                p.Slot.DayOfWeek,
                p.Slot.StartTime,
                p.Slot.EndTime))
            .OrderBy(x => x.DayOfWeek)
            .ThenBy(x => x.StartTime)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<PeriodSummaryDto>> GetPeriodsForDateAsync(DateOnly date, CancellationToken ct = default)
    {
        var dayOfWeek = (int)date.DayOfWeek;
        return await dbContext.Periods
            .AsNoTracking()
            .Where(p => p.Slot.DayOfWeek == dayOfWeek &&
                        p.Slot.Term.StartDate <= date &&
                        p.Slot.Term.EndDate >= date)
            .Select(p => new PeriodSummaryDto(
                p.SlotId,
                p.Id,
                p.SectionId,
                p.TeacherId,
                p.ClassroomId,
                p.Subject,
                p.Slot.StartTime,
                p.Slot.EndTime))
            .OrderBy(x => x.StartTime)
            .ToListAsync(ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        dbContext.SaveChangesAsync(ct);
}
