using Lex.SharedKernel.Primitives;

namespace Lex.Module.DiaryManagement.Core.Domain;

public sealed class DiaryAttachment(Guid fileId, string fileName) : ValueObject
{
    public Guid FileId { get; } = fileId;
    public string FileName { get; } = fileName;

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FileId;
        yield return FileName;
    }
}
