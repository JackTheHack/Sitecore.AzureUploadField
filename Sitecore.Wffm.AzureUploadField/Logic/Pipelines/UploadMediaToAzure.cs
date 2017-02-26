using Microsoft.WindowsAzure.Storage;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using System;
using System.IO;

namespace Sitecore.Wffm.AzureUploadField.Logic.Pipelines
{
    public class UploadMediaToAzure : IFormClientUploadPipelineProcessor
    {
        public UploadMediaToAzure()
        {
        }       

        public string AzureConnectionString
        {
            get
            {
                return Settings.GetSetting("AzureUploadField.AzureConnectionString", "UseDevelopmentStorage=true");
            }
        }

        public string Upload(string container, string fileName, byte[] data)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureConnectionString);

            Log.Debug("Azure File Upload: Creating blob client...", this);
            var blobClient = storageAccount.CreateCloudBlobClient();
            
            Log.Debug("Azure File Upload: Getting container reference", this);
            var blobContainer = blobClient.GetContainerReference(container);
            blobContainer.CreateIfNotExists();
            var folder = blobContainer.GetDirectoryReference(DateTime.UtcNow.Date.ToShortDateString().Replace('/','-'));
            
            Log.Debug("Azure File Upload: Uploading...", this);
            
            var blob = folder.GetBlockBlobReference(Path.GetFileNameWithoutExtension(fileName)+ DateTime.UtcNow.TimeOfDay.Ticks.ToString() + Path.GetExtension(fileName));
            
            blob.UploadFromByteArray(data, 0, data.Length);
            
            return blob.Uri.ToString();
        }

        public void Process(FormClientUploadPipelineArgs args)
        {
            if(args.StopProcessing)
            {
                return;
            }

            Log.Debug("Azure File Upload: Uploading to blob storage...", this);

            string containerName = "default";

            if (args.FieldParameters.ContainsKey("azurecontainer"))
            {
                containerName = args.FieldParameters["azurecontainer"];
            }

            var file = args.PostedFile;

            Log.Debug("Azure File Upload: Container - " + containerName + "File - " + file.FileName + "...", this);

            var url = Upload(containerName, file.FileName, file.Data);

            Log.Debug("Azure File Upload: Success - " + url, this);

            args.ReturnValue = (object)new AzurePostedFile()
            {
                Name = file.FileName,
                Url = url,
                AspectRatio = args.AspectRatioValue
            };
        }
    }
}
