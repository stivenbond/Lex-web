using Lex.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lex.Module.ObjectStorage.Infrastructure.Controllers;

[ApiController]
[Route("api/files")]
[Authorize]
public sealed class FileController(IObjectStorageService storageService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var result = await storageService.ListFilesAsync(ct);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        var result = await storageService.UploadAsync(stream, file.FileName, file.ContentType, ct);

        if (result.IsFailure) return BadRequest(result.Error);

        return Ok(new { FileId = result.Value });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Download(Guid id, CancellationToken ct)
    {
        var result = await storageService.DownloadAsync(id, ct);
        if (result.IsFailure) return NotFound(result.Error);

        var filesResult = await storageService.ListFilesAsync(ct);
        var metadata = filesResult.Value?.FirstOrDefault(x => x.Id == id);
        var contentType = metadata?.ContentType ?? "application/octet-stream";
        var fileName = metadata?.FileName ?? "file";

        return File(result.Value!, contentType, fileName, true);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await storageService.DeleteAsync(id, ct);
        if (result.IsFailure) return BadRequest(result.Error);

        return NoContent();
    }
}
