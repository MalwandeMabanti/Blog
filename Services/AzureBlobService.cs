using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(IOptions<BlobServiceSettings> settings)
        {
            _blobServiceClient = new BlobServiceClient(settings.Value.ConnectionString);
        }

        public async Task<string> UploadFileAsync(string containerName, IFormFile file, string fileName)
        {
            BlobContainerClient containerClient;

            
            if (await _blobServiceClient.GetBlobContainerClient(containerName).ExistsAsync())
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            else
            {
                
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName, PublicAccessType.BlobContainer);
            }


          
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            
            using var uploadFileStream = file.OpenReadStream();

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentDisposition = "inline; filename=\"" + fileName + "\"",
                ContentType = file.ContentType
                
            };

            await blobClient.UploadAsync(uploadFileStream, httpHeaders);
            uploadFileStream.Close();

            
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
