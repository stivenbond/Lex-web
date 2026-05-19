using FluentAssertions;
using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.RemoveSlot;
using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using Lex.Module.Scheduling.Core.Features.UpdateSlot;
using Moq;
using Xunit;

namespace Lex.Module.Scheduling.Tests;

/// <summary>
/// Tests for the RemoveSlot command handler.
/// </summary>
public sealed class RemoveSlotHandlerTests
{
    [Fact]
    public async Task Handle_WhenSlotExists_RemovesPeriodAndPublishesEvent()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var publisher = new Mock<ISchedulingEventPublisher>(MockBehavior.Strict);
        var sut = new RemoveSlotHandler(repository.Object, publisher.Object);

        var period = CreatePeriod(15, 7, "teacher-1", 4);
        repository.Setup(x => x.GetPeriodByIdAsync(15, It.IsAny<CancellationToken>()))
            .ReturnsAsync(period);
        repository.Setup(x => x.RemovePeriod(period));
        repository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        publisher.Setup(x => x.PublishSlotRemovedAsync(
                It.Is<SlotRemovedEvent>(e => e.SlotId == 10 && e.ClassId == 7 && e.TeacherId == "teacher-1"),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new RemoveSlotCommand(SlotAssignmentId: 15);
        var result = await sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        repository.Verify(x => x.RemovePeriod(period), Times.Once);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        publisher.Verify(x => x.PublishSlotRemovedAsync(It.IsAny<SlotRemovedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSlotNotFound_ReturnsNotFound()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var publisher = new Mock<ISchedulingEventPublisher>(MockBehavior.Strict);
        var sut = new RemoveSlotHandler(repository.Object, publisher.Object);

        repository.Setup(x => x.GetPeriodByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Period?)null);

        var command = new RemoveSlotCommand(SlotAssignmentId: 999);
        var result = await sut.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Scheduling.Period.NotFound");
        repository.Verify(x => x.RemovePeriod(It.IsAny<Period>()), Times.Never);
        publisher.Verify(x => x.PublishSlotRemovedAsync(It.IsAny<SlotRemovedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    private static Period CreatePeriod(int periodId, int classId, string teacherId, int classroomId)
    {
        var slot = Slot.Create(1, DayOfWeek.Monday, new TimeOnly(8, 0), new TimeOnly(8, 45), 1);
        slot.Id = 10;
        var section = Section.Create("Class", "Test");
        section.Id = classId;
        var classroom = Classroom.Create("Room", capacity: 30);
        classroom.Id = classroomId;

        var period = Period.Create(slot.Id, section.Id, classroom.Id, "Math", teacherId);
        period.Id = periodId;
        period.Slot = slot;
        period.Section = section;
        period.Classroom = classroom;
        return period;
    }
}

/// <summary>
/// Tests for the UpdateSlot command handler.
/// </summary>
public sealed class UpdateSlotHandlerTests
{
    [Fact]
    public async Task Handle_WhenPeriodExists_UpdatesAndPublishesEvent()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var sut = new UpdateSlotHandler(repository.Object);

        var period = CreatePeriod(20, 5, "teacher-old", 3, "OldSubject");
        repository.Setup(x => x.GetPeriodByIdAsync(20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(period);
        repository.Setup(x => x.HasConflictingPeriodAsync(
                period.SlotId,
                5,
                3,
                "teacher-new",
                20,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        repository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var command = new UpdateSlotCommand(
            SlotAssignmentId: 20,
            ClassId: 5,
            TeacherId: "teacher-new",
            ClassroomId: 3,
            Subject: "NewSubject",
            TeacherName: "Prof. New");

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        period.Subject.Should().Be("NewSubject");
        period.TeacherId.Should().Be("teacher-new");
        period.TeacherName.Should().Be("Prof. New");
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenConflictExists_ReturnsConflictAndSkipsUpdate()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var sut = new UpdateSlotHandler(repository.Object);

        var period = CreatePeriod(21, 6, "teacher-1", 4, "Math");
        repository.Setup(x => x.GetPeriodByIdAsync(21, It.IsAny<CancellationToken>()))
            .ReturnsAsync(period);
        repository.Setup(x => x.HasConflictingPeriodAsync(
                period.SlotId,
                6,
                4,
                "teacher-conflict",
                21,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new UpdateSlotCommand(
            SlotAssignmentId: 21,
            ClassId: 6,
            TeacherId: "teacher-conflict",
            ClassroomId: 4,
            Subject: "Physics");

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Scheduling.Slot.Conflict");
        period.Subject.Should().Be("Math"); // Should not be updated
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenPeriodNotFound_ReturnsNotFound()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var sut = new UpdateSlotHandler(repository.Object);

        repository.Setup(x => x.GetPeriodByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Period?)null);

        var command = new UpdateSlotCommand(
            SlotAssignmentId: 999,
            ClassId: 5,
            TeacherId: "teacher",
            ClassroomId: 1,
            Subject: "Math");

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Scheduling.Period.NotFound");
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static Period CreatePeriod(int periodId, int classId, string teacherId, int classroomId, string subject)
    {
        var slot = Slot.Create(1, DayOfWeek.Tuesday, new TimeOnly(9, 0), new TimeOnly(9, 45), 2);
        slot.Id = 11;
        var section = Section.Create("Class-" + classId);
        section.Id = classId;
        var classroom = Classroom.Create("Room-" + classroomId, capacity: 30);
        classroom.Id = classroomId;

        var period = Period.Create(slot.Id, section.Id, classroom.Id, subject, teacherId);
        period.Id = periodId;
        period.Slot = slot;
        period.Section = section;
        period.Classroom = classroom;
        return period;
    }
}
