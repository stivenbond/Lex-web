namespace Lex.Module.Scheduling.Core.Domain;

/// <summary>
/// Represents a section (class/group) of students.
/// Example: "Class 9A", "10B-Science", "UG-Semester1".
/// </summary>
public class Section
{
    private List<Period> _periods = [];

    public int Id { get; set; }

    /// <summary>
    /// Display name/identifier for the section (e.g., "9A", "10B-Science").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the section (e.g., "Grade 9, Science stream").
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Academic level/grade (e.g., "9", "10", "11", "12").
    /// </summary>
    public string? Grade { get; set; }

    /// <summary>
    /// Estimated or actual student count in this section.
    /// </summary>
    public int? StudentCount { get; set; }

    /// <summary>
    /// Name or ID of the class teacher/head teacher for this section.
    /// </summary>
    public string? ClassTeacherId { get; set; }

    /// <summary>
    /// Whether this section is active and accepting new periods.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property: all periods scheduled for this section.
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

    public Section() { }

    /// <summary>
    /// Creates a new section.
    /// </summary>
    public static Section Create(string name, string? description = null, string? grade = null, int? studentCount = null, string? classTeacherId = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Section
        {
            Name = name,
            Description = description,
            Grade = grade,
            StudentCount = studentCount,
            ClassTeacherId = classTeacherId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Adds a period to this section.
    /// </summary>
    public void AddPeriod(Period period)
    {
        ArgumentNullException.ThrowIfNull(period);
        _periods.Add(period);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a period from this section.
    /// </summary>
    public void RemovePeriod(Period period)
    {
        ArgumentNullException.ThrowIfNull(period);
        _periods.Remove(period);
        UpdatedAt = DateTime.UtcNow;
    }
}
