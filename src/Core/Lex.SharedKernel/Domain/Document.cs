using Lex.SharedKernel.Domain.Abstractions;
public class Document
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<IContentBlock<TData>> Blocks { get; init; }
}
