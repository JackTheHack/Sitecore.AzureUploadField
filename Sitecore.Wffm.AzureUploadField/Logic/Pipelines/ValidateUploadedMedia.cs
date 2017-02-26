using Sitecore.Diagnostics;
using Sitecore.Globalization;
using System;
using System.IO;
using System.Linq;

namespace Sitecore.Wffm.AzureUploadField.Logic.Pipelines
{
    public class ValidateUploadedMedia : IFormClientUploadPipelineProcessor
    {
        public void Process(FormClientUploadPipelineArgs args)
        {
            int _fileSizeLimit = Sitecore.Configuration.Settings.GetIntSetting("AzureUploadField.FileSizeLimit", 10); //2MB
            string _extensions = Sitecore.Configuration.Settings.GetSetting("AzureUploadField.Extensions", ".jpg,.jpeg,.png,.bmp");

            var fileUpload = args.PostedFile;
            bool isValid = true;

            if(args.FieldParameters.ContainsKey("filesizelimit"))
            {
                _fileSizeLimit = int.Parse(args.FieldParameters["filesizelimit"]);
            }

            if(args.FieldParameters.ContainsKey("fileextensions"))
            {
                _extensions = args.FieldParameters["fileextensions"];
            }

            if (fileUpload != null && fileUpload.Data != null)
            {
                Log.Info("AzureUploadField: Validating uploaded media ", this);

                int sizeInBytes = fileUpload.Data.Length;
                int limit = _fileSizeLimit * 1024 * 1024;
                isValid = (sizeInBytes <= limit);

                Log.Info("AzureUploadField: Validate file size - " + sizeInBytes + " " + limit + " " + isValid, this);

                string[] extensions = _extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var extension = Path.GetExtension(fileUpload.FileName);
                if (extension != null && !extensions.Contains(extension.ToLower()))
                {
                    isValid = false;
                }

                Log.Info("AzureUploadField: Validate file extension- " + extension + " " + _extensions + " " + isValid, this);
            }

            if (!isValid)
            {
                args.StopProcessing = true;
                args.ReturnValue = new AzurePostedFile()
                {
                    ValidationError = Translate.Text("Story_Invalid_Image")
                };
            }

        }
    }
}
