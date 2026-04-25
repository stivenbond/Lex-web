namespace Lex.SharedKernel.Domain;

public class Document
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Lex.SharedKernel.Domain.Abstractions.IContentBlock> Blocks { get; init; } = [];
}
