
namespace GymManagementSystem.BLL.AttachmentServices
{
    public sealed class UploadResult
    {
        public bool IsSuccess { get; private init; }
        public string? StoredFileName { get; private init; }
        public string? Error { get; private init; }

        public static UploadResult Success(string storedFileName) => new() { IsSuccess = true, StoredFileName = storedFileName };

        public static UploadResult Failure(string error) =>new() { IsSuccess = false, Error = error };
    }
}
