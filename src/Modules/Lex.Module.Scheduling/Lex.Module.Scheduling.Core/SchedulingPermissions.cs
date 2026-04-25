namespace Lex.Module.Scheduling;
public static class SchedulingPermissions
{
    private const string Prefix = "scheduling";

    // Spec-aligned policy names
    public const string ManageCalendar = $"{Prefix}.manage-calendar";
    public const string AssignSlots = $"{Prefix}.assign-slots";
    public const string ViewSchedule = $"{Prefix}.view-schedule";

    // Backward-compatible aliases for any existing callers
    public const string View = ViewSchedule;
    public const string Create = ManageCalendar;
    public const string Edit = AssignSlots;
    public const string Delete = AssignSlots;
}
