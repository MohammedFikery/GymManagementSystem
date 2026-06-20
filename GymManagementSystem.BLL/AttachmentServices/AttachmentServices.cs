using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem.BLL.AttachmentServices
{
    public sealed class AttachmentServices : IAttachmentServices
    {
        #region Fields & Constants

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase){".jpg", ".jpeg", ".png", ".gif", ".pdf", ".jfif"};
        private static readonly IContentTypeProvider ContentTypeProvider =  new FileExtensionContentTypeProvider();
        private readonly ILogger<AttachmentServices> _logger;
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructor

        public AttachmentServices(ILogger<AttachmentServices> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        #endregion

        #region Upload

        public async Task<UploadResult> UploadAsync(
            Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead)
                return UploadResult.Failure("File stream is not readable.");

            if (string.IsNullOrWhiteSpace(fileName))
                return UploadResult.Failure("File name is required.");

            if (string.IsNullOrWhiteSpace(folderName))
                return UploadResult.Failure("Folder name is required.");

            if (fileStream.Length == 0)
                return UploadResult.Failure("File is empty.");

            if (fileStream.Length > MaxFileSize)
            {
                _logger.LogWarning("Rejected file {FileName}: size {Size} exceeds limit {Limit}.",fileName, fileStream.Length, MaxFileSize);
                return UploadResult.Failure($"File size exceeds {MaxFileSize} bytes.");
            }

            // 3) فحص الامتداد (normalized)
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
            {
                _logger.LogWarning("Rejected file {FileName}: invalid extension {Ext}.",fileName, extension);
                return UploadResult.Failure($"Invalid file extension: {extension}");
            }

            // 4) إنشاء المجلد لو مش موجود
            var uploadsFolder = Path.GetFullPath(Path.Combine(_env.ContentRootPath, folderName));
            Directory.CreateDirectory(uploadsFolder);

            // 5) اسم فريد آمن: Guid + extension (بلا الاسم الأصلي)
            var storedFileName = $"{Guid.NewGuid():N}{extension}";
            var fullFilePath = Path.GetFullPath(Path.Combine(uploadsFolder, storedFileName));

            // 6) حماية Path Traversal
            if (!fullFilePath.StartsWith(uploadsFolder, StringComparison.Ordinal))
            {
                _logger.LogError("Path traversal attempt detected for {FileName}.", fileName);
                return UploadResult.Failure("Invalid file path.");
            }

            // 7) حفظ الملف
            try
            {
                await using var target = new FileStream(fullFilePath, FileMode.Create, FileAccess.Write, FileShare.None,bufferSize: 81920, useAsync: true);
                await fileStream.CopyToAsync(target, ct);
                _logger.LogInformation("File {Stored} saved successfully.", storedFileName);
                return UploadResult.Success(storedFileName);
            }
            catch (OperationCanceledException)
            {
                TryDeletePartialFile(fullFilePath);
                _logger.LogWarning("Upload cancelled for {FileName}.", fileName);
                return UploadResult.Failure("Upload was cancelled.");
            }
            catch (Exception ex)
            {
                TryDeletePartialFile(fullFilePath);
                _logger.LogError(ex, "Error saving file {FileName}.", fileName);
                return UploadResult.Failure("An error occurred while saving the file.");
            }
        }

        #endregion

        #region Delete

        public Task<bool> DeleteAsync(
            string storedFileName, string folderName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(storedFileName) || string.IsNullOrWhiteSpace(folderName))
                return Task.FromResult(false);

            try
            {
                var safeFileName = Path.GetFileName(storedFileName);
                if (safeFileName != storedFileName)
                    return Task.FromResult(false);

                var fullFolder = Path.GetFullPath(Path.Combine(_env.ContentRootPath, folderName));
                var fullPath = Path.GetFullPath(Path.Combine(fullFolder, safeFileName));

                if (!fullPath.StartsWith(fullFolder, StringComparison.Ordinal))
                    return Task.FromResult(false);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("Deleted file {File}.", storedFileName);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete file {File}.", storedFileName);
                return Task.FromResult(false);
            }
        }

        #endregion

        #region Get File

        public (Stream stream, string contentType)? GetFile(string storedFileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(storedFileName) || string.IsNullOrWhiteSpace(folderName))
                return null;

            // 1) تنظيف الاسم
            var safeFileName = Path.GetFileName(storedFileName);
            if (safeFileName != storedFileName)
                return null;

            // 2) بناء المسار + حماية Path Traversal
            var fullFolder = Path.GetFullPath(Path.Combine(_env.ContentRootPath, folderName));
            var fullPath = Path.GetFullPath(Path.Combine(fullFolder, safeFileName));

            if (!fullPath.StartsWith(fullFolder, StringComparison.Ordinal))
            {
                _logger.LogError("Path traversal attempt detected for {File}.", storedFileName);
                return null;
            }

            if (!File.Exists(fullPath)) return null;
            // 3) تحديد الـ ContentType بالـ provider الجاهز
            if (!ContentTypeProvider.TryGetContentType(fullPath, out var contentType))
                contentType = "application/octet-stream"; 
            // 4) فتح الـ Stream بـ async I/O
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read,bufferSize: 81920, useAsync: true);
            return (stream, contentType);
        }

        #endregion

        #region Helpers

        private void TryDeletePartialFile(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to clean up partial file {Path}.", path);
            }
        }

        #endregion
    }
}