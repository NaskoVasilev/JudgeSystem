using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.IocConfiguration
{
    public static class AzureBlobStorageConfiguration
    {
        private const string StorageConnectionStringKey = "AzureBlob:StorageConnectionString";
        private const string ContainerNameKey = "AzureBlob:ContainerName";

        public static IServiceCollection ConfigureAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            string cloudStorageConnectionString = configuration[StorageConnectionStringKey];

            var storageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);
            services.AddSingleton(storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(configuration[ContainerNameKey]);
            services.AddSingleton(cloudBlobContainer);

            return services;
        }
    }
}
