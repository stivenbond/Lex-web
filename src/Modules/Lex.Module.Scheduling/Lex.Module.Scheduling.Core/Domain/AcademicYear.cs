namespace Lex.Module.Scheduling.Core.Domain;

/// <summary>
/// Represents a school academic year (e.g., 2024-2025).
/// Serves as the parent container for terms and all scheduled activities.
/// </summary>
public class AcademicYear
{
    private List<Term> _terms = [];

    public int Id { get; set; }
    
    /// <summary>
    /// Display name (e.g., "2024-2025", "Class of 2024").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The calendar year when this academic year begins.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Start date of the academic year.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the academic year.
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Whether this academic year is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Navigation property: all terms within this academic year.
    /// </summary>
    public IReadOnlyList<Term> Terms => _terms.AsReadOnly();

    /// <summary>
    /// UTC timestamp when this was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UTC timestamp when this was last modified.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public AcademicYear() { }

    /// <summary>
    /// Creates a new academic year.
    /// </summary>
    public static AcademicYear Create(string name, int year, DateOnly startDate, DateOnly endDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(endDate.DayNumber, startDate.DayNumber, nameof(endDate));

        return new AcademicYear
        {
            Name = name,
            Year = year,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Adds a term to this academic year.
    /// </summary>
    public void AddTerm(Term term)
    {
        ArgumentNullException.ThrowIfNull(term);
        _terms.Add(term);
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes a term from this academic year.
    /// </summary>
    public void RemoveTerm(Term term)
    {
        ArgumentNullException.ThrowIfNull(term);
        _terms.Remove(term);
        UpdatedAt = DateTime.UtcNow;
    }
}
