namespace Lex.Module.Scheduling.Core.Domain;

/// <summary>
/// Represents a physical classroom location where periods are taught.
/// </summary>
public class Classroom
{
    private List<Period> _periods = [];

    public int Id { get; set; }

    /// <summary>
    /// Display name/identifier for the classroom (e.g., "A101", "Lab 2", "Gym").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the classroom (e.g., location, capacity, equipment).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Room capacity (number of students).
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Comma-separated list of facilities (e.g., "Projector, Whiteboard, Computer Lab").
    /// </summary>
    public string? Facilities { get; set; }

    /// <summary>
    /// Whether this classroom is active and available for scheduling.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property: all periods scheduled in this classroom.
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

    public Classroom() { }

    /// <summary>
    /// Creates a new classroom.
    /// </summary>
    public static Classroom Create(string name, string? description = null, int? capacity = null, string? facilities = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Classroom
        {
            Name = name,
            Description = description,
            Capacity = capacity,
            Facilities = facilities,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Adds a period to this classroom.
    /// </summary>
    public void AddPeriod(Period period)
    {
        ArgumentNullException.ThrowIfNull(period);
        _periods.Add(period);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a period from this classroom.
    /// </summary>
    public void RemovePeriod(Period period)
    {
        ArgumentNullException.ThrowIfNull(period);
        _periods.Remove(period);
        UpdatedAt = DateTime.UtcNow;
    }
}
