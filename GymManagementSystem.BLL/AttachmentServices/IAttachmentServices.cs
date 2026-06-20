

namespace GymManagementSystem.BLL.AttachmentServices
{
    public interface IAttachmentServices
    {
        /// <summary>Uploads a file, returns the stored (GUID-based) file name on success.</summary>
        Task<UploadResult> UploadAsync( Stream fileStream, string fileName, string folderName, CancellationToken ct = default);

        /// <summary>Deletes a stored file. Returns true if deleted or already absent.</summary>
        Task<bool> DeleteAsync(string storedFileName, string folderName, CancellationToken ct = default);

        /// <summary>
        /// Opens a readable stream for a stored file along with its content type.
        /// Synchronous: only opens the stream — actual reading happens when the response is written.
        /// Returns null if the file is missing or the path is invalid.
        /// </summary>
        (Stream stream, string contentType)? GetFile(string storedFileName, string folderName);
    }
}