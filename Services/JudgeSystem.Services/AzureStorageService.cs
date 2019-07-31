using System;
using System.IO;
using System.Threading.Tasks;

using JudgeSystem.Web.Dtos.Common;

using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace JudgeSystem.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly CloudStorageAccount storageAccount;

        public AzureStorageService(CloudStorageAccount storageAccount)
        {
            this.storageAccount = storageAccount;
        }

        public async Task<string> Upload(CloudBlobStream stream, string fileExtension, string containerName)
        {
            CloudBlobContainer cloudBlobContainer = await GetBlobContainer(containerName);

            string fileName = $"{Guid.NewGuid().ToString()}.{fileExtension}";
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            return $"{containerName}/{fileName}";
        }

        public async Task Download(string filePath, Stream stream)
        {
            ResourceFileComponentsDto resourceFileComponents = GetResourceFileComponents(filePath);

            CloudBlobContainer cloudBlobContainer = await GetBlobContainer(resourceFileComponents.ContainerName);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(resourceFileComponents.FileName);
            await cloudBlockBlob.DownloadToStreamAsync(stream);
        }

        public async Task Delete(string filePath)
        {
            ResourceFileComponentsDto resourceFileComponents = GetResourceFileComponents(filePath);

            CloudBlobContainer cloudBlobContainer = await GetBlobContainer(resourceFileComponents.ContainerName);

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(resourceFileComponents.FileName);
            await cloudBlockBlob.DeleteIfExistsAsync();
        }

        private async Task<CloudBlobContainer> GetBlobContainer(string containerName)
        {
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await cloudBlobContainer.SetPermissionsAsync(permissions);
            return cloudBlobContainer;
        }

        private ResourceFileComponentsDto GetResourceFileComponents(string filePath)
        {
            int indexOfSlash = filePath.IndexOf('/');
            string containerName = filePath.Substring(0, indexOfSlash);
            string fileName = filePath.Substring(indexOfSlash + 1);

            return new ResourceFileComponentsDto { FileName = fileName, ContainerName = containerName };
        }
    }
}
