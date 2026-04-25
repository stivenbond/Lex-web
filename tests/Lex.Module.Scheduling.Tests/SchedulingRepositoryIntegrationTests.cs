using FluentAssertions;
using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Persistence;
using Lex.Module.Scheduling.Tests.TestInfrastructure;
using Xunit;

namespace Lex.Module.Scheduling.Tests;

public sealed class SchedulingRepositoryIntegrationTests : IClassFixture<AppContainerFixture>, IAsyncLifetime
{
    private readonly AppContainerFixture _fixture;

    public SchedulingRepositoryIntegrationTests(AppContainerFixture fixture)
    {
        _fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var db = _fixture.CreateSchedulingDbContext();
        await db.Database.EnsureCreatedAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetScheduleForTeacherAsync_WhenDataExists_ReturnsExpectedItems()
    {
        if (!_fixture.IsAvailable)
        {
            // Keeps local/CI test runs stable when Docker is unavailable.
            return;
        }

        await using var arrangeDb = _fixture.CreateSchedulingDbContext();
        var repository = new SchedulingRepository(arrangeDb);

        var year = AcademicYear.Create("2026-2027", 2026, new DateOnly(2026, 9, 1), new DateOnly(2027, 6, 30));
        repository.AddAcademicYear(year);
        await repository.SaveChangesAsync();

        var term = Term.Create(year.Id, "Term 1", 1, new DateOnly(2026, 9, 1), new DateOnly(2026, 12, 31));
        repository.AddTerm(term);
        await repository.SaveChangesAsync();

        var slot = Slot.Create(term.Id, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(9, 45), 1);
        repository.AddSlot(slot);
        await repository.SaveChangesAsync();

        var period = Period.Create(slot.Id, sectionId: 30, classroomId: 12, subject: "Physics", teacherId: "teacher-42", teacherName: "Dr. Green");
        repository.AddPeriod(period);
        await repository.SaveChangesAsync();

        await using var assertDb = _fixture.CreateSchedulingDbContext();
        var assertRepository = new SchedulingRepository(assertDb);

        var items = await assertRepository.GetScheduleForTeacherAsync(
            "teacher-42",
            new DateOnly(2026, 9, 1),
            new DateOnly(2026, 12, 31));

        items.Should().HaveCount(1);
        var item = items.Single();
        item.TeacherId.Should().Be("teacher-42");
        item.Subject.Should().Be("Physics");
        item.ClassId.Should().Be(30);
        item.ClassroomId.Should().Be(12);
    }
}
