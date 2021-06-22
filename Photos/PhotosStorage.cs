using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Photos.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Photos.AnalyzerService.Abstractions;

namespace Photos
{
    public class PhotosStorage
    {
        [FunctionName("PhotosStorage")]
        public async Task<byte[]> Run(
            [ActivityTrigger] PhotoUploadModel request,
            [Blob("photos", FileAccess.ReadWrite, Connection = Literals.StorageConnectionString)] CloudBlobContainer blobContainer,
            [CosmosDB("photos", "metadata", ConnectionStringSetting = Literals.CosmosDbConnection, CreateIfNotExists = true)] IAsyncCollector<dynamic> items,
            ILogger logger)
        {
            var newId = Guid.NewGuid();
            var blobName = $"{newId}.jpg";
            await blobContainer.CreateIfNotExistsAsync();
            var photoBytes = Convert.FromBase64String(request.Photo);
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(blobName);
            await cloudBlockBlob.UploadFromByteArrayAsync(photoBytes, 0, photoBytes.Length);

            var item = new
            {
                id = newId,
                name = request.Name,
                description = request.Description,
                tags = request.Tags
            };
            await items.AddAsync(item);

            logger?.LogInformation($"Successfully uploaded {blobName} file and its metadata ");
            return photoBytes;
        }
    }
}
