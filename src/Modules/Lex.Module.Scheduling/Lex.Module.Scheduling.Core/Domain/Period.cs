namespace Lex.Module.Scheduling.Core.Domain;

/// <summary>
/// Represents an actual teaching period (lesson/class session).
/// A period is the concrete assignment of: a teacher, a section (class), a subject, and a classroom
/// within a specific time slot.
/// Example: "Ms. Johnson teaches Biology to Class 9A in Room A101 during Monday 08:00–08:45"
/// 
/// Key constraint: Every period MUST belong to exactly one slot and one section.
/// Every period MUST reference a classroom where it takes place.
/// Not every slot has a period assigned to it (some slots may be free/unassigned).
/// </summary>
public class Period
{
    public int Id { get; set; }

    public int SlotId { get; set; }

    /// <summary>
    /// Navigation property: the time slot this period belongs to.
    /// </summary>
    public Slot Slot { get; set; } = null!;

    public int SectionId { get; set; }

    /// <summary>
    /// Navigation property: the student section (class) this period is for.
    /// </summary>
    public Section Section { get; set; } = null!;

    public int ClassroomId { get; set; }

    /// <summary>
    /// Navigation property: the classroom where this period is taught.
    /// </summary>
    public Classroom Classroom { get; set; } = null!;

    /// <summary>
    /// Name/code of the subject being taught (e.g., "Biology", "Mathematics", "English").
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// ID of the teacher assigned to teach this period.
    /// Reference to a user in the identity system (typically from Keycloak or similar).
    /// </summary>
    public required string TeacherId { get; set; }

    /// <summary>
    /// Optional display name for the teacher (cached for performance/readability).
    /// </summary>
    public string? TeacherName { get; set; }

    /// <summary>
    /// Whether this period is currently active and being used.
    /// Allows soft-deletion or temporary suspension of a period.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Optional notes or remarks about the period
    /// (e.g., "Lab session", "Practical assessment", "Guest lecturer").
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// UTC timestamp when this was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UTC timestamp when this was last modified.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public Period() { }

    /// <summary>
    /// Creates a new period.
    /// </summary>
    public static Period Create(
        int slotId,
        int sectionId,
        int classroomId,
        string subject,
        string teacherId,
        string? teacherName = null,
        string? notes = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        ArgumentException.ThrowIfNullOrWhiteSpace(teacherId);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(slotId, 0, nameof(slotId));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(sectionId, 0, nameof(sectionId));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(classroomId, 0, nameof(classroomId));

        return new Period
        {
            SlotId = slotId,
            SectionId = sectionId,
            ClassroomId = classroomId,
            Subject = subject,
            TeacherId = teacherId,
            TeacherName = teacherName,
            IsActive = true,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Updates the period's details (subject, teacher, classroom, or notes).
    /// </summary>
    public void Update(string subject, string teacherId, string? teacherName = null, int? classroomId = null, string? notes = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        ArgumentException.ThrowIfNullOrWhiteSpace(teacherId);

        Subject = subject;
        TeacherId = teacherId;
        TeacherName = teacherName;

        if (classroomId.HasValue && classroomId > 0)
        {
            ClassroomId = classroomId.Value;
        }

        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets a human-readable description of this period.
    /// Example: "Biology with Ms. Johnson - Class 9A in Room A101 (Monday 08:00–08:45)"
    /// </summary>
    public string GetDisplayName()
    {
        var teacherPart = !string.IsNullOrWhiteSpace(TeacherName) ? $" with {TeacherName}" : string.Empty;
        return $"{Subject}{teacherPart} - {Section.Name} (Slot {Slot.SlotNumber})";
    }
}
