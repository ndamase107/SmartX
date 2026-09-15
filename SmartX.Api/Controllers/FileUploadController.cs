using Microsoft.AspNetCore.Mvc;
using SmartX.Api.Services;

namespace SmartX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly FileService _fileService;

    public FileUploadController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("{sensorId:int}")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async Task<IActionResult> Upload(
        int sensorId,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (!_fileService.IsAllowedFile(file))
        {
            return BadRequest(
                "Invalid file. Maximum size is 10 MB " +
                "and the file type must be supported.");
        }

        try
        {
            var savedPath =
                await _fileService.SaveFileAsync(
                    file,
                    sensorId,
                    cancellationToken);

            return Ok(new
            {
                message = "File uploaded successfully.",
                sensorId,
                fileName = Path.GetFileName(savedPath)
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (OperationCanceledException)
        {
            return BadRequest("Upload was cancelled.");
        }
    }
}