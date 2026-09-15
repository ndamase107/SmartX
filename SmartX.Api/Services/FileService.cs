using Microsoft.AspNetCore.Http;

namespace SmartX.Api.Services;

public class FileService
{
    private readonly IWebHostEnvironment _environment;

    private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    private static readonly string[] AllowedExtensions =
    [
        ".txt",
        ".log",
        ".json",
        ".csv",
        ".xml",
        ".jpg",
        ".jpeg",
        ".png",
        ".pdf"
    ];

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(
        IFormFile file,
        int sensorId,
        CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file was provided.");

        if (file.Length > MaxFileSize)
            throw new ArgumentException("File size cannot exceed 10 MB.");

        var extension = Path.GetExtension(file.FileName)
            .ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException(
                $"File type '{extension}' is not supported.");

        var uploadsFolder = Path.Combine(
            _environment.ContentRootPath,
            "Uploads");

        Directory.CreateDirectory(uploadsFolder);

        /*
         * Never trust the original filename.
         * A new GUID prevents collisions and unsafe filenames.
         */
        var safeFileName =
            $"{Guid.NewGuid():N}{extension}";

        var sensorFolder = Path.Combine(
            uploadsFolder,
            $"Sensor-{sensorId}");

        Directory.CreateDirectory(sensorFolder);

        var filePath = Path.Combine(
            sensorFolder,
            safeFileName);

        await using var stream = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);

        await file.CopyToAsync(
            stream,
            cancellationToken);

        return filePath;
    }

    public bool IsAllowedFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        if (file.Length > MaxFileSize)
            return false;

        var extension = Path.GetExtension(file.FileName)
            .ToLowerInvariant();

        return AllowedExtensions.Contains(extension);
    }
}