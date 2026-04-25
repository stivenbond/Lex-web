using Lex.Module.DiaryManagement.Core.Domain;
using Lex.SharedKernel.Primitives;
using MediatR;

namespace Lex.Module.DiaryManagement.Core.Features.CreateDiaryEntry;

public record CreateDiaryEntryCommand(string Title, string Content, DateTime Date, List<Guid>? AttachmentIds = null) : IRequest<Result<Guid>>;

internal sealed class CreateDiaryEntryHandler(IDiaryRepository repository) 
    : IRequestHandler<CreateDiaryEntryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateDiaryEntryCommand request, CancellationToken ct)
    {
        var entry = DiaryEntry.Create(request.Title, request.Content, request.Date);

        if (request.AttachmentIds != null)
        {
            foreach (var id in request.AttachmentIds)
            {
                // In a real scenario, we'd fetch the file name from ObjectStorage 
                // or have it passed in the command. For now we use a placeholder.
                entry.AddAttachment(id, "Attached File");
            }
        }

        repository.AddEntry(entry);
        await repository.SaveChangesAsync(ct);

        return entry.Id;
    }
}
