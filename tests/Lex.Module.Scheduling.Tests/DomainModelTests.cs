using FluentAssertions;
using Lex.Module.Scheduling.Core.Domain;
using Xunit;

namespace Lex.Module.Scheduling.Tests;

/// <summary>
/// Tests for AcademicYear domain model invariants and operations.
/// </summary>
public sealed class AcademicYearDomainTests
{
    [Fact]
    public void Create_WithValidDates_CreatesSuccessfully()
    {
        var startDate = new DateOnly(2026, 9, 1);
        var endDate = new DateOnly(2027, 6, 30);

        var year = AcademicYear.Create("2026-2027", 2026, startDate, endDate);

        year.Should().NotBeNull();
        year.Name.Should().Be("2026-2027");
        year.Year.Should().Be(2026);
        year.StartDate.Should().Be(startDate);
        year.EndDate.Should().Be(endDate);
        year.IsActive.Should().BeFalse();
        year.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithEndDateBeforeStartDate_ThrowsArgumentOutOfRangeException()
    {
        var startDate = new DateOnly(2027, 6, 30);
        var endDate = new DateOnly(2026, 9, 1);

        var action = () => AcademicYear.Create("2026-2027", 2026, startDate, endDate);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_WithSameDateForStartAndEnd_ThrowsArgumentOutOfRangeException()
    {
        var date = new DateOnly(2026, 9, 1);

        var action = () => AcademicYear.Create("2026-2027", 2026, date, date);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_WithNullOrEmptyName_ThrowsArgumentException()
    {
        var action = () => AcademicYear.Create("", 2026, new DateOnly(2026, 9, 1), new DateOnly(2027, 6, 30));

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddTerm_WithValidTerm_AddsSuccessfully()
    {
        var year = AcademicYear.Create("2026-2027", 2026, new DateOnly(2026, 9, 1), new DateOnly(2027, 6, 30));
        var term = Term.Create(year.Id, "Term 1", 1, new DateOnly(2026, 9, 1), new DateOnly(2026, 12, 15));

        year.AddTerm(term);

        year.Terms.Should().HaveCount(1);
        year.Terms[0].Should().Be(term);
        year.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AddTerm_WithNullTerm_ThrowsArgumentNullException()
    {
        var year = AcademicYear.Create("2026-2027", 2026, new DateOnly(2026, 9, 1), new DateOnly(2027, 6, 30));

        var action = () => year.AddTerm(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RemoveTerm_WithExistingTerm_RemovesSuccessfully()
    {
        var year = AcademicYear.Create("2026-2027", 2026, new DateOnly(2026, 9, 1), new DateOnly(2027, 6, 30));
        var term = Term.Create(year.Id, "Term 1", 1, new DateOnly(2026, 9, 1), new DateOnly(2026, 12, 15));
        year.AddTerm(term);

        year.RemoveTerm(term);

        year.Terms.Should().BeEmpty();
    }
}

/// <summary>
/// Tests for Term domain model invariants and operations.
/// </summary>
public sealed class TermDomainTests
{
    [Fact]
    public void Create_WithValidData_CreatesSuccessfully()
    {
        var startDate = new DateOnly(2026, 9, 1);
        var endDate = new DateOnly(2026, 12, 15);

        var term = Term.Create(1, "Fall Term", 1, startDate, endDate);

        term.Name.Should().Be("Fall Term");
        term.SequenceNumber.Should().Be(1);
        term.StartDate.Should().Be(startDate);
        term.EndDate.Should().Be(endDate);
        term.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Create_WithEndDateBeforeStartDate_ThrowsArgumentOutOfRangeException()
    {
        var action = () => Term.Create(1, "Term", 1, new DateOnly(2026, 12, 15), new DateOnly(2026, 9, 1));

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_WithInvalidSequenceNumber_ThrowsArgumentOutOfRangeException()
    {
        var action = () => Term.Create(1, "Term", 0, new DateOnly(2026, 9, 1), new DateOnly(2026, 12, 15));

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}

/// <summary>
/// Tests for Slot domain model invariants and operations.
/// </summary>
public sealed class SlotDomainTests
{
    [Fact]
    public void Create_WithValidTimeRange_CreatesSuccessfully()
    {
        var startTime = new TimeOnly(8, 0);
        var endTime = new TimeOnly(8, 45);

        var slot = Slot.Create(1, DayOfWeek.Monday, startTime, endTime, 1);

        slot.TermId.Should().Be(1);
        slot.DayOfWeek.Should().Be((int)DayOfWeek.Monday);
        slot.StartTime.Should().Be(startTime);
        slot.EndTime.Should().Be(endTime);
        slot.DurationMinutes.Should().Be(45);
        slot.SlotNumber.Should().Be(1);
    }

    [Fact]
    public void Create_WithEndTimeBeforeStartTime_ThrowsArgumentOutOfRangeException()
    {
        var action = () => Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 45), new TimeOnly(8, 0), 1);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_WithInvalidDayOfWeek_ThrowsArgumentOutOfRangeException()
    {
        var action = () => Slot.Create(1, (DayOfWeek)7, new TimeOnly(8, 0), new TimeOnly(8, 45), 1);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Create_WithInvalidSlotNumber_ThrowsArgumentOutOfRangeException()
    {
        var action = () => Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(8, 45), 0);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void GetDisplayName_ReturnsFormattedName()
    {
        var slot = Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(8, 45), 1);

        var displayName = slot.GetDisplayName();

        displayName.Should().Be("Monday 08:00–08:45");
    }

    [Fact]
    public void AddPeriod_WithValidPeriod_AddsSuccessfully()
    {
        var slot = Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(8, 45), 1);
        var section = Section.Create("9A");
        var classroom = Classroom.Create("A101", 30);
        var period = Period.Create(1, 1, 1, "Math", "teacher-1");

        slot.AddPeriod(period);

        slot.Periods.Should().HaveCount(1);
    }
}

/// <summary>
/// Tests for Period domain model invariants and operations.
/// </summary>
public sealed class PeriodDomainTests
{
    [Fact]
    public void Create_WithValidData_CreatesSuccessfully()
    {
        var period = Period.Create(1, 1, 1, "Mathematics", "teacher-1", "Prof. Smith", "Lecture");

        period.SlotId.Should().Be(1);
        period.SectionId.Should().Be(1);
        period.ClassroomId.Should().Be(1);
        period.Subject.Should().Be("Mathematics");
        period.TeacherId.Should().Be("teacher-1");
        period.TeacherName.Should().Be("Prof. Smith");
        period.Notes.Should().Be("Lecture");
        period.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithNullSubject_ThrowsArgumentException()
    {
        var action = () => Period.Create(1, 1, 1, "", "teacher-1");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithNullTeacherId_ThrowsArgumentException()
    {
        var action = () => Period.Create(1, 1, 1, "Math", "");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithInvalidSlotId_ThrowsArgumentOutOfRangeException()
    {
        var action = () => Period.Create(0, 1, 1, "Math", "teacher-1");

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Update_WithValidData_UpdatesSuccessfully()
    {
        var period = Period.Create(1, 1, 1, "Mathematics", "teacher-1", "Prof. Smith");

        period.Update("Physics", "teacher-2", "Prof. Jones", 2, "Lab session");

        period.Subject.Should().Be("Physics");
        period.TeacherId.Should().Be("teacher-2");
        period.TeacherName.Should().Be("Prof. Jones");
        period.ClassroomId.Should().Be(2);
        period.Notes.Should().Be("Lab session");
        period.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_WithNullSubject_ThrowsArgumentException()
    {
        var period = Period.Create(1, 1, 1, "Mathematics", "teacher-1");

        var action = () => period.Update("", "teacher-1");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetDisplayName_ReturnsFormattedName()
    {
        var slot = Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(8, 45), 1);
        slot.Id = 1;
        var section = Section.Create("9A");
        section.Id = 1;
        var classroom = Classroom.Create("A101", 30);
        classroom.Id = 1;

        var period = Period.Create(slot.Id, section.Id, classroom.Id, "Mathematics", "teacher-1", "Prof. Smith");
        period.Slot = slot;
        period.Section = section;

        var displayName = period.GetDisplayName();

        displayName.Should().Contain("Mathematics");
        displayName.Should().Contain("Prof. Smith");
        displayName.Should().Contain("9A");
    }
}

/// <summary>
/// Tests for Section domain model invariants and operations.
/// </summary>
public sealed class SectionDomainTests
{
    [Fact]
    public void Create_WithValidData_CreatesSuccessfully()
    {
        var section = Section.Create("9A", "Science Stream", "9", 35, "teacher-class-9a");

        section.Name.Should().Be("9A");
        section.Description.Should().Be("Science Stream");
        section.Grade.Should().Be("9");
        section.StudentCount.Should().Be(35);
        section.ClassTeacherId.Should().Be("teacher-class-9a");
        section.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithNullName_ThrowsArgumentException()
    {
        var action = () => Section.Create("");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithOnlyRequiredParameter_CreatesSuccessfully()
    {
        var section = Section.Create("10B");

        section.Name.Should().Be("10B");
        section.Description.Should().BeNull();
        section.Grade.Should().BeNull();
        section.StudentCount.Should().BeNull();
        section.ClassTeacherId.Should().BeNull();
    }
}

/// <summary>
/// Tests for Classroom domain model invariants and operations.
/// </summary>
public sealed class ClassroomDomainTests
{
    [Fact]
    public void Create_WithValidData_CreatesSuccessfully()
    {
        var classroom = Classroom.Create("A101", "Main Building", 30, "Projector, Whiteboard");

        classroom.Name.Should().Be("A101");
        classroom.Description.Should().Be("Main Building");
        classroom.Capacity.Should().Be(30);
        classroom.Facilities.Should().Be("Projector, Whiteboard");
        classroom.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithNullName_ThrowsArgumentException()
    {
        var action = () => Classroom.Create("");

        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithOnlyRequiredParameter_CreatesSuccessfully()
    {
        var classroom = Classroom.Create("Lab-01");

        classroom.Name.Should().Be("Lab-01");
        classroom.Description.Should().BeNull();
        classroom.Capacity.Should().BeNull();
        classroom.Facilities.Should().BeNull();
    }

    [Fact]
    public void AddPeriod_WithValidPeriod_AddsSuccessfully()
    {
        var classroom = Classroom.Create("A101", capacity: 30);
        var slot = Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(8, 45), 1);
        slot.Id = 1;
        var section = Section.Create("9A");
        section.Id = 1;

        var period = Period.Create(slot.Id, section.Id, 1, "Math", "teacher-1");
        classroom.AddPeriod(period);

        classroom.Periods.Should().HaveCount(1);
    }
}
