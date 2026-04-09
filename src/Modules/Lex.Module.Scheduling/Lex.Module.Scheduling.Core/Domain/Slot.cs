namespace Lex.Module.Scheduling.Core.Domain;

/// <summary>
/// Represents a recurring time slot available for teaching.
/// Example: "Monday 08:00–08:45", "Tuesday 08:50–09:35", etc.
/// A slot is a time availability; it may or may not have an actual period assigned to it.
/// </summary>
public class Slot
{
    private List<Period> _periods = [];

    public int Id { get; set; }

    public int TermId { get; set; }

    /// <summary>
    /// Navigation property: the term this slot belongs to.
    /// </summary>
    public Term Term { get; set; } = null!;

    /// <summary>
    /// Day of the week (0 = Sunday, 1 = Monday, ..., 6 = Saturday).
    /// </summary>
    public int DayOfWeek { get; set; }

    /// <summary>
    /// Start time of the slot (e.g., 08:00 for 8 AM).
    /// Stored as TimeOnly (HH:mm format).
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// End time of the slot (e.g., 08:45 for 8:45 AM).
    /// Stored as TimeOnly (HH:mm format).
    /// </summary>
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// Duration in minutes (calculated as EndTime - StartTime).
    /// For a school with 45-minute periods, this should be 45.
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// Sequential slot number within the day (1, 2, 3, etc.).
    /// Helps with UI ordering and readability.
    /// </summary>
    public int SlotNumber { get; set; }

    /// <summary>
    /// Navigation property: all periods assigned to this slot.
    /// Most slots will have zero or one period, but the relationship allows flexibility.
    /// </summary>
    public IReadOnlyList<Period> Periods => _periods.AsReadOnly();

    /// <summary>
    /// UTC timestamp when this was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UTC timestamp when this was last modified.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public Slot() { }

    /// <summary>
    /// Creates a new slot.
    /// </summary>
    public static Slot Create(int termId, DayOfWeek dayOfWeek, TimeOnly startTime, TimeOnly endTime, int slotNumber)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan((int)dayOfWeek, 6, nameof(dayOfWeek));
        ArgumentOutOfRangeException.ThrowIfLessThan((int)dayOfWeek, 0, nameof(dayOfWeek));
        ArgumentOutOfRangeException.ThrowIfLessThan(slotNumber, 1, nameof(slotNumber));

        var durationMinutes = (int)(endTime - startTime).TotalMinutes;
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(durationMinutes, 0, nameof(endTime));

        return new Slot
        {
            TermId = termId,
            DayOfWeek = (int)dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            DurationMinutes = durationMinutes,
            SlotNumber = slotNumber,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Adds a period to this slot.
    /// </summary>
    public void AddPeriod(Period period)
    {
        ArgumentNullException.ThrowIfNull(period);
        _periods.Add(period);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a period from this slot.
    /// </summary>
    public void RemovePeriod(Period period)
    {
        ArgumentNullException.ThrowIfNull(period);
        _periods.Remove(period);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets a human-readable name for this slot (e.g., "Monday 08:00–08:45").
    /// </summary>
    public string GetDisplayName()
    {
        var dayName = (DayOfWeek)DayOfWeek switch
        {
            System.DayOfWeek.Sunday => "Sunday",
            System.DayOfWeek.Monday => "Monday",
            System.DayOfWeek.Tuesday => "Tuesday",
            System.DayOfWeek.Wednesday => "Wednesday",
            System.DayOfWeek.Thursday => "Thursday",
            System.DayOfWeek.Friday => "Friday",
            System.DayOfWeek.Saturday => "Saturday",
            _ => "Unknown"
        };

        return $"{dayName} {StartTime:HH:mm}–{EndTime:HH:mm}";
    }
}
