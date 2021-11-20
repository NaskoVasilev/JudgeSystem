using JudgeSystem.Common;
using JudgeSystem.Common.Settings;
using JudgeSystem.Services;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.Configuration
{
    public static class FileProviderConfiguration
    {
        public static IServiceCollection ConfigureFileProvider(this IServiceCollection services, IConfiguration configuration)
        {
            string selectedProvider = configuration["FileProvider"];

            if(selectedProvider == "AzureStorage")
            {
                //If someone try to start the application but have no azure storage account, just will skip adding azure storage related services to the DI container
                ConfigureAzureStorage(services, configuration);
                services.AddTransient<IFileStorageService, AzureStorageService>();
            }
            else if(selectedProvider == "LocalStorage")
            {
                // TODO:
                // Create the base directory if not exist
                services.AddTransient<IFileStorageService, LocalFileStorageService>();
            }

            return services;
        }

        private static void ConfigureAzureStorage(IServiceCollection services, IConfiguration configuration)
        {
            AzureBlobSettings azureBlobSettings = configuration.GetSection(AppSettingsSections.AzureBlobSection).Get<AzureBlobSettings>();

            if (azureBlobSettings == null)
            {
                return;
            }

            var storageAccount = CloudStorageAccount.Parse(azureBlobSettings.StorageConnectionString);
            services.AddSingleton(storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(azureBlobSettings.ContainerName);
            services.AddSingleton(cloudBlobContainer);
        }
    }
}
