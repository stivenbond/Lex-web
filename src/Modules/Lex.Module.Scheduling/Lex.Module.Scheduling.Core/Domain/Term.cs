namespace Lex.Module.Scheduling.Core.Domain;

/// <summary>
/// Represents a term (e.g., Fall, Winter, Spring) within an academic year.
/// Contains scheduled periods/slots for that term.
/// </summary>
public class Term
{
    private List<Slot> _slots = [];

    public int Id { get; set; }

    public int AcademicYearId { get; set; }

    /// <summary>
    /// Navigation property: the academic year this term belongs to.
    /// </summary>
    public AcademicYear AcademicYear { get; set; } = null!;

    /// <summary>
    /// Display name (e.g., "Fall 2024", "Spring 2025").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Display order within the academic year (e.g., 1 for Fall, 2 for Winter).
    /// </summary>
    public int SequenceNumber { get; set; }

    /// <summary>
    /// Start date of the term.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the term.
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Whether this term is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Navigation property: all slots (time periods) in this term.
    /// </summary>
    public IReadOnlyList<Slot> Slots => _slots.AsReadOnly();

    /// <summary>
    /// UTC timestamp when this was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UTC timestamp when this was last modified.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public Term() { }

    /// <summary>
    /// Creates a new term.
    /// </summary>
    public static Term Create(int academicYearId, string name, int sequenceNumber, DateOnly startDate, DateOnly endDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(endDate.DayNumber, startDate.DayNumber, nameof(endDate));
        ArgumentOutOfRangeException.ThrowIfLessThan(sequenceNumber, 1, nameof(sequenceNumber));

        return new Term
        {
            AcademicYearId = academicYearId,
            Name = name,
            SequenceNumber = sequenceNumber,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Adds a slot to this term.
    /// </summary>
    public void AddSlot(Slot slot)
    {
        ArgumentNullException.ThrowIfNull(slot);
        _slots.Add(slot);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a slot from this term.
    /// </summary>
    public void RemoveSlot(Slot slot)
    {
        ArgumentNullException.ThrowIfNull(slot);
        _slots.Remove(slot);
        UpdatedAt = DateTime.UtcNow;
    }
}
