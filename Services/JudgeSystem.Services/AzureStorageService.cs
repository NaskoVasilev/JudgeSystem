using System.IO;
using System.Threading.Tasks;

using Microsoft.Azure.Storage.Blob;

namespace JudgeSystem.Services
{
    public class AzureStorageService : IFileStorageService
    {
        private readonly CloudBlobContainer cloudBlobContainer;

        public AzureStorageService(CloudBlobContainer cloudBlobContainer)
        {
            this.cloudBlobContainer = cloudBlobContainer;
        }

        public async Task<string> Upload(Stream stream, string fileName, string folderName)
        {
            string filePath = $"{Path.GetRandomFileName()}_{fileName}";

            CloudBlockBlob cloudBlockBlob = await GetCloudBlockBlob(filePath);
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            return filePath;
        }

        public async Task Download(string filePath, Stream stream)
        {
            CloudBlockBlob cloudBlockBlob = await GetCloudBlockBlob(filePath);
            await cloudBlockBlob.DownloadToStreamAsync(stream);
        }

        public async Task Delete(string filePath)
        {
            CloudBlockBlob cloudBlockBlob = await GetCloudBlockBlob(filePath);
            await cloudBlockBlob.DeleteIfExistsAsync();
        }

        private async Task<CloudBlockBlob> GetCloudBlockBlob(string filePath)
        {
            CloudBlobContainer cloudBlobContainer = await ConfigureContainer();
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filePath);
            return cloudBlockBlob;
        }

        private async Task<CloudBlobContainer> ConfigureContainer()
        {
            await cloudBlobContainer.CreateIfNotExistsAsync();

            var permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            return cloudBlobContainer;
        }
    }
}
