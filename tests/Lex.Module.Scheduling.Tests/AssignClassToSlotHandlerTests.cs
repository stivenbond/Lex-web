using FluentAssertions;
using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.AssignClassToSlot;
using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using Moq;
using Xunit;

namespace Lex.Module.Scheduling.Tests;

public sealed class AssignClassToSlotHandlerTests
{
    [Fact]
    public async Task Handle_WhenConflictExists_ReturnsConflictAndSkipsPublish()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var publisher = new Mock<ISchedulingEventPublisher>(MockBehavior.Strict);
        var sut = new AssignClassToSlotHandler(repository.Object, publisher.Object);

        repository.Setup(x => x.GetSlotByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateSlot(10));
        repository.Setup(x => x.HasConflictingPeriodAsync(10, 7, 4, "teacher-1", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new AssignClassToSlotCommand(
            SlotId: 10,
            ClassId: 7,
            TeacherId: "teacher-1",
            ClassroomId: 4,
            Subject: "Mathematics");

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Scheduling.Slot.Conflict");
        repository.Verify(x => x.AddPeriod(It.IsAny<Period>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        publisher.Verify(x => x.PublishSlotAssignedAsync(It.IsAny<SlotAssignedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNoConflict_CreatesPeriodAndPublishesEvent()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var publisher = new Mock<ISchedulingEventPublisher>(MockBehavior.Strict);
        var sut = new AssignClassToSlotHandler(repository.Object, publisher.Object);

        var slot = CreateSlot(12);
        Period? capturedPeriod = null;

        repository.Setup(x => x.GetSlotByIdAsync(12, It.IsAny<CancellationToken>()))
            .ReturnsAsync(slot);
        repository.Setup(x => x.HasConflictingPeriodAsync(12, 8, 3, "teacher-2", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        repository.Setup(x => x.AddPeriod(It.IsAny<Period>()))
            .Callback<Period>(p =>
            {
                p.Id = 99;
                capturedPeriod = p;
            });
        repository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        publisher.Setup(x => x.PublishSlotAssignedAsync(
                It.Is<SlotAssignedEvent>(e => e.SlotId == 12 && e.PeriodId == 99 && e.ClassId == 8 && e.TeacherId == "teacher-2"),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await sut.Handle(
            new AssignClassToSlotCommand(
                SlotId: 12,
                ClassId: 8,
                TeacherId: "teacher-2",
                ClassroomId: 3,
                Subject: "Chemistry",
                TeacherName: "Prof. Adams"),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(99);
        capturedPeriod.Should().NotBeNull();
        capturedPeriod!.Subject.Should().Be("Chemistry");
        repository.Verify(x => x.AddPeriod(It.IsAny<Period>()), Times.Once);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        publisher.Verify(x => x.PublishSlotAssignedAsync(It.IsAny<SlotAssignedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Slot CreateSlot(int id)
    {
        var slot = Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(8, 45), 1);
        slot.Id = id;
        slot.Term = Term.Create(1, "Term 1", 1, new DateOnly(2026, 9, 1), new DateOnly(2027, 6, 30));
        return slot;
    }
}
