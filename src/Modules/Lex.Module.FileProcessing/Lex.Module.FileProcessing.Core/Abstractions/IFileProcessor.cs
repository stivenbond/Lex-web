using Lex.SharedKernel.Primitives;

namespace Lex.Module.FileProcessing.Core.Abstractions;

public interface IFileProcessor
{
    string SupportedExtension { get; }
    Task<Result<string>> ExtractTextAsync(Stream data, CancellationToken ct = default);
}
