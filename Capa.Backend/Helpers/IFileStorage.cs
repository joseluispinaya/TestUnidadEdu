namespace Capa.Backend.Helpers
{
    public interface IFileStorage
    {
        Task<string> SaveFileAsync(byte[] content, string extention, string containerName);
        Task<string> UploadFileAsync(IFormFile file, string containerName);

        Task RemoveFileAsync(string path, string containerName);
    }
}
