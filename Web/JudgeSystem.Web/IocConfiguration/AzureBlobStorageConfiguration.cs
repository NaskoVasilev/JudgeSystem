﻿using JudgeSystem.Common;
using JudgeSystem.Common.Settings;

using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JudgeSystem.Web.IocConfiguration
{
    public static class AzureBlobStorageConfiguration
    {
        public static IServiceCollection ConfigureAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var azureBlobSettings = new AzureBlobSettings();
            configuration.GetSection(AppSettingsSections.AzureBlobSection).Bind(azureBlobSettings);

            var storageAccount = CloudStorageAccount.Parse(azureBlobSettings.StorageConnectionString);
            services.AddSingleton(storageAccount);

            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(azureBlobSettings.ContainerName);
            services.AddSingleton(cloudBlobContainer);

            return services;
        }
    }
}
