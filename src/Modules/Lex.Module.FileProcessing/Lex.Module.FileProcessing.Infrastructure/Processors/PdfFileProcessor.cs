using Lex.SharedKernel.Primitives;
using Lex.Module.FileProcessing.Core.Abstractions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace Lex.Module.FileProcessing.Infrastructure.Processors;

public sealed class PdfFileProcessor : IFileProcessor
{
    public string SupportedExtension => ".pdf";

    public async Task<Result<string>> ExtractTextAsync(Stream data, CancellationToken ct = default)
    {
        try
        {
            return await Task.Run(() =>
            {
                using var reader = new PdfReader(data);
                using var pdfDoc = new PdfDocument(reader);
                var text = new System.Text.StringBuilder();

                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var page = pdfDoc.GetPage(i);
                    text.Append(PdfTextExtractor.GetTextFromPage(page, strategy));
                }

                return Result<string>.Success(text.ToString());
            }, ct);
        }
        catch (Exception ex)
        {
            return Error.Failure("PdfExtractionFailed", ex.Message);
        }
    }
}
