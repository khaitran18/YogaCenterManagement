using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.CloudStorage
{
    public interface ICloudStorageService
    {
        Task<string> UploadFileAsync(IFormFile fileToUpLoad, string fileName);
    }
    public class CloudStorageService : ICloudStorageService
    {
        public CloudStorageService()
        {

        }
        public async Task<string> UploadFileAsync(IFormFile fileToUpLoad, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                await fileToUpLoad.CopyToAsync(memoryStream);
                using (var storageClient = StorageClient.Create(GoogleCredential.FromFile("yoga-guru-391213-5f27d3bece3a.json")))
                {
                    Random rnd = new Random();
                    var bucketName = "yoga-guru-391213.appspot.com";
                    var uploadedFile = await storageClient.UploadObjectAsync(bucketName, fileName+rnd.Next(), "image/jpeg", memoryStream);
                    string downloadUrl = $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(uploadedFile.Name)}?alt=media&token={uploadedFile.Generation}";
                    return downloadUrl;
                }

            }
        }
    }
}
