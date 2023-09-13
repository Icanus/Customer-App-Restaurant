using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Helpers
{
    public static class StrHelper
    {
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
        public static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
        public static int ComputeDurationInMinutes(string durationText)
        {
            // Split the duration text into words and filter numeric values
            var numericValues = durationText.Split()
                                           .Where(word => int.TryParse(word, out _))
                                           .Select(int.Parse);

            int totalMinutes = numericValues.Sum();
            return totalMinutes;
        }
        public static async Task<string> getUploadedImageString(string imagePath)
        {
            using (var fileStream = File.OpenRead(imagePath))
            {
                var account = CloudStorageAccount.Parse($"DefaultEndpointsProtocol=https;AccountName=foodappblobstorage;AccountKey={App.azureKeys};EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("images");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(fileStream);
                string url = blockBlob.Uri.OriginalString;
                return url;
            }

            return "";
        }

        public static async Task<string> deleteUploadedImageString(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return "";
            var account = CloudStorageAccount.Parse($"DefaultEndpointsProtocol=https;AccountName=foodappblobstorage;AccountKey={App.azureKeys};EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("images");

            Uri uri = new Uri(imagePath);
            var fileName = Path.GetFileName(uri.LocalPath);
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            var res = await blob.DeleteIfExistsAsync();
            return res.ToString();
        }
    }
}
