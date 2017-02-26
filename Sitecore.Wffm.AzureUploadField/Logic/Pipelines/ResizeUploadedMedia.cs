using ImageResizer;
using Sitecore.Diagnostics;
using System.IO;

namespace Sitecore.Wffm.AzureUploadField.Logic.Pipelines
{
    public class ResizeUploadedMedia : IFormClientUploadPipelineProcessor
    {
        private byte[] ResizeMedia(byte[] source, int maxWidth, out string aspectRatio)
        {
            using (var resizedImageStream = new MemoryStream())
            {
                string extension = "jpg";
                Stream stream = new MemoryStream(source);
                ImageResizer.ImageBuilder.Current.Build(stream, resizedImageStream, new ResizeSettings(maxWidth, 0, FitMode.Max, extension));
                resizedImageStream.Seek(0, SeekOrigin.Begin);
                using (System.Drawing.Image objImage = System.Drawing.Image.FromStream(resizedImageStream))
                {
                    aspectRatio = (((double)objImage.Height / (double)objImage.Width)*100).ToString();
                }
                resizedImageStream.Seek(0, SeekOrigin.Begin);
                return resizedImageStream.ToArray();
            }
        }

        public void Process(FormClientUploadPipelineArgs args)
        {
            if(args.StopProcessing)
            {
                return;
            }

            var isResized = args.FieldParameters.ContainsKey("resizemedia") && args.FieldParameters["resizemedia"] == "yes";

            if(isResized)
            {
                int width = 1000;
                int.TryParse(args.FieldParameters["maxwidth"], out width);

                Log.Info("AzureUploadField: Resizing media to " + width, this);

                string aspectRatioVal;

                args.PostedFile.Data = ResizeMedia(args.PostedFile.Data, 1000, out aspectRatioVal);

                args.AspectRatioValue = aspectRatioVal;

                Log.Info("AzureUploadField: Success" + width, this);
            }
        }
    }
}
