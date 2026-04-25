using FluentAssertions;
using Lex.Module.Scheduling.Core.Domain;
using Lex.Module.Scheduling.Core.Features.CreateAcademicYear;
using Lex.Module.Scheduling.Core.Features.SchedulingEvents;
using Moq;
using Xunit;

namespace Lex.Module.Scheduling.Tests;

public sealed class CreateAcademicYearHandlerTests
{
    [Fact]
    public async Task Handle_WhenTermsOverlap_ReturnsConflictAndDoesNotPersist()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var publisher = new Mock<ISchedulingEventPublisher>(MockBehavior.Strict);
        var sut = new CreateAcademicYearHandler(repository.Object, publisher.Object);

        var command = new CreateAcademicYearCommand(
            "2026-2027",
            2026,
            new DateOnly(2026, 9, 1),
            new DateOnly(2027, 6, 30),
            [
                new TermInput("Term 1", new DateOnly(2026, 9, 1), new DateOnly(2026, 12, 15)),
                new TermInput("Term 2", new DateOnly(2026, 12, 1), new DateOnly(2027, 3, 31))
            ]);

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error!.Code.Should().Be("Scheduling.Term.Overlap");
        repository.Verify(x => x.AddAcademicYear(It.IsAny<AcademicYear>()), Times.Never);
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        publisher.Verify(
            x => x.PublishAcademicYearCreatedAsync(It.IsAny<AcademicYearCreatedEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidTerms_PersistsAndPublishesEvent()
    {
        var repository = new Mock<ISchedulingRepository>(MockBehavior.Strict);
        var publisher = new Mock<ISchedulingEventPublisher>(MockBehavior.Strict);
        var sut = new CreateAcademicYearHandler(repository.Object, publisher.Object);

        AcademicYear? capturedYear = null;
        repository.Setup(x => x.AddAcademicYear(It.IsAny<AcademicYear>()))
            .Callback<AcademicYear>(y =>
            {
                y.Id = 42;
                capturedYear = y;
            });
        repository.Setup(x => x.AddTerm(It.IsAny<Term>()));
        repository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        publisher.Setup(x => x.PublishAcademicYearCreatedAsync(
                It.Is<AcademicYearCreatedEvent>(e => e.YearId == 42 && e.Name == "2026-2027"),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new CreateAcademicYearCommand(
            "2026-2027",
            2026,
            new DateOnly(2026, 9, 1),
            new DateOnly(2027, 6, 30),
            [
                new TermInput("Term 1", new DateOnly(2026, 9, 1), new DateOnly(2026, 12, 15)),
                new TermInput("Term 2", new DateOnly(2027, 1, 5), new DateOnly(2027, 3, 31))
            ]);

        var result = await sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        capturedYear.Should().NotBeNull();
        repository.Verify(x => x.AddAcademicYear(It.IsAny<AcademicYear>()), Times.Once);
        repository.Verify(x => x.AddTerm(It.IsAny<Term>()), Times.Exactly(2));
        repository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        publisher.Verify(
            x => x.PublishAcademicYearCreatedAsync(It.IsAny<AcademicYearCreatedEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
